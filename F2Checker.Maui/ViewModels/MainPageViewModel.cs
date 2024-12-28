using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using AppContext = F2Checker.Models.AppContext;

namespace F2Checker.ViewModels;

public class MainPageViewModel : ViewModelBase
{
    public MainPageViewModel(AppContext model)
    {
        Model = model;
        CancellationToken = CancellationToken.None;
        FirstFilePath = Model.ObserveProperty(m => m.FirstFilePath)
            .ToReadOnlyReactivePropertySlim()
            .AddTo(CompositeDisposable);
        FirstFileHash = Model.ObserveProperty(m => m.FirstFileHash)
            .ToReadOnlyReactivePropertySlim()
            .AddTo(CompositeDisposable);

        SelectedFirstFile = new AsyncReactiveCommand()
            .WithSubscribe(
                async () => { Model.FirstFilePath = await SelectedFileAsync(); }
            )
            .AddTo(CompositeDisposable);

        CheckSum = new AsyncReactiveCommand()
            .WithSubscribe(() => Model.CheckSum(CancellationToken))
            .AddTo(CompositeDisposable);

        Status = Model.ObserveProperty(m => m.Status)
            .ToReadOnlyReactivePropertySlim()
            .AddTo(CompositeDisposable);
    }

    /// <summary>
    ///     FilePickerによるメディア選択
    /// </summary>
    /// <returns></returns>
    private async Task<string> SelectedFileAsync()
    {
        try
        {
            var result =
                await LukeMauiFilePicker.FilePickerService.Instance.PickFileAsync("Open File", null);
            if (result is { FileResult: not null })
            {
                return result.FileResult.FullPath;
            }
        }
        // TODO: Pokemon
        catch (Exception ex)
        {
            Console.WriteLine($"Error picking file: {ex.Message}");
        }

        return string.Empty;
    }

    /// <summary>
    ///     モデル
    /// </summary>
    private AppContext Model { get; }

    /// <summary>
    ///     ファイル選択コマンド（1つ目）
    /// </summary>
    public AsyncReactiveCommand SelectedFirstFile { get; }

    /// <summary>
    ///     ファイル選択コマンド（2つ目）
    /// </summary>
    public AsyncReactiveCommand SelectedSecondFile { get; }

    /// <summary>
    ///     1つ目のファイルパス
    /// </summary>
    public ReadOnlyReactivePropertySlim<string?> FirstFilePath { get; }

    /// <summary>
    ///     2つ目のファイルパス
    /// </summary>
    public ReadOnlyReactivePropertySlim<string?> SecondFilePath { get; }

    public ReadOnlyReactivePropertySlim<string?> FirstFileHash { get; }

    public ReactivePropertySlim<string?> SecondFileHash { get; }

    public AsyncReactiveCommand CheckSum { get; }

    public ReadOnlyReactivePropertySlim<double> Progress { get; }

    public ReadOnlyReactivePropertySlim<string?> Status { get; }

    private CancellationToken CancellationToken { get; }
}