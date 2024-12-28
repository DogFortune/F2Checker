// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace F2Checker.Core.Hashing;

public interface IHashProvider
{
    public Task<byte[]> GetHashAsync(string filename, IProgress<string> p, CancellationToken token);
}