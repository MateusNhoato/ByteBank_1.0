using Views;

namespace Byte_Bank_1_0.Services
{
    static class Utilidade
    {
        internal static void AperteEnterContinuar()
        {
            Console.Write("\n  Aperte enter para continuar.");
            Console.ReadLine();
            Console.Clear();
        }
        internal static void EncerrarPrograma()
        {
            Console.WriteLine(Arte.OBRIGADO);
            Thread.Sleep(1000);
            Console.WriteLine(Arte.FEITOPOR);
            Thread.Sleep(1000);
            Console.WriteLine(Arte.NOME);
        }
    }
}
