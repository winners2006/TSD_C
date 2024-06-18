namespace TSDLestate
{
	public partial class MoveGoods : ContentPage
	{
		private readonly IHttpService _httpService;
		private readonly string _postGUID;

		public MoveGoods(IHttpService httpService, string postGUID)
		{
			_httpService = httpService;
			_postGUID = postGUID;
			InitializeComponent();
			GetResult();
#if ANDROID
			BroadcastReceiverTest.OnScanReceived += Scanning;
#endif
		}

		private async Task GetResult()
		{
			try
			{
				List<MovementGoods> movementgoods = await _httpService.GetMovementosGoodsAsync(_postGUID);
				listViewMoveGoods.ItemsSource = movementgoods;
			}
			catch (Exception ex)
			{
				Error(ex.Message);
			}
		}

		private void Scanning(string scanData)
		{
			MovementGoods? foundItem = listViewMoveGoods.ItemsSource.Cast<MovementGoods>()
										.FirstOrDefault(item => int.Parse(item.Barcode) == int.Parse(scanData));

			if (foundItem != null)
			{
				foundItem.Count++;
				textMessege.Text = $"Наименование: {foundItem.Name}.  Артикул: {foundItem.VendorCode}.";
				count.Text = foundItem.Count.ToString();
				quantity.Text = foundItem.Quantity.ToString();
				UpdateListView(foundItem);
			}
			else
			{
				textMessege.Text = $"Штрих-код {scanData} не найден.";
			}
		}

		private void UpdateListView(MovementGoods foundItem)
		{
			int index = -1;
			List<MovementGoods> items = listViewMoveGoods.ItemsSource as List<MovementGoods>;
			for (int i = 0; i < items.Count; i++)
			{
				if (items[i].Barcode == foundItem.Barcode)
				{
					index = i;
					break;
				}
			}

			if (index != -1)
			{
				items[index] = foundItem;
				listViewMoveGoods.ItemsSource = null;
				listViewMoveGoods.ItemsSource = items;
			}
		}

		public void Error(string error)
		{
			textMessege.Text = error;
		}

		public void SendResult()
		{
			textMessege.Text = "Приходный ордер создан!";
			Navigation.PushAsync(new MainPage(_httpService));
		}

		private async void Button_Clicked(object sender, EventArgs e)
		{
			var itemStrings = listViewMoveGoods.ItemsSource.Cast<MovementGoods>()
						.Select(item => $"{item.Batch} - {item.Count}")
						.ToList();
			string combinedString = string.Join(Environment.NewLine, itemStrings);

			try
			{
				await _httpService.SetMovementosGoodsAsync(_postGUID, combinedString);
				SendResult();
			}
			catch (Exception ex)
			{
				Error(ex.Message);
			}
		}
	}
}