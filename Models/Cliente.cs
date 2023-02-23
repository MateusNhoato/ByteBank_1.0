using Byte_Bank_1_0.Models;
using Newtonsoft.Json;
using System.Globalization;

namespace Entities
{
    public class Cliente
    {
        private string _titular;
        private string _cpf;
        private string _senha;
        public decimal Saldo { get; private set; }
        public List<Operacao> HistoricoOperacoes { get; private set; }

        public Cliente()
        {
            HistoricoOperacoes = new List<Operacao>();
        }

        [JsonConstructor]
        public Cliente(string titular, string cpf, string senha, decimal saldo, List<Operacao> historicoOperacoes)
        {
            _titular = titular;
            _cpf = cpf;
            _senha = senha;
            Saldo = saldo;
            HistoricoOperacoes = historicoOperacoes;
        }

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
                            throw new ClienteException("\n  Não é possível cadastrar numeros ou caracteres especiais no nome.");
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
                    throw new ClienteException("\n  Nome inválido, digite o nome corretamente.");

            }
        }
        public string Cpf
        {
            get { return _cpf; }
            set
            {
                foreach (char c in value.Trim())
                {
                    if (!char.IsDigit(c))
                    {
                        throw new ClienteException("\n  Cpf inválido, digite somente os números.");
                    }
                }
                if (value.Trim().Length == 11)
                    _cpf = value;
                else
                    throw new ClienteException("\n  Quantidade de dígitos invalida, digite os 11 números do cpf.");

            }
        }

        public string Senha
        {
            get { return _senha; }
            set
            {
                if (value.Contains(' '))
                {
                    throw new ClienteException("\n  Pin com espaços em branco.");
                }
                if (value.Trim().Length == 6)
                {
                    _senha = value;
                }
                else
                    throw new ClienteException("\n  Pin inválido, digite 6 digitos/letras/caracteres.");
            }
        }

        public void MudarBalanco(decimal quantia)
        {
            Saldo += quantia;
        }

        public void AdicionarOperacao(Operacao operacao)
        {
            HistoricoOperacoes.Add(operacao);
        }
        public override string ToString()
        {
            return $"  Titular: {_titular} | Cpf: {_cpf} | Saldo: {Saldo:C2}";
        }
    }
}
