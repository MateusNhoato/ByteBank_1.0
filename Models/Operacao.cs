namespace Byte_Bank_1_0.Models
{
    public class Operacao
    {
        public string Tipo { get; set; }
        public string? Descricao { get; set; }
        public decimal Quantia { get; set; }
        public DateTime Data { get; set; }

        public Operacao(string tipo, decimal quantia, string? descricao = null)
        {
            Tipo = tipo;
            Descricao = descricao;
            Quantia = quantia;
            Data = DateTime.Now;
        }
    }
}
