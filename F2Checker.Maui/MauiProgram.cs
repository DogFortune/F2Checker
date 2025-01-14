using F2Checker.Models;
using F2Checker.Services;
using F2Checker.ViewModels;
using F2Checker.Views;
using Microsoft.Extensions.Logging;

namespace F2Checker;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .RegisterServices()
            .RegisterViewModels()
            .RegisterViews();
#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }

    private static MauiAppBuilder RegisterServices(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddTransient<Models.AppContext>();
        mauiAppBuilder.Services.AddSingleton<INavigationService, NavigationService>();
        return mauiAppBuilder;
    }

    private static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<MainPageViewModel>();
        mauiAppBuilder.Services.AddSingleton<AboutPageViewModel>();
        return mauiAppBuilder;
    }

    private static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<MainPage>();
        mauiAppBuilder.Services.AddSingleton<AboutPage>();
        return mauiAppBuilder;
    }
}