namespace gestao_financeira.Models
{
    public class Reserva
    {
        public int ReservaId { get; set; }
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        public string TipoReserva { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataReserva { get; set; }
    }
}
