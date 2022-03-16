using System.Linq;
using BL;


namespace ConsoleUI_BL
{
    public static class ListDisplayMethods
    {
        public static void DisplayStationList(Bl bl)
        {
            bl.GetStations().ToList().ForEach(s => bl.DisplayStation(s));
        }

        public static void DisplayDroneList(Bl bl)
        {
            bl.GetDrones().ToList().ForEach(d => bl.DisplayDrone(d));
        }

        public static void DisplayCustomerList(Bl bl)
        {
            bl.GetCustomers().ToList().ForEach(c => bl.DisplayCustomer(c));
        }

        public static void DisplayParcelList(Bl bl)
        {
            bl.GetParcels().ToList().ForEach(p => bl.DisplayParcel(p));
        }
    }
}
