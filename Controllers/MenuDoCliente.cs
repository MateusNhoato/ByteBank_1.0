using Byte_Bank_1_0.Models;
using Byte_Bank_1_0.Services;
using Entities;
using Views;

namespace Byte_Bank_1_0.Controllers
{
    internal class MenuDoCliente
    {
        private Cliente _cliente;
        private Sistema _sistema;
        public MenuDoCliente(Cliente cliente, Sistema sistema)
        {
            _cliente = cliente;
            _sistema = sistema;
            MostrarMenu();
        }

        private void MostrarMenu()
        {
            string opcao;
            do
            {
                Console.Clear();
                Console.WriteLine(Arte.ATM);
                Console.WriteLine($"\n  Saldo Atual: {_cliente.Saldo:C2}");
                Console.WriteLine(Arte.LINHA);
                Console.WriteLine("  1 - Depositar");
                Console.WriteLine("  2 - Sacar");
                Console.WriteLine("  3 - Transferência");
                Console.WriteLine("  4 - Comprar Moedas Estrangeiras");
                Console.WriteLine("  5 - Histórico de Transações");
                Console.WriteLine("  0 - Voltar para o menu");
                Console.Write("  Digite a opção desejada: ");
                try
                {

                }
                catch (SistemaException e)
                {
                    Console.WriteLine(e.Message);
                    Thread.Sleep(1000);
                    Console.Clear();
                }

                opcao = Console.ReadLine();
                switch (opcao)
                {
                    case "0":
                        Console.Clear();
                        break;
                    case "1":
                        DepositoMenu();
                        break;
                    case "2":
                        SaqueMenu();
                        break;
                    case "3":
                        TransferenciaMenu();
                        break;
                    case "4":
                        _sistema.ComprarMoedasEstrangeiras(_cliente);
                        break;
                    case "5":
                        HistoricoTransacoes();
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("  Entrada inválida");
                        Utilidade.AperteEnterContinuar();
                        break;
                }
            } while (opcao != "0");
        }

        private void DepositoMenu()
        {
            decimal valor;
            Console.Clear();
            Console.WriteLine(Arte.ATM);
            Console.Write("\n  Quantia a ser depositada: ");

            valor = Sistema.CheckarDinheiro(Console.ReadLine());

            Console.WriteLine(Arte.LINHA);
            _sistema.Deposito(_cliente, valor);
            Utilidade.AperteEnterContinuar();
        }
        // função para sacar, mudando o menu e ligando com o sistema
        private void SaqueMenu()
        {
            decimal valor;
            Console.Clear();
            Console.WriteLine(Arte.ATM);

            Console.Write("  Quantia a ser sacada: ");
            valor = Sistema.CheckarDinheiro(Console.ReadLine());

            Console.WriteLine(Arte.LINHA);
            _sistema.Saque(_cliente, valor);
            Utilidade.AperteEnterContinuar();
        }
        // função para transferência, mudando o menu e ligando com o sistema
        private void TransferenciaMenu()
        {
            decimal valor;
            Console.Clear();
            Console.WriteLine(Arte.ATM);

            Console.Write("\n  Quantia a ser transferida: ");
            valor = Sistema.CheckarDinheiro(Console.ReadLine());

            Console.Write("  Cpf do usuário que receberá a quantia: ");
            string cpf = Console.ReadLine();
            Cliente destinatario = _sistema.EncontrarCliente(cpf);

            Console.WriteLine(Arte.LINHA);
            if (destinatario != null)
            {
                _sistema.Transferencia(_cliente, destinatario, valor);
                Utilidade.AperteEnterContinuar();
            }
            else
                throw new SistemaException("  Usuário não encontrado.");
        }

        private void HistoricoTransacoes()
        {
            Console.Clear();
            Console.WriteLine("\n" + Arte.INFO);
            Console.WriteLine(Arte.LINHA);
            ConsoleColor backgroundColor = Console.BackgroundColor;
            ConsoleColor foregroundColor = Console.ForegroundColor;
            Console.BackgroundColor = ConsoleColor.White;
            foreach (Operacao op in _cliente.HistoricoOperacoes)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("  --------------------------------");
                Console.WriteLine($"  {op.Tipo}");
                Console.WriteLine($"  {op.Data}");
                Console.Write($"  R$ ");

                if (op.Quantia > 0)
                    Console.ForegroundColor = ConsoleColor.Green;
                else
                    Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine(op.Quantia);

                Console.ForegroundColor = ConsoleColor.Black;
                if (op.Descricao != null)
                    Console.WriteLine("  " + op.Descricao);
                Console.WriteLine("  --------------------------------\n");
            }
            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;
            Console.WriteLine(Arte.LINHA);
            Utilidade.AperteEnterContinuar();
        }

    }
}
