using BenchmarkDotNet.Running;

namespace XboxAuthNet.Game.Benchmarks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Benchmarks>();
        }
    }
}
