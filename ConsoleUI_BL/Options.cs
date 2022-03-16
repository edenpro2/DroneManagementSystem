using System;

namespace ConsoleUI_BL
{
    public static class Options
    {
        public static void Menu()
        {
            Console.Write(
                "Main Menu\n" +
                "Choose one of the options below to proceed: \n" +
                "Add:\t\t1 \n" +
                "Update:\t\t2 \n" +
                "Display:\t\t3 \n" +
                "List Display:\t\t4 \n" +
                "Exit:\t\t5 \n"
            );
        }

        public static void AddMenu()
        {
            Console.Write(
                "Choose item to add: \n" +
                "Station:\t\t 1 \n" +
                "Drone:\t\t 2 \n" +
                "Customer:\t\t 3 \n" +
                "Parcel:\t\t 4 \n" +
                "Return to main menu:\t\tEnter\n"
            );
        }

        public static void UpdateMenu()
        {
            Console.Write(
                "Choose item to update: \n" +
                "Update drone:\t\t1 \n" +
                "Update station:\t\t2 \n" +
                "Update customer:\t\t3 \n" +
                "Send a drone to charge:\t\t4 \n" +
                "Release drone from charge:\t\t5 \n" +
                "Assign drone to parcel:\t\t6 \n" +
                "Collect parcel by drone:\t\t7 \n" +
                "Deliver parcel by drone:\t\t8 \n" +
                "Return to main menu:\t\tEnter\n"
            );
        }

        public static void DisplayMenu()
        {
            Console.Write(
                "Choose item to display: \n" +
                "Station:\t\t1 \n" +
                "Drone:\t\t2 \n" +
                "Customer:\t\t3 \n" +
                "Parcel:\t\t4 \n" +
                "Return to main menu:\t\tEnter\n"
            );
        }

        public static void DisplayListMenu()
        {
            Console.Write(
                "Choose list of items to display: \n" +
                "Stations:\t\t1 \n" +
                "Drones:\t\t2 \n" +
                "Customers:\t\t3 \n" +
                "Parcels:\t\t4 \n" +
                "Return to main menu:\t\tEnter\n"
            );
        }
    }
}
