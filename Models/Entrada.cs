namespace gestao_financeira.Models
{
    public class Entrada
    {
        public int EntradaId { get; set; }
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataEntrada { get; set; }
    }
}
