namespace gestao_financeira.Models
{
    namespace gestao_financeira.Models
    {
        public class UsarReserva
        {
            public int UsarReservaId { get; set; }
            public int UsuarioId { get; set; }
            public Usuario? Usuario { get; set; }
            public decimal Valor { get; set; }
            public string MotivoUsoReserva { get; set; }
            public DateTime DataUsarReserva { get; set; }
        }
    }

}
