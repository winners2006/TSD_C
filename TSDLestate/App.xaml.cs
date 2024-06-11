using Microsoft.Maui.Controls;

namespace TSDLestate
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			MainPage = new NavigationPage(new LoginPage());
		}
	}
}
