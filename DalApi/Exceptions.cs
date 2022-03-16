using System;

namespace DALFACADE
{
    [Serializable]
    public class DALNotFoundException : Exception
    {
        public DALNotFoundException()
        {
        }

        public DALNotFoundException(string message) : base("Item not found in list")
        {
        }
    }
}