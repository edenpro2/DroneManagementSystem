using System;
using DO;
using BL;

namespace ConsoleUI_BL
{
    public static class UpdateMethods
    {
        public static void UpdateDrone(Bl bl)
        {
            int id;

            Console.WriteLine("Enter:");
            Console.Write("Drone ID: ");
            while (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Not valid");
            }

            Drone drone = bl.SearchForDroneById(id);

            Console.Write("Drone model: ");
            drone.model = Console.ReadLine();

            bl.UpdateDrone(drone);

        }

        public static void UpdateStation(Bl bl)
        {
            int id, name, chargeSlots;

            Console.WriteLine("Enter:");
            Console.Write("Station ID: ");
            while (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Not valid");
            }

            Station station = bl.SearchForStationById(id);

            Console.Write("Station name: ");
            string temp = Console.ReadLine();
            if (!string.IsNullOrEmpty(temp))
            {
                while (!int.TryParse(temp, out name))
                {
                    Console.WriteLine("Not valid");
                    temp = Console.ReadLine();
                }
                station.name = name;
            }

            Console.Write("Total charging ports: ");
            temp = Console.ReadLine();
            if (!string.IsNullOrEmpty(temp))
            {
                while (!int.TryParse(temp, out chargeSlots))
                {
                    Console.WriteLine("Not valid");
                    temp = Console.ReadLine();
                }
                station.ports.Capacity = chargeSlots;
            }

            bl.UpdateStation(station);
        }

        public static void UpdateCustomer(Bl bl)
        {
            int id;
            string name, phone;

            Console.WriteLine("Enter:");
            Console.Write("Customer ID: ");
            while (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Not valid");
            }

            Customer customer = bl.SearchForCustomerById(id);

            Console.Write("Name: ");
            name = Console.ReadLine();
            if (!string.IsNullOrEmpty(name))
            {
                customer.name = name;
            }

            Console.Write("Phone: ");
            phone = Console.ReadLine();
            if (!string.IsNullOrEmpty(phone))
            {
                customer.phone = phone;
            }

            bl.UpdateCustomer(customer);
        }

        public static void ChargeDrone(Bl bl)
        {
            int id;

            Console.WriteLine("Enter:");
            Console.Write("Drone ID: ");
            while (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Not valid");
            }

            bl.DroneCharge(id);
            
        }

        public static void ReleaseDrone(Bl bl)
        {
            int id, hours;

            Console.WriteLine("Enter:");
            Console.Write("Drone ID: ");
            while (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Not valid");
            }
            Console.Write("Charge time: ");
            while (!int.TryParse(Console.ReadLine(), out hours))
            {
                Console.WriteLine("Not valid");
            }

            bl.DroneRelease(id, hours);

        }

        public static void AssignDronetoParcel(Bl bl)
        {
            int id;
            Console.WriteLine("Enter:");
            Console.Write("Drone ID: ");
            while (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Not valid");
            }

            bl.AssignDronetoParcel(id);
        }

        public static void CollectParcelbyDrone(Bl bl)
        {
            int id;
            Console.WriteLine("Enter:");
            Console.Write("Drone ID: ");
            while (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Not valid");
            }

            bl.CollectParcelbyDrone(id);
        }

        public static void DeliverByDrone(Bl bl)
        {
            int id;
            Console.WriteLine("Enter:");
            Console.Write("Drone ID: ");
            while (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Not valid");
            }

            bl.DeliverByDrone(id);
        }
    }

}