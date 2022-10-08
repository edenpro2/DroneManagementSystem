using DalFacade;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;


namespace DALXML
{
    public partial class DalXml : DalApi
    {
        private static readonly Lazy<DalXml> _instance = new(() => new DalXml());
        public static DalXml Instance => _instance.Value;

        private const string Dir = @"../../../../data/";

        private const string DronesFilePath = Dir + "Drones.xml";
        private const string StationsFilePath = Dir + "Stations.xml";
        private const string CustomersFilePath = Dir + "Customers.xml";
        private const string ParcelsFilePath = Dir + "Parcels.xml";
        private const string UsersFilePath = Dir + "Users.xml";
        private const string ChatsFilePath = Dir + "Chats.xml"; 

        internal static class Config
        {
            // Consumption is per kilometer
            internal const double ConsumptionWhenFree = 0.1;
            internal const double ConsumptionWhenLight = 0.13;
            internal const double ConsumptionWhenMid = 0.17;
            internal const double ConsumptionWhenHeavy = 0.2;

            // Charge rate per hour
            internal const double ChargingRate = 15.0;

            internal static int ParcelId;
            internal static int CustomerId;
        }

        static DalXml()
        {
            if (!Directory.Exists(Dir))
            {
                Directory.CreateDirectory(Dir);
            }
        }

        private DalXml()
        {
            if (!File.Exists(DronesFilePath))
            {
                CreateXmlDoc(DronesFilePath, DalObject.DalObject.Instance.GetDrones().ToList());
            }

            if (!File.Exists(StationsFilePath))
            {
                CreateXmlDoc(StationsFilePath, DalObject.DalObject.Instance.GetStations().ToList());
            }

            if (!File.Exists(ParcelsFilePath))
            {
                CreateXmlDoc(ParcelsFilePath, DalObject.DalObject.Instance.GetParcels().ToList());
            }

            if (!File.Exists(CustomersFilePath))
            {
                CreateXmlDoc(CustomersFilePath, DalObject.DalObject.Instance.GetCustomers().ToList());
            }

            if (!File.Exists(UsersFilePath))
            {
                CreateXmlDoc(UsersFilePath, DalObject.DalObject.Instance.GetUsers().ToList());
            }

            if (!File.Exists(ChatsFilePath))
            {
                CreateXmlDoc(ChatsFilePath, DalObject.DalObject.Instance.GetChats().ToList());
            }


            Config.CustomerId = GetCustomers().Count();
            Config.ParcelId = GetParcels().Count();
        }

        private static void CreateXmlDoc<T>(string filePath, List<T> list)
        {
            var file = new FileStream(filePath, FileMode.Create);
            var serializer = new XmlSerializer(list.GetType());
            serializer.Serialize(file, list);
            file.Close();
        }

        private static IEnumerable<T> Load<T>(string filePath)
        {
            var serializer = new XmlSerializer(typeof(List<T>));
            using var file = new FileStream(filePath, FileMode.Open);
            var list = (IEnumerable<T>)serializer.Deserialize(file);
            file.Close();
            return list;
        }

        private static void Save<T>(string fileName, List<T> list)
        {
            var serializer = new XmlSerializer(list.GetType());
            using var writer = new StreamWriter(fileName);
            serializer.Serialize(writer, list);
        }

        // Power consumption method
        [MethodImpl(MethodImplOptions.Synchronized)]
        public double[] RequestPowerConsumption()
        {
            return new[]
            {
                Config.ConsumptionWhenFree,
                Config.ConsumptionWhenLight,
                Config.ConsumptionWhenMid,
                Config.ConsumptionWhenHeavy,
                Config.ChargingRate
             };
        }
    }
}