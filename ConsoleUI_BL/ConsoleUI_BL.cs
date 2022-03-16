using System;
using BL;
using static BO.Choices;

namespace ConsoleUI_BL
{
    public static partial class ConsoleUI_BL
    {
        public static bool repeat = true;
        public static int option = 0;

        public static partial void Add(int option, Bl bl);
        public static partial void Update(int option, Bl bl);
        public static partial void Display(int option, Bl bl);
        public static partial void ListDisplay(int option, Bl bl);

        static int Main()
        {
            Bl database = (Bl)BlFactory.GetBl();

            do
            {
                Options.Menu();

                if (int.TryParse(Console.ReadLine(), out option))
                {
                    try
                    {
                        switch (option)
                        {
                            case (int)Adding:
                                Options.AddMenu();
                                option = int.Parse(Console.ReadLine());
                                Add(option, database);
                                break;
                            case (int)Updating:
                                Options.UpdateMenu();
                                option = int.Parse(Console.ReadLine());
                                Update(option, database);
                                break;
                            case (int)Displaying:
                                Options.DisplayMenu();
                                option = int.Parse(Console.ReadLine());
                                Display(option, database);
                                break;
                            case (int)ListDisplaying:
                                Options.DisplayListMenu();
                                option = int.Parse(Console.ReadLine());
                                ListDisplay(option, database);
                                break;
                            case (int)Exit:
                                repeat = false;
                                break;
                            default:
                                Console.WriteLine("Not an option");
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            } while (repeat);


            return 0;
        }
    }
}
