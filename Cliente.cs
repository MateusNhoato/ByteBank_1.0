using System.Globalization;

namespace Byte_Bank_1_0
{
    internal class Cliente
    {
        // informações do cliente
        private string _titular;
        private string _cpf;
        private string _senha;
        protected internal decimal Saldo { get; internal set; }

        // nomes são strings de 2 a 60 caracteres sem números
        public string Titular
        {
            get { return _titular; }
            set
            {
                value = value.Trim();
                string[] array = value.Split(' ');
                foreach (string s in array)
                {
                    foreach (char c in s)
                    {
                        if (!char.IsLetter(c))
                        {
                            Console.WriteLine("\n  Não é possível cadastrar numeros ou caracteres especiais no nome.");
                            return;
                        }
                    }
                }
                if (value != null && value.Length > 1 && value.Length <= 60)
                {
                    TextInfo textInfo = new CultureInfo("pt-br", false).TextInfo;
                    value = textInfo.ToTitleCase(value);
                    _titular = value;
                }
                else
                    Console.WriteLine("\n  Nome inválido, digite o nome corretamente.");

            }
        }
        // cpf consiste em 11 numeros exatamente, não aceitando os pontos nem / ou -
        public string Cpf
        {
            get { return _cpf; }
            set
            {
                foreach (char c in value.Trim())
                {
                    if (!char.IsDigit(c))
                    {
                        Console.WriteLine("\n  Cpf inválido, digite somente os números.");
                        return;
                    }
                }
                if (value.Trim().Length == 11)
                    _cpf = value;
                else
                    Console.WriteLine("\n  Quantidade de dígitos invalida, digite os 11 números do cpf.");

            }
        }
        // pin do cliente tem que ser 6 números/letras/caracteres exatamente
        public string Senha
        {
            get { return _senha; }
            set
            {
                if (value.Contains(' '))
                {
                    Console.WriteLine("\n  Pin com espaços em branco.");
                    return;
                }
                if (value.Contains(';'))
                {
                    Console.WriteLine("\n  ';' é um caractere inválido.");
                    return;
                }
                if (value.Trim().Length == 6)
                {
                    _senha = value;
                }
                else
                    Console.WriteLine("\n  Pin inválido, digite 6 digitos/letras/caracteres.");
            }
        }

        // função para ver se todos os campos foram preenchidos
        internal bool CheckagemCliente(Cliente novocliente)
        {
            Console.WriteLine(Arte.LINHA);
            if (_cpf != null && Titular != null && _senha != null)
            {
                foreach (Cliente cliente in Sistema.clientes)
                {
                    if (cliente.Cpf == _cpf)
                    {
                        Console.WriteLine("  Cpf já cadastrado.");
                        Menu.AperteEnterContinuar();
                        return false;
                    }
                }
                Sistema.clientes.Add(novocliente);
                Console.WriteLine("  Usuário cadastrado com sucesso!");
                Console.Beep();
                Menu.AperteEnterContinuar();
                return true;
            }
            else
            {
                Console.WriteLine("  Dados incorretos, cliente não foi cadastrado.");
                Menu.AperteEnterContinuar();
                return false;
            }
        }

        public override string ToString()
        {
            return $"  Titular: {_titular} | Cpf: {_cpf} | Saldo: {Saldo:C2}";
        }
    }
}
