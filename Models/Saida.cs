namespace gestao_financeira.Models
{
    public class Saida
    {
        public int SaidaId { get; set; }
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataSaida { get; set; }
    }
}
