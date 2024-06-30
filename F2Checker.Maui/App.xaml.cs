using F2Checker.Views;

namespace F2Checker;

public partial class App : Application
{
    public App(MainPage page)
    {
        InitializeComponent();
        MainPage = new NavigationPage(page);
    }
}