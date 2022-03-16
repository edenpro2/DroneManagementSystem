using System;
using System.Collections.Generic;
using DO;
using Dal;
using BL;

namespace ConsoleUI_BL
{
    public static class AddingMethods
    {
        public static void AddStation(Bl bl)
        {
            int id, name, openSlots;
            double lat, lon;

            Console.WriteLine("Enter:");
            Console.Write("Station ID: ");
            while (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Not valid");
            }
            Console.Write("Station Name: ");
            while (!int.TryParse(Console.ReadLine(), out name))
            {
                Console.WriteLine("Not valid");
            }
            Console.Write("Open Slots: ");
            while (!int.TryParse(Console.ReadLine(), out openSlots))
            {
                Console.WriteLine("Not valid");
            }
            Console.Write("Latitude: ");
            while (!double.TryParse(Console.ReadLine(), out lat))
            {
                Console.WriteLine("Not valid");
            }
            Console.Write("Longitude: ");
            while (!double.TryParse(Console.ReadLine(), out lon))
            {
                Console.WriteLine("Not valid");
            }

            bl.AddStation(new(id, name, openSlots, lat, lon));

        }

        public static void AddDrone(Bl bl)
        {
            int id;
            string name;
            WeightCategories max;

            Console.WriteLine("Enter:");
            Console.Write("Drone ID: ");
            while (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Not valid");
            }
            Console.Write("Drone Name: ");
            name = Console.ReadLine();
            Console.Write("Max weight: Light - 0, Mid - 1, Heavy - 2");
            while (!Enum.TryParse(Console.ReadLine(), out max))
            {
                Console.WriteLine("Not valid");
            }

            Drone drone = new(id, name, max);

            List<Station> stations = (List<Station>)bl.GetStations();
            Station station = Randomize.Station(stations, new());

            bl.AddDrone(drone, station.id);
        }

        public static void AddCustomer(Bl bl)
        { 
            int id;
            string name, phone;
            double lat, lon;

            Console.WriteLine("Enter:");
            Console.Write("Customer ID: ");
            while (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Not valid");
            }
            Console.Write("First and Last Name: ");
            name = Console.ReadLine();
            Console.Write("Phone number: ");
            phone = Console.ReadLine();
            Console.Write("Latitude: ");
            while (!double.TryParse(Console.ReadLine(), out lat))
            {
                Console.WriteLine("Not valid");
            }
            Console.Write("Longitude: ");
            while (!double.TryParse(Console.ReadLine(), out lon))
            {
                Console.WriteLine("Not valid");
            }

            bl.AddCustomer(new(id,name,phone,lat,lon));

        }

        public static void AddParcel(Bl bl)
        {
     
            int senderId, targetId, id = bl.DalObj.GetCurrentPID();
            WeightCategories weight;
            Priorities priority;

            Console.WriteLine("Enter:");
            Console.Write("Sender ID: ");
            while (!int.TryParse(Console.ReadLine(), out senderId))
            {
                Console.WriteLine("Not valid");
            }
            Console.Write("Target ID: ");
            while (!int.TryParse(Console.ReadLine(), out targetId))
            {
                Console.WriteLine("Not valid");
            }
            Console.Write("Max weight: Light - 0, Mid - 1, Heavy - 2");
            while (!Enum.TryParse(Console.ReadLine(), out weight))
            {
                Console.WriteLine("Not valid");
            }
            Console.Write("Priority: Regular - 0, Fast - 1, Emergency - 2");
            while (!Enum.TryParse(Console.ReadLine(), out priority))
            {
                Console.WriteLine("Not valid");
            }


            bl.AddParcel(new(id, senderId, targetId, 0, weight, priority, DateTime.Now, default, default, default));

        }
    }
}
