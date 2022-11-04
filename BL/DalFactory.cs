using DalFacade;
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
                // If namespace doesn't match DalObject (or v.v.), error
                case nameof(DalObject):
                    return DalObject.DalObject.Instance;
                case nameof(DalXml):
                    return DalXml.Instance;
                default:
                    throw new ArgumentException("Invalid parameter");
            }
        }
    }
}