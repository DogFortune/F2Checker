namespace F2Checker.ViewModels;

public class AboutPageViewModel
{
    public AboutPageViewModel()
    {
        Task.Run(LoadAsset);
    }

    private async Task LoadAsset()
    {
        await using var stream = await Task.Run(() => FileSystem.OpenAppPackageFileAsync("THANKS.md"));
        using var reader = new StreamReader(stream);
        Thanks = await reader.ReadToEndAsync();
    }

    public string Version { get; set; } = AppInfo.Current.VersionString;

    public string Copyright { get; set; } = "Copyright (c) 2024 DogFortune";
    public string Thanks { get; set; } = string.Empty;
}