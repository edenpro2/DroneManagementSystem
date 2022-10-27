using BenchmarkDotNet.Attributes;

namespace BL
{
    public static class BlFactory
    {
        public static BlApi GetBl() => Bl.Instance;
    }
}