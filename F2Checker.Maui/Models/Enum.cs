// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace F2Checker.Models;

/// <summary>
///     モード
/// </summary>
public enum ApplicationMode
{
    /// <summary>
    ///     ２つのファイルのハッシュを比較するモード
    /// </summary>
    Compare,

    /// <summary>
    ///     ユーザーが入力したハッシュを照合するモード
    /// </summary>
    Verify
}

/// <summary>
///     ハッシュアルゴリズム
/// </summary>
public enum HashAlgorithm
{
    XxHash128,

    Sha256
}