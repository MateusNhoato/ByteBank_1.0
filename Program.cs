using Byte_Bank_1_0.Services;
using Controllers;

namespace Byte_Bank_1_0;

class Program
{
    static void Main(string[] args)
    {
        Console.Title = "Byte Bank 1.0";

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.BackgroundColor = ConsoleColor.DarkMagenta;
        Console.Clear();

        Menu menu = new Menu(new Sistema());
        menu.ShowMenu();


    }
}