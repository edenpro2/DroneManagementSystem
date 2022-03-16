using System;

namespace DalFacade
{
    [Serializable]
    public class DalNotFoundException : Exception
    {
        public DalNotFoundException() : base("Item not found in list") {}

    }
}