 using ShopApp.Views;
using ShopApp.DataAccess;

namespace ShopApp;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
		MainPage = new AppShell();
	}
}
