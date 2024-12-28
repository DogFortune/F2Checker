using F2Checker.Core.Hashing;

namespace F2Checker.Console;

class Program
{
    static async Task Main(string[] args)
    {
        var model = new XxHashProvider();
        var progress = new Progress<string>(System.Console.WriteLine);
        var dir = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
        var hash = await model.GetHashAsync(Path.Combine(dir, "test.mp4"), progress, CancellationToken.None);
        System.Console.WriteLine(Convert.ToHexString(hash).ToLower());
    }
}