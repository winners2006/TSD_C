namespace TSDLestate
{
	public partial class MainPage : ContentPage
	{

		public MainPage()
		{
			InitializeComponent();

			GetResult();
		}

		private async Task GetResult()
		{
			List<Movementos> movements = await HttpClientTSD.GetMovementos(this);
			listViewMovemented.ItemsSource = movements;
		}

		private void ClicMovemented(object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem != null)
			{
				// Здесь можно обработать выбор элемента
				var selectedMovement = (Movementos)e.SelectedItem;
				HttpClientTSD.postGUID = selectedMovement.GUID;
				Navigation.PushAsync(new MoveGoods());
			}
		}

		public void Error(string error) 
		{
		    textMessege.Text = error;
		}
	}
}
