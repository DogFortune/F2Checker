using System;
using System.Buffers;
using System.IO;
using System.IO.Hashing;
using System.Threading;
using System.Threading.Tasks;
using ByteSizeLib;
using ReadFullBufferFileStream;

namespace F2Checker.Core.Hashing;

public class XxHashProvider : IHashProvider
{
    public async Task<byte[]> GetHashAsync(string filename, IProgress<string> p, CancellationToken token)
    {
        // ディレクトリ構造も含めてコピーする方法
        // var dir = "/Users/r0052005/Movies/CM_Sozai/";
        // var files = Directory.GetFiles(dir, "*", SearchOption.AllDirectories);
        // var dest = new List<string>();
        // var dest_relative = new List<string>();
        //
        // foreach (string file in files)
        // {
        //     dest_relative.Add(Path.GetRelativePath(dir, file));
        // }
        var buffer = ArrayPool<byte>.Shared.Rent(BufferSize);
        try
        {
            using (var entryStream = File.OpenRead(filename))
            {
                var totalBytesRead = 0L;
                var bytesRead = await entryStream.ReadFullBufferAsync(buffer, token).ConfigureAwait(false);
                var startTime = DateTime.Now;
                while (bytesRead > 0)
                {
                    HashAlgorithm.Append(buffer.AsSpan(0, bytesRead));
                    totalBytesRead += bytesRead;
                    var speed = ByteSize.FromBytes(totalBytesRead / (DateTime.Now - startTime).TotalSeconds);
                    p.Report(
                        $"{speed.LargestWholeNumberBinaryValue:#.##} {speed.LargestWholeNumberBinarySymbol}/s");
                    bytesRead = await entryStream.ReadFullBufferAsync(buffer, token).ConfigureAwait(false);
                }
            }
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }

        return HashAlgorithm.GetHashAndReset();
    }

    /// <summary>
    ///     ハッシュアルゴリズム
    /// </summary>
    private XxHash3 HashAlgorithm { get; } = new();

    /// <summary>
    ///     バッファサイズ
    /// </summary>
    private int BufferSize { get; } = 1 << 20;
}