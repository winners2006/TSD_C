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
			string username = usernameEntry.Text;
			string password = passwordEntry.Text;

			IHttpService httpService = new HttpClientTSD(username, password);
			await Navigation.PushAsync(new MainPage(httpService));
		}
	}
}
