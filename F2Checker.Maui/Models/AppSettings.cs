// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using MemoryPack;

namespace F2Checker.Models;

[MemoryPackable]
public partial class AppSettings
{
    public AppSettings()
    {
        ApplicationMode = ApplicationMode.Compare;
        HashAlgorithm = HashAlgorithm.XxHash128;
    }

    /// <summary>
    ///     値の更新
    /// </summary>
    /// <param name="appSettings">値を更新したインスタンス</param>
    public void Update(AppSettings appSettings)
    {
        ApplicationMode = appSettings.ApplicationMode;
        HashAlgorithm = appSettings.HashAlgorithm;
    }

    /// <summary>
    ///     モード
    /// </summary>
    public ApplicationMode ApplicationMode { get; set; }

    /// <summary>
    ///     ハッシュアルゴリズム
    /// </summary>
    public HashAlgorithm HashAlgorithm { get; set; }
}