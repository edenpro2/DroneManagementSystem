using DAL;
using DALFACADE;
using DALXML;
using System;

namespace BL
{
    public static class DalFactory
    {
        public static DalApi GetDal(string param)
        {
            switch (param)
            {
                case "DalObject":
                    return DalObject.Instance;
                case "DalXml":
                    return DalXml.Instance;
                default:
                    throw new ArgumentException("Invalid parameter");
            }
        }
    }
}