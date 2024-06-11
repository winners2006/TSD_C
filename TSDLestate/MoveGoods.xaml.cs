namespace TSDLestate;

public partial class MoveGoods : ContentPage
{
	public MoveGoods()
	{
		InitializeComponent();
		GetResult();
#if ANDROID
		BroadcastReceiverTest.OnScanReceived += Scanning;
#endif
	}

	private async Task GetResult()
	{
		List<MovementGoods> movementgoods = await HttpClientTSD.GetMovementosGoods(HttpClientTSD.postGUID, this);
		listViewMoveGoods.ItemsSource = movementgoods;
	}

	private void Scanning(string scanData)
	{
		MovementGoods? foundItem = listViewMoveGoods.ItemsSource.Cast<MovementGoods>()
										.FirstOrDefault(item => int.Parse(item.Barcode) == int.Parse(scanData));

		if (foundItem != null)
		{
			foundItem.Count++; // ����������� �������
			textMessege.Text = $"������������: {foundItem.Name}.  �������: {foundItem.VendorCode}.";
			count.Text = foundItem.Count.ToString();
			quantity.Text = foundItem.Quantity.ToString();
			UpdateListView(foundItem);
		}
		else
		{
			// �����-��� �� ������ � ������
			textMessege.Text = $"�����-��� {scanData} �� ������.";
		}
	}

	private void UpdateListView(MovementGoods foundItem)
	{
		// �������� ������ ���������� ��������
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

		// ��������� ������� � ������
		if (index != -1)
		{
			items[index] = foundItem;
			// ��������� ItemsSource ListView
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
		textMessege.Text = "��������� ����� ������!";

		Navigation.PushAsync(new MainPage());
	}

	private async void Button_Clicked(object sender, EventArgs e)
	{
		var itemStrings = listViewMoveGoods.ItemsSource.Cast<MovementGoods>()
						.Select(item => $"{item.Batch} - {item.Count}")
						.ToList();
		string combinedString = string.Join(Environment.NewLine, itemStrings);

		await HttpClientTSD.SetMovementosGoods(HttpClientTSD.postGUID, this, combinedString);
	}
}