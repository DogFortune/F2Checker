using F2Checker.Core.Hashing;
using Prism.Mvvm;

namespace F2Checker.Models;

public class AppContext : BindableBase
{
    public AppContext()
    {
        FirstFilePath = string.Empty;
        Status = string.Empty;
        HashProvider = new XxHashProvider();
    }

    /// <summary>
    ///     ハッシュ値計算の実行
    /// </summary>
    public async Task CheckSum(CancellationToken token)
    {
        // TODO: 仮実装で1つ目のファイルのみ計算。本来はパスの空チェックとかやる。
        FirstFileHash = await GetHashAsync(FirstFilePath, token).ConfigureAwait(false);
    }

    private async Task<string> GetHashAsync(string filename, CancellationToken token)
    {
        if (token.IsCancellationRequested)
            return string.Empty;
        var progress = new Progress<string>(m => Status = m);
        var hashbyte = await HashProvider.GetHashAsync(filename, progress).ConfigureAwait(false);
        return Convert.ToHexString(hashbyte).ToLower();
    }

    /// <summary>
    ///     ハッシュプロバイダー
    /// </summary>
    private XxHashProvider HashProvider { get; }

    private string _firstFilePath;

    /// <summary>
    ///     解析対象のファイルパス
    /// </summary>
    public string FirstFilePath

    {
        get => _firstFilePath;
        set => SetProperty(ref _firstFilePath, value);
    }

    private string _firstFileHash;

    public string FirstFileHash
    {
        get => _firstFileHash;
        set => SetProperty(ref _firstFileHash, value);
    }

    private string _status;

    /// <summary>
    ///     ステータス
    /// </summary>
    public string Status
    {
        get => _status;
        set => SetProperty(ref _status, value);
    }
}