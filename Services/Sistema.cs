using Byte_Bank_1_0.Controllers;
using Byte_Bank_1_0.Models;
using Byte_Bank_1_0.Repositories;
using Entities;
using Newtonsoft.Json.Linq;
using System.Globalization;
using Views;

namespace Byte_Bank_1_0.Services
{
    public class Sistema
    {
        private List<Cliente>? _clientes;
        private readonly Clientes _repositorio;
        public List<Moedas> Conversoes;

        public Sistema()
        {
            _repositorio = new Clientes();
            _clientes = _repositorio.PuxarBancoDeDados() ?? new List<Cliente>();
            Conversoes = GetMoedasAsync().Result;
        }

        public Cliente? EncontrarCliente(string cpf)
        {
            return _clientes.Find(c => c.Cpf == cpf);
        }

        public void Saque(Cliente cliente, decimal quantia, bool transferencia = false)
        {
            if (quantia > 0)
            {
                if (cliente.Saldo < quantia)
                    throw new ClienteException("  Saldo insuficiente");

                cliente.MudarBalanco(quantia * -1);
                if (!transferencia)
                {
                    cliente.AdicionarOperacao(new Operacao("Saque", quantia * -1));
                    Console.WriteLine($"  Saque de {quantia:C2} efetuado com sucesso!");
                    _repositorio.AtualizarListaDeClientes(_clientes);
                }
            }
            else
            {
                throw new SistemaException("\n  Quantia ou entrada inválida.");
            }
        }

        public void Deposito(Cliente cliente, decimal quantia, bool transferencia = false)
        {
            if (quantia > 0)
            {

                cliente.MudarBalanco(quantia);
                if (!transferencia)
                {
                    cliente.AdicionarOperacao(new Operacao("Deposito", quantia));
                    Console.WriteLine($"  {quantia:C2} depositado(s) com sucesso!");
                    _repositorio.AtualizarListaDeClientes(_clientes);
                }
            }
            else
                throw new SistemaException("\n  Quantia ou entrada inválida.");
        }

        public void Transferencia(Cliente depositando, Cliente depositado, decimal quantia)
        {

            if (depositando != depositado)
            {
                Saque(depositando, quantia, true);
                Deposito(depositado, quantia, true);


                depositando.AdicionarOperacao(new Operacao("Transferência", quantia * -1));
                depositado.AdicionarOperacao(new Operacao("Transferência", quantia));
                _repositorio.AtualizarListaDeClientes(_clientes);

                Console.WriteLine($"\n  Transferência de {quantia:C2} para {depositado.Titular} efetuada com sucesso!");
            }
            else
            {
                throw new SistemaException("  Não é possível transferir para si mesmo."
                                            + "\n  Transferência mal sucedida.");
            }
        }

        public static decimal CheckarDinheiro(string input)
        {

            if (input.Contains(','))
                input = input.Replace(',', '.');

            if (decimal.TryParse(input, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal quantia))
                return quantia;
            else
                throw new SistemaException("  Quantia inválida");
        }

        // função para adicionar novos clientes no Sistema e no arquivo txt
        public void AdicionarUsuario()
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

            try
            {
                if (cpf != null && titular != null && senha != null)
                {
                    foreach (Cliente cliente in _clientes)
                    {
                        if (cliente.Cpf == cpf)
                            throw new SistemaException("Cpf já cadastrado.");
                    }
                }
                else
                    throw new SistemaException("  Dados incorretos, cliente não foi cadastrado.");
            }
            catch (SistemaException e)
            {
                Console.WriteLine(e.Message);
                Utilidade.AperteEnterContinuar();
                Console.Clear();
                return;
            }

            _clientes.Add(novoCliente);
            _repositorio.AtualizarListaDeClientes(_clientes);
            Console.WriteLine("  Usuário cadastrado com sucesso!");
            Utilidade.AperteEnterContinuar();
        }

        public void DeletarUsuario()
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
                    throw new ClienteException("  Cpf inválido, digite somente os números.");
            }
            if (cpf.Trim().Length == 11)
            {
                cliente = _clientes.Find(x => x.Cpf == cpf);

                if (cliente != null)
                {
                    _clientes.Remove(cliente);
                    Console.WriteLine("  Cliente removido.");
                    _repositorio.AtualizarListaDeClientes(_clientes);
                }
                else
                    throw new SistemaException("  Cliente não encontrado.");
            }
            else
                throw new ClienteException("  Quantidade de dígitos invalida, digite os 11 números do cpf.");

            Thread.Sleep(2000);
            Console.Clear();
        }
        // função para listar todos os usuários do banco
        internal void ListarUsuarios()
        {
            Console.WriteLine(Arte.LISTA);
            if (_clientes.Count > 0)
            {
                for (int i = 0; i < _clientes.Count; i++)
                {
                    Console.WriteLine();
                    Console.Write($"  Cliente {i + 1}:");
                    Console.WriteLine(_clientes[i]);
                }
            }
            else
                Console.WriteLine("\n  Nenhum usuário cadastrado.");
            Utilidade.AperteEnterContinuar();

        }
        public void VerConta()
        {
            Console.WriteLine(Arte.INFO);
            Console.Write("  Digite o Cpf do titular: ");
            string cpf = Console.ReadLine();
            Cliente? cliente = EncontrarCliente(cpf);
            Console.WriteLine(Arte.LINHA);

            if (cliente == null)
                throw new SistemaException("  Cliente não encontrado.");

            Console.WriteLine(cliente);
            Utilidade.AperteEnterContinuar();
            Console.Clear();
        }
        public void ManipulacaoConta()
        {
            Console.WriteLine(Arte.INFO);
            Console.Write("  Digite o Cpf do titular: ");
            string cpf = Console.ReadLine();
            Console.WriteLine(Arte.LINHA);

            Cliente? cliente = EncontrarCliente(cpf);

            if (cliente == null)
                throw new SistemaException("  Cliente não encontrado.");

            ValidacaoUsuario(cliente);
            MenuDoCliente menuDoCliente = new MenuDoCliente(cliente, this);
        }


        public void ValidacaoUsuario(Cliente cliente)
        {
            Console.WriteLine(Arte.ENTRAR);
            Console.Write("  Digite seu PIN (6 digitos/letras): ");
            string? senha = Console.ReadLine();


            if (cliente.Senha != senha)
                throw new ClienteException("  Senha inválida.");

            Console.WriteLine("  Acesso permitido.");
            Thread.Sleep(1000);
            Console.Clear();
        }

        // função para ver o total de dinheiro no banco
        internal void QuantiaNoBanco()
        {
            decimal total = 0;
            foreach (Cliente cliente in _clientes)
            {
                total += cliente.Saldo;
            }
            Console.WriteLine(Arte.DINHEIRO);
            Console.WriteLine(Arte.LINHA);
            Console.WriteLine($"  {total:C2}");
            Utilidade.AperteEnterContinuar();

        }


        private async ValueTask<List<Moedas>> GetMoedasAsync()
        {
            List<Moedas> moedas = new List<Moedas>();

            try
            {
                HttpClient httpCliente = new HttpClient();

                string reponseString = await httpCliente.GetStringAsync("https://economia.awesomeapi.com.br/last/USD-BRL,EUR-BRL");

                JObject jsonResponse = JObject.Parse(reponseString);

                IEnumerable<string> nomeMoedas = jsonResponse.Properties().Select(p => p.Name);

                foreach (string nomeMoeda in nomeMoedas)
                {
                    string nome = (string)jsonResponse[nomeMoeda]["code"];
                    string codigo = (string)jsonResponse[nomeMoeda]["codein"];
                    string nomeConversao = (string)jsonResponse[nomeMoeda]["name"];
                    decimal valor = (decimal)jsonResponse[nomeMoeda]["bid"];
                    DateTime data = (DateTime)jsonResponse[nomeMoeda]["create_date"];

                    Moedas novaMoeda = new Moedas()
                    {
                        MoedaConvertidaPara = nome,
                        MoedaConvertidaDe = codigo,
                        NomeConversaoExtenso = nomeConversao,
                        Valor = valor,
                        Data = data
                    };
                    moedas.Add(novaMoeda);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro na consulta da moeda: " + ex.Message);
            }
            return moedas;
        }

        public void ComprarMoedasEstrangeiras(Cliente cliente)
        {
            string opcao = string.Empty;

            do
            {
                Console.Clear();
                Console.WriteLine(Arte.ATM + "\n");

                int contador = 0;
                foreach (var moeda in Conversoes)
                {
                    Console.WriteLine($"  {++contador} - {moeda.MoedaConvertidaPara}");
                }

                Console.WriteLine("\n  0 - Voltar");

                Console.Write("\n  Escolha a opção desejada: ");
                opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "0":
                        break;
                    default:
                        Console.Clear();
                        int n;
                        if (int.TryParse(opcao, out n))
                        {
                            if (n >= 0 && n <= Conversoes.Count)
                            {
                                Moedas moeda = Conversoes[n - 1];

                                string[] dePara = moeda.NomeConversaoExtenso.Split('/');
                                string paraMoeda = dePara[0];
                                string deMoeda = dePara[1];

                                Console.WriteLine(Arte.ATM + "\n");
                                Console.WriteLine($"  Comprar {paraMoeda} com {deMoeda}");
                                Console.WriteLine($"  1 {moeda.MoedaConvertidaPara} = {moeda.Valor:C2} {moeda.MoedaConvertidaDe}");
                                Console.Write("\n  Digite a quantia que gostaria de comprar: ");

                                decimal valor = Sistema.CheckarDinheiro(Console.ReadLine());
                                decimal valorTotal = valor * moeda.Valor;

                                Saque(cliente, valorTotal);

                                Console.Clear();
                                Console.WriteLine(Arte.ATM + "\n");
                                Console.WriteLine(Arte.LINHA);
                                Console.WriteLine($"  {valor:C2} ({moeda.MoedaConvertidaPara}) comprados por {valorTotal:C2} ({moeda.MoedaConvertidaDe})");

                                cliente.AdicionarOperacao(new Operacao("Compra de Moeda Estrangeira", valorTotal * -1, $"Compra de {valor} {moeda.MoedaConvertidaPara}"));
                                _repositorio.AtualizarListaDeClientes(_clientes);
                                Utilidade.AperteEnterContinuar();
                            }
                        }
                        else
                        {
                            Console.WriteLine("  Entrada inválida");
                            Utilidade.AperteEnterContinuar();
                        }
                        break;
                }

            } while (opcao != "0");
        }
    }
}
