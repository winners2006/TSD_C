namespace TSDLestate
{
	public partial class LoginPage : ContentPage
	{
		public LoginPage()
		{
			InitializeComponent();
		}

		private void OnEntryFocused(object sender, FocusEventArgs e)
		{
			if (sender is Entry entry)
			{
				entry.Placeholder = string.Empty;
			}
		}

		private async void OnLoginButtonClicked(object sender, EventArgs e)
		{
			HttpClientTSD.username = usernameEntry.Text;
			HttpClientTSD.password = passwordEntry.Text;

			
			Navigation.PushAsync(new MainPage());
		}
	}
}
