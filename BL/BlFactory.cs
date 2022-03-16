using BLAPI;

namespace BL
{
    public static class BlFactory
    {
        public static BlApi GetBl()
        {
            return Bl.Instance;
        }
    }
}