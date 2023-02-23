using Byte_Bank_1_0.Models;
using Byte_Bank_1_0.Services;
using Views;

namespace Controllers
{
    public class Menu
    {
        private readonly Sistema _sistema;

        public Menu(Sistema sistema)
        {
            _sistema = sistema;
        }

        public void ShowMenu()
        {
            string opcao = "";
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
                try
                {
                    opcao = Console.ReadLine();
                    Console.Clear();
                    switch (opcao)
                    {
                        case "0":
                            Utilidade.EncerrarPrograma();
                            break;
                        case "1":
                            _sistema.AdicionarUsuario();
                            break;
                        case "2":
                            _sistema.DeletarUsuario();
                            break;
                        case "3":
                            _sistema.ListarUsuarios();
                            break;
                        case "4":
                            _sistema.VerConta();
                            break;
                        case "5":
                            _sistema.QuantiaNoBanco();
                            break;
                        case "6":
                            _sistema.ManipulacaoConta();
                            break;
                        default:
                            Console.WriteLine("  Entrada inválida");
                            Utilidade.AperteEnterContinuar();
                            break;
                    }
                }
                catch (SistemaException e)
                {
                    Console.WriteLine(e.Message);
                    Utilidade.AperteEnterContinuar();
                }
                catch (ClienteException e)
                {
                    Console.WriteLine(e.Message);
                    Utilidade.AperteEnterContinuar();
                }

            } while (opcao != "0");
        }

    }
}


