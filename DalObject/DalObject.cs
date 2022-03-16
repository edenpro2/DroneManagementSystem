using System;
using System.Runtime.CompilerServices;

namespace DAL
{
    public partial class DalObject
    {
        private static readonly Lazy<DalObject> _instance = new(() => new DalObject());

        public static DalObject Instance => _instance.Value;

        private DalObject()
        {
            DataSource.Initialize();
        }

        // Power consumption method
        [MethodImpl(MethodImplOptions.Synchronized)]
        public double[] RequestPowerConsumption()
        {
            return new[]
            {
                DataSource.Config.ConsumptionWhenFree,
                DataSource.Config.ConsumptionWhenLight,
                DataSource.Config.ConsumptionWhenMid,
                DataSource.Config.ConsumptionWhenHeavy,
                DataSource.Config.ChargingRate
            };
        }
    }
}