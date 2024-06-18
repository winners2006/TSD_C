namespace TSDLestate
{
	public partial class MainPage : ContentPage
	{
		private readonly IHttpService _httpService;

		public MainPage(IHttpService httpService)
		{
			_httpService = httpService;
			InitializeComponent();
			GetResult();
		}

		private async Task GetResult()
		{
			try
			{
				List<Movementos> movements = await _httpService.GetMovementosAsync();
				listViewMovemented.ItemsSource = movements;
			}
			catch (Exception ex)
			{
				Error(ex.Message);
			}
		}

		private void ClicMovemented(object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem != null)
			{
				var selectedMovement = (Movementos)e.SelectedItem;
				Navigation.PushAsync(new MoveGoods(_httpService, selectedMovement.GUID));
			}
		}

		public void Error(string error)
		{
			textMessege.Text = error;
		}
	}
}
