using System;
using BO;
using BL;

namespace ConsoleUI_BL
{
    public static partial class ConsoleUI_BL
    {
        public static partial void Add(int option, Bl bl)
        {
            switch (option)
            {
                case (int)AddChoices.Station:
                    AddingMethods.AddStation(bl);
                    break;
                case (int)AddChoices.Drone:
                    AddingMethods.AddDrone(bl);
                    break;
                case (int)AddChoices.Customer:
                    AddingMethods.AddCustomer(bl);
                    break;
                case (int)AddChoices.Parcel:
                    AddingMethods.AddParcel(bl);
                    break;
                default:
                    Console.WriteLine("Not an option");
                    break;
            }
        }

        public static partial void Update(int option, Bl bl)
        {
            switch(option)
            {
                case (int)UpdateChoices.Station:
                    UpdateMethods.UpdateStation(bl);
                    break;
                case (int)UpdateChoices.Drone:
                    UpdateMethods.UpdateDrone(bl);
                    break;
                case (int)UpdateChoices.Customer:
                    UpdateMethods.UpdateCustomer(bl);
                    break;
                case (int)UpdateChoices.SendToCharge:
                    UpdateMethods.ChargeDrone(bl);
                    break;
                case (int)UpdateChoices.ReleaseFromCharge:
                    UpdateMethods.ReleaseDrone(bl);
                    break;
                case (int)UpdateChoices.AssignDrone:
                    UpdateMethods.AssignDronetoParcel(bl);
                    break;
                case (int)UpdateChoices.CollectParcel:
                    UpdateMethods.CollectParcelbyDrone(bl);
                    break;
                case (int)UpdateChoices.DeliverByDrone:
                    UpdateMethods.DeliverByDrone(bl);
                    break;
                default:
                    Console.WriteLine("Not an option");
                    break;

            }
        }

        public static partial void Display(int option, Bl bl)
        {
            switch (option)
            {
                case (int)DisplayChoices.Station:
                    DisplayMethods.DisplayStation(bl);
                    break;
                case (int)DisplayChoices.Drone:
                    DisplayMethods.DisplayDrone(bl);
                    break;
                case (int)DisplayChoices.Customer:
                    DisplayMethods.DisplayCustomer(bl);
                    break;
                case (int)DisplayChoices.Parcel:
                    DisplayMethods.DisplayParcel(bl);
                    break;
                default:
                    Console.WriteLine("Not an option");
                    break;
            }
        }

        public static partial void ListDisplay(int option, Bl bl)
        {
            switch (option)
            {
                case (int)DisplayChoices.Station:
                    ListDisplayMethods.DisplayStationList(bl);
                    break;
                case (int)DisplayChoices.Drone:
                    ListDisplayMethods.DisplayDroneList(bl);
                    break;
                case (int)DisplayChoices.Customer:
                    ListDisplayMethods.DisplayCustomerList(bl);
                    break;
                case (int)DisplayChoices.Parcel:
                    ListDisplayMethods.DisplayParcelList(bl);
                    break;
                default:
                    Console.WriteLine("Not an option");
                    break;
            }
        }
    }
}
