using System.Buffers;
using CommunityToolkit.Mvvm.ComponentModel;
using F2Checker.Core.Hashing;
using Foundation;
using MemoryPack;

namespace F2Checker.Models;

public class AppContext : ObservableObject
{
    public AppContext()
    {
        _firstFilePath = string.Empty;
        _firstFileHash = string.Empty;
        _status = string.Empty;
        FirstFilePath = string.Empty;
        Status = string.Empty;
        HashProvider = new XxHashProvider();
        LoadSetting();
    }

    /// <summary>
    ///     設定を読み込みます。もし設定ファイルが無い場合は新規に作成します。
    /// </summary>
    /// <exception cref="DirectoryNotFoundException">設定ファイルの保存先が見つかりません</exception>
    public void LoadSetting()
    {
        if (string.IsNullOrEmpty(AppDirectory))
        {
            throw new DirectoryNotFoundException(AppDirectory);
        }

        var appSettingsPath = Path.Combine(AppDirectory, SettingFileName);

        if (!File.Exists(appSettingsPath))
        {
            SaveSetting(appSettingsPath);
        }
        else
        {
            var bytes = File.ReadAllBytes(appSettingsPath);
            var loadSetting = MemoryPackSerializer.Deserialize<AppSettings>(bytes);
            if (loadSetting == null)
                throw new FileNotFoundException(appSettingsPath);
            AppSetting.Update(loadSetting);
        }
    }

    public void SaveSetting(string appSettingsPath)
    {
        var bytes = MemoryPackSerializer.Serialize(AppSetting);
        File.WriteAllBytes(appSettingsPath, bytes);
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
        var hashValue = await HashProvider.GetHashAsync(filename, progress, token).ConfigureAwait(false);
        return Convert.ToHexString(hashValue).ToLower();
    }

    /// <summary>
    ///     ハッシュプロバイダー
    /// </summary>
    private IHashProvider HashProvider { get; }

    private AppSettings AppSetting { get; } = new();

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

    /// <summary>
    ///     設定ファイル保存先パス
    ///     https://learn.microsoft.com/ja-jp/dotnet/maui/macios/system-special-folders?view=net-maui-8.0
    /// </summary>
    private string AppDirectory { get; } = new NSFileManager().GetUrls(
        NSSearchPathDirectory.ApplicationSupportDirectory,
        NSSearchPathDomain.User)[0].Path;

    /// <summary>
    ///     設定ファイル名
    /// </summary>
    private string SettingFileName { get; } = "Setting.mep";
}