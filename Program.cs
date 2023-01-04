using Controllers;
using Entities;

namespace Byte_Bank_1_0
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Byte Bank 1.0";

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.Clear();
            Sistema.CriarBancoDeDados();
            Menu.ShowMenu();
        }
    }
}