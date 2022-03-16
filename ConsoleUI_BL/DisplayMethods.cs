using System;
using BL;
using DO;

namespace ConsoleUI_BL
{
    public static class DisplayMethods
    {
        public static void DisplayStation(Bl bl)
        {
            int id;
            Console.WriteLine("Enter");
            Console.Write("Station ID:");
            while (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Not valid");
            }

            Station station = bl.SearchForStationById(id);

            bl.DisplayStation(station);
        }

        public static void DisplayDrone(Bl bl)
        {
            int id;
            Console.WriteLine("Enter");
            Console.Write("Drone ID:");
            while (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Not valid");
            }

            Drone drone = bl.SearchForDroneById(id);
            
            bl.DisplayDrone(drone);
        }

        public static void DisplayCustomer(Bl bl)
        {
            int id;
            Console.WriteLine("Enter");
            Console.Write("Customer ID:");
            while (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Not valid");
            }

            Customer customer = bl.SearchForCustomerById(id);
            
            bl.DisplayCustomer(customer);
        }

        public static void DisplayParcel(Bl bl)
        {
            int id;
            Console.WriteLine("Enter");
            Console.Write("Parcel ID:");
            while (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Not valid");
            }

            Parcel parcel = bl.SearchForParcelById(id);
            
            bl.DisplayParcel(parcel);
        }
    }
}
