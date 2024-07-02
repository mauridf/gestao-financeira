using gestao_financeira.Data;
using gestao_financeira.Models;
using gestao_financeira.Models.gestao_financeira.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace gestao_financeira.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly FinanceContext _context;

        public DashboardController(FinanceContext context)
        {
            _context = context;
        }

        [HttpGet("Resumo")]
        public async Task<IActionResult> GetResumo([FromQuery] int usuarioId, [FromQuery] int mesInicio, [FromQuery] int anoInicio, [FromQuery] int mesFim, [FromQuery] int anoFim)
        {
            DateTime dataInicio = new DateTime(anoInicio, mesInicio, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime dataFim = new DateTime(anoFim, mesFim, DateTime.DaysInMonth(anoFim, mesFim), 23, 59, 59, DateTimeKind.Utc);

            var totalEntradas = await _context.Set<Entrada>()
                .Where(e => e.UsuarioId == usuarioId && e.DataEntrada >= dataInicio && e.DataEntrada <= dataFim)
                .SumAsync(e => e.Valor);

            var totalSaidas = await _context.Set<Saida>()
                .Where(s => s.UsuarioId == usuarioId && s.DataSaida >= dataInicio && s.DataSaida <= dataFim)
                .SumAsync(s => s.Valor);

            var saldo = totalEntradas - totalSaidas;

            var totalReservas = await _context.Set<Reserva>()
                .Where(r => r.UsuarioId == usuarioId && r.DataReserva >= dataInicio && r.DataReserva <= dataFim)
                .SumAsync(r => r.Valor);

            var totalReservasUsadas = await _context.Set<UsarReserva>()
                .Where(r => r.UsuarioId == usuarioId && r.DataUsarReserva >= dataInicio && r.DataUsarReserva <= dataFim)
                .SumAsync(r => r.Valor);

            return Ok(new
            {
                TotalEntradas = totalEntradas,
                TotalSaidas = totalSaidas,
                Saldo = saldo,
                TotalReservas = totalReservas,
                TotalReservasUsadas = totalReservasUsadas
            });
        }
    }
}