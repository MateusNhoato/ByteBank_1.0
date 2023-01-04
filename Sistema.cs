using System.Globalization;

namespace Byte_Bank_1_0
{
    internal class Sistema
    {
        // lista de clientes do banco
        public static List<Cliente>? clientes;

        // função para pegar a lista de usuários do arquivo txt
        internal static void CriarBancoDeDados()
        {
            // lista de clientes do banco
            clientes = new List<Cliente>();

            try
            {
                StreamReader sr = new StreamReader(@"..\..\..\Dados\Dados.txt");
                string line = sr.ReadLine();
                while (line != null)
                {
                    string[] info = line.Split(";");
                    Cliente novoCliente = new Cliente();
                    novoCliente.Titular = info[0];
                    novoCliente.Cpf = info[1];
                    novoCliente.Senha = info[2];
                    novoCliente.Saldo = decimal.Parse(info[3]);
                    clientes.Add(novoCliente);
                    line = sr.ReadLine();
                }
                sr.Close();
            }
            catch (FileNotFoundException e)
            {

                using FileStream fs = File.Create(@"..\..\..\Dados\Dados.txt");
            }

        }

        // função para atualizar a lista de usuarios no arquivo txt
        internal static void AtualizarListaDeUsuarios()
        {
            File.WriteAllText(@"..\..\..\Dados\Dados.txt", String.Empty);
            StreamWriter sw = new StreamWriter(@"..\..\..\Dados\Dados.txt");

            foreach (Cliente cliente in clientes)
            {
                sw.WriteLine($"{cliente.Titular};{cliente.Cpf};{cliente.Senha};{cliente.Saldo}");
            }
            sw.Close();
        }


        // função de saque
        public static bool Saque(Cliente cliente, decimal quantia)
        {
            if (quantia > 0)
            {
                if (cliente.Saldo >= quantia)
                {
                    cliente.Saldo -= quantia;
                    Console.WriteLine($"  Saque de {quantia:C2} efetuado com sucesso!");
                    Console.Beep();
                    AtualizarListaDeUsuarios();
                    return true;
                }
                Console.WriteLine("  Saldo insuficiente.");
                return false;
            }
            else
            {
                Console.WriteLine($"\n  Quantia ou entrada inválida.");
                return false;
            }
        }

        // função de depósito
        public static void Deposito(Cliente cliente, decimal quantia)
        {
            if (quantia > 0)
            {
                cliente.Saldo += quantia;
                Console.WriteLine($"  {quantia:C2} depositado(s) com sucesso!");
                Console.Beep();
                AtualizarListaDeUsuarios();
            }
            else
                Console.WriteLine($"\n  Quantia ou entrada inválida.");


        }

        // função de transferencia, reaproveitando a de saque e deposito
        public static void Transferencia(Cliente depositando, Cliente depositado, decimal quantia)
        {

            if (depositando != depositado)
            {
                if (Saque(depositando, quantia) && quantia != 0)
                {
                    Deposito(depositado, quantia);
                    Console.Clear();
                    Console.WriteLine(Arte.ATM);
                    Console.WriteLine(Arte.LINHA);
                    Console.WriteLine($"\n  Transferência de {quantia:C2} para {depositado.Cpf} efetuada com sucesso!");
                    Console.Beep();
                    return;
                }
            }
            else
                Console.WriteLine("\n  Não é possível transferir para si mesmo.");
            Console.WriteLine("\n  Transferência mal sucedida.");


        }

        // função para validar a entrada do usuário (dinheiro)
        public static decimal CheckarDinheiro()
        {
            var input = Console.ReadLine();

            if (input.Contains(','))
                input = input.Replace(',', '.');

            if (decimal.TryParse(input, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal quantia))
                return quantia;
            else
                return 0;
        }

        // função para adicionar novos clientes no Sistema e no arquivo txt
        internal static void AdicionarUsuario()
        {
            Console.WriteLine(Arte.CADASTRO);
            Console.Write("  Titular da conta (somente letras): ");
            string titular = Console.ReadLine();
            Console.Write("  Cpf do titular (somente os 11 números): ");
            string cpf = Console.ReadLine();
            Console.Write("  Escreva um PIN de acesso (6 dígitos/letras/caracteres): ");
            string senha = Console.ReadLine();

            Cliente novoCliente = new Cliente();
            novoCliente.Titular = titular;
            novoCliente.Cpf = cpf;
            novoCliente.Senha = senha;

            if (novoCliente.CheckagemCliente(novoCliente))
            {
                StreamWriter sw = new StreamWriter(@"..\..\..\Dados\Dados.txt");
                sw.WriteLine($"{novoCliente.Titular};{novoCliente.Cpf};{novoCliente.Senha};{novoCliente.Saldo}");
                sw.Close();
            }

        }

        // função para remover o cliente
        internal static void DeletarUsuario()
        {
            Console.Clear();
            Console.WriteLine(Arte.REMOVER);
            Console.Write("  Digite o cpf do usuário que quer remover: ");
            string cpf = Console.ReadLine();
            Cliente cliente;
            Console.WriteLine(Arte.LINHA);
            foreach (char c in cpf.Trim())
            {
                if (!char.IsDigit(c))
                {
                    Console.WriteLine("  Cpf inválido, digite somente os números.");
                }
            }
            if (cpf.Trim().Length == 11)
            {
                cliente = clientes.Find(x => x.Cpf == cpf);

                if (cliente != null)
                {
                    clientes.Remove(cliente);
                    Console.WriteLine("  Cliente removido.");
                    Console.Beep();
                    AtualizarListaDeUsuarios();
                }

                else
                    Console.WriteLine("  Cliente não encontrado.");
            }
            else
                Console.WriteLine("  Quantidade de dígitos invalida, digite os 11 números do cpf.");

            Thread.Sleep(2000);
            Console.Clear();


        }
        // função para listar todos os usuários do banco
        internal static void ListarUsuarios()
        {
            Console.WriteLine(Arte.LISTA);
            if (clientes.Count > 0)
            {
                for (int i = 0; i < clientes.Count; i++)
                {
                    Console.WriteLine();
                    Console.Write($"  Cliente {i + 1}:");
                    Console.WriteLine(clientes[i]);
                }
            }
            else
                Console.WriteLine("\n  Nenhum usuário cadastrado.");
            Menu.AperteEnterContinuar();

        }
        // esta função tem 2 propósitos:
        // 1- passar os detalhes da conta, se passado true para o parâmetro detalhes
        // 2- para false no parâmetro detalhes, esta função vai chamar a função de validação do usuário no sistema
        internal static void ManipulacaoConta(bool detalhes)
        {
            Console.WriteLine(Arte.INFO);
            Console.Write("  Digite o Cpf do titular: ");
            var info = Console.ReadLine();
            Cliente? cliente;
            Console.WriteLine(Arte.LINHA);
            if (!string.IsNullOrEmpty(info))
            {
                cliente = clientes.Find(x => x.Cpf == info);

                if (cliente != null)
                {
                    if (detalhes)
                    {
                        Console.WriteLine(cliente);
                        Console.Beep();
                        Menu.AperteEnterContinuar();
                        return;
                    }
                    else
                    {
                        if (ValidacaoUsuario(cliente))
                        {
                            Menu.MenuDoCliente(cliente);
                            Console.Beep();
                        }
                        else
                            Menu.ShowMenu();
                    }
                }
                else
                {
                    Console.WriteLine("  Usuário não encontrado.");
                    System.Threading.Thread.Sleep(1500);
                    Console.Clear();
                }

            }
            else
            {
                Console.WriteLine("  Usuário não encontrado.");
                System.Threading.Thread.Sleep(1500);
                Console.Clear();
            }
        }


        // após o usuário já ter confirmado seu nome ou cpf na função ManipulacaoConta,
        // nesta função validamos o usuário ao pedir pelo seu PIN(senha) e checkar com o valor guardado
        public static bool ValidacaoUsuario(Cliente cliente)
        {
            Console.WriteLine(Arte.ENTRAR);
            Console.Write("  Digite seu PIN (6 digitos/letras): ");
            var senha = Console.ReadLine();
            if (cliente.Senha == senha)
            {
                Console.WriteLine("  Acesso permitido.");
                Console.Beep();
                System.Threading.Thread.Sleep(1000);
                Console.Clear();
                return true;
            }
            Console.WriteLine("  Senha inválida.");
            System.Threading.Thread.Sleep(1000);
            Console.Clear();
            return false;
        }

        // função para ver o total de dinheiro no banco
        internal static void QuantiaNoBanco()
        {
            decimal total = 0;
            foreach (Cliente cliente in clientes)
            {
                total += cliente.Saldo;
            }
            Console.WriteLine(Arte.DINHEIRO);
            Console.WriteLine(Arte.LINHA);
            Console.WriteLine($"  {total:C2}");
            Menu.AperteEnterContinuar();

        }
    }
}
