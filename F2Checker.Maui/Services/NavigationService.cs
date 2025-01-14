using F2Checker.Views;

namespace F2Checker.Services;

public class NavigationService : INavigationService
{
    public NavigationService(IServiceProvider services) => _services = services;

    readonly IServiceProvider _services;

    protected INavigation Navigation
    {
        get
        {
            var navigation = Application.Current?.MainPage?.Navigation;
            if (navigation is not null)
                return navigation;
            throw new NullReferenceException();
        }
    }

    public Task NavigateToAboutPage() => NavigateToPage<AboutPage>();

    public Task NavigateToPage<T>() where T : Page
    {
        var page = ResolvePage<T>();
        return Navigation.PushAsync(page, true);
    }

    public Task NavigateBack()
    {
        if (Navigation.NavigationStack.Count > 1)
            //前ページへ戻る
            return Navigation.PopAsync();
        throw new InvalidOperationException("No pages to navigate back to!");
    }

    private T ResolvePage<T>() where T : Page => _services.GetService<T>();
}