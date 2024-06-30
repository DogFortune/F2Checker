using System;
using System.Buffers;
using System.IO;
using System.IO.Hashing;
using System.Threading.Tasks;
using ByteSizeLib;

namespace F2Checker.Core.Hashing
{
    public class XxHashProvider
    {
        private const int BufferSize = 1 << 20;

        public async Task<byte[]> GetHashAsync(string filename, IProgress<string> p)
        {
            var buffer = ArrayPool<byte>.Shared.Rent(BufferSize);
            using (var entryStream = File.OpenRead(filename))
            {
                var xxhash = new XxHash64();
                var totalBytesRead = 0L;
                var bytesRead = await entryStream.ReadAtLeastAsync(buffer, BufferSize).ConfigureAwait(false);
                var startTime = DateTime.Now;
                while (bytesRead > 0)
                {
                    xxhash.Append(buffer.AsSpan(0, bytesRead));
                    totalBytesRead += bytesRead;
                    var speed = ByteSize.FromBytes(totalBytesRead / (DateTime.Now - startTime).TotalSeconds);
                    p.Report(
                        $"{speed.LargestWholeNumberBinaryValue:#.##} {speed.LargestWholeNumberBinarySymbol}/s");
                    bytesRead = await entryStream.ReadAtLeastAsync(buffer, BufferSize, false).ConfigureAwait(false);
                }

                ArrayPool<byte>.Shared.Return(buffer);
                return xxhash.GetHashAndReset();
            }
        }
    }
}