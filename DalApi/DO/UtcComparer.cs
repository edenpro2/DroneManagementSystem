using System.Collections.Generic;

namespace DalFacade.DO
{
    class UtcComparer : IComparer<long>
    {
        public int Compare(long x, long y)
        {
            // use the default comparer to do the original comparison for datetimes
            var ascendingResult = Comparer<long>.Default.Compare(x, y);

            // turn the result around
            return 0 - ascendingResult;
        }
    }
}
