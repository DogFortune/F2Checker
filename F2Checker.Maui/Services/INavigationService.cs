namespace F2Checker.Services;

public interface INavigationService
{
    Task NavigateToAboutPage();
    Task NavigateBack();
}