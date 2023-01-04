using Views;
using Entities;

namespace Controllers
{
    static class Menu
    {
        public static void ShowMenu()
        {
            // opcao escolhida pelo usuário
            string opcao;
            Console.WriteLine();
            do
            {
                Console.WriteLine(Arte.LOGO);

                Console.WriteLine("\n\n  1 - Inserir novo usuário");
                Console.WriteLine("  2 - Deletar um usuário");
                Console.WriteLine("  3 - Listar todas as contas registradas");
                Console.WriteLine("  4 - Detalhes de um usuário");
                Console.WriteLine("  5 - Quantia armazenada no banco");
                Console.WriteLine("  6 - Manipular a conta");
                Console.WriteLine("  0 - Para sair do programa\n");
                Console.Write("  Digite a opção desejada: ");
                opcao = Console.ReadLine();

                Console.Clear();
                switch (opcao)
                {
                    case "0":
                        Console.Beep();
                        EncerrarPrograma();
                        break;
                    case "1":
                        Console.Beep();
                        Sistema.AdicionarUsuario();
                        break;
                    case "2":
                        Console.Beep();
                        Sistema.DeletarUsuario();
                        break;
                    case "3":
                        Console.Beep();
                        Sistema.ListarUsuarios();
                        break;
                    case "4":
                        Console.Beep();
                        Sistema.ManipulacaoConta(true);
                        break;
                    case "5":
                        Console.Beep();
                        Sistema.QuantiaNoBanco();
                        break;
                    case "6":
                        Console.Beep();
                        Sistema.ManipulacaoConta(false);
                        break;
                    default:
                        EntradaInvalida();
                        break;

                }
            } while (true);
        }

        // menu de deposito/saque/transferência
        internal static void MenuDoCliente(Cliente cliente)
        {
            string opcao;
            do
            {
                Console.WriteLine(Arte.ATM);
                Console.WriteLine($"\n  Saldo Atual: {cliente.Saldo:C2}");
                Console.WriteLine(Arte.LINHA);
                Console.WriteLine("  1 - Depositar");
                Console.WriteLine("  2 - Sacar");
                Console.WriteLine("  3 - Transferência");
                Console.WriteLine("  0 - Voltar para o menu");
                Console.Write("  Digite a opção desejada: ");
                opcao = Console.ReadLine();
                switch (opcao)
                {
                    case "0":
                        Console.Beep();
                        Console.Clear();
                        ShowMenu();
                        break;

                    case "1":
                        Console.Beep();
                        DepositoMenu(cliente);
                        break;
                    case "2":
                        Console.Beep();
                        SaqueMenu(cliente);
                        break;
                    case "3":
                        Console.Beep();
                        TransferenciaMenu(cliente);
                        break;
                    default:
                        Console.Beep();
                        Console.Clear();
                        EntradaInvalida();
                        break;
                }
            } while (true);
        }
        // função para depositar, mudando o menu e ligando com o sistema
        private static void DepositoMenu(Cliente cliente)
        {
            decimal valor;
            Console.Clear();
            Console.WriteLine(Arte.ATM);
            Console.Write("\n  Quantia a ser depositada: ");

            valor = Sistema.CheckarDinheiro();

            Console.WriteLine(Arte.LINHA);
            Sistema.Deposito(cliente, valor);
            AperteEnterContinuar();
        }
        // função para sacar, mudando o menu e ligando com o sistema
        private static void SaqueMenu(Cliente cliente)
        {
            decimal valor;
            Console.Clear();
            Console.WriteLine(Arte.ATM);

            Console.Write("  Quantia a ser sacada: ");
            valor = Sistema.CheckarDinheiro();

            Console.WriteLine(Arte.LINHA);
            Sistema.Saque(cliente, valor);
            AperteEnterContinuar();
        }
        // função para transferência, mudando o menu e ligando com o sistema
        private static void TransferenciaMenu(Cliente cliente)
        {
            decimal valor;
            Console.Clear();
            Console.WriteLine(Arte.ATM);

            Console.Write("\n  Quantia a ser transferida: ");
            valor = Sistema.CheckarDinheiro();

            Console.Write("  Cpf do usuário que receberá a quantia: ");
            string cpf = Console.ReadLine();
            Cliente destinatario = Sistema.clientes.Find(x => x.Cpf == cpf);

            Console.WriteLine(Arte.LINHA);
            if (destinatario != null)
                Sistema.Transferencia(cliente, destinatario, valor);

            else
                Console.WriteLine("  Usuário não encontrado.");
            AperteEnterContinuar();
        }

        // funções utilitárias para falar com o usuário
        internal static void EntradaInvalida()
        {
            Console.WriteLine("  Entrada inválida");
            Thread.Sleep(1000);
            Console.Clear();
        }

        internal static void AperteEnterContinuar()
        {
            Console.WriteLine();
            Console.Write("  Aperte enter para continuar.");
            Console.ReadLine();
            Console.Clear();
        }
        // função de encerramento do programa
        internal static void EncerrarPrograma()
        {
            Console.WriteLine(Arte.OBRIGADO);
            Thread.Sleep(1000);
            Console.WriteLine(Arte.FEITOPOR);
            Thread.Sleep(1000);
            Console.WriteLine(Arte.NOME);
            Environment.Exit(0);
        }
    }
}


