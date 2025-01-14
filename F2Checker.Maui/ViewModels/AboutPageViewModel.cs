using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace F2Checker.ViewModels;

public class AboutPageViewModel : ViewModelBase
{
    public AboutPageViewModel()
    {
        Version = new ReactivePropertySlim<string>(AppInfo.Current.VersionString).ToReadOnlyReactivePropertySlim()
            .AddTo(CompositeDisposable);
        Copyright = new ReactivePropertySlim<string>("Copyright (c) 2024 DogFortune").ToReadOnlyReactivePropertySlim()
            .AddTo(CompositeDisposable);

        Task.Run(LoadAsset);
    }

    private async Task LoadAsset()
    {
        await using var stream = await Task.Run(() => FileSystem.OpenAppPackageFileAsync("THANKS.md"));
        using var reader = new StreamReader(stream);
        var text = await reader.ReadToEndAsync();
        if (!string.IsNullOrEmpty(text))
            Thanks = new ReactivePropertySlim<string>(text).ToReadOnlyReactivePropertySlim().AddTo(CompositeDisposable);
    }

    public ReadOnlyReactivePropertySlim<string> Version { get; }
    public ReadOnlyReactivePropertySlim<string> Copyright { get; }
    public ReadOnlyReactivePropertySlim<string> Thanks { get; set; }
}