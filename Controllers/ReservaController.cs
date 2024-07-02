using gestao_financeira.Data;
using gestao_financeira.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace gestao_financeira.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservaController : ControllerBase
    {
        private readonly FinanceContext _context;

        public ReservaController(FinanceContext context)
        {
            _context = context;
        }

        [HttpGet("Reservas/{usuarioId}")]
        public async Task<IActionResult> GetReservas(int usuarioId)
        {
            var reservas = await _context.Set<Reserva>().Where(e => e.UsuarioId == usuarioId).ToListAsync();
            return Ok(reservas);
        }

        [HttpGet("Reserva/{usuarioId}/{id}")]
        public async Task<IActionResult> GetReserva(int usuarioId, int id)
        {
            var reserva = await _context.Set<Reserva>().Where(e => e.UsuarioId == usuarioId && e.ReservaId == id).FirstOrDefaultAsync();
            if (reserva == null) return NotFound();
            return Ok(reserva);
        }

        [HttpPost("RegistrarReserva")]
        public async Task<IActionResult> CreateReserva([FromBody] Reserva reserva)
        {
            // Verificar se a reserva é maior que o saldo
            var totalEntradas = _context.Set<Entrada>().Where(e => e.UsuarioId == reserva.UsuarioId).Sum(e => e.Valor);
            var totalSaidas = _context.Set<Saida>().Where(s => s.UsuarioId == reserva.UsuarioId).Sum(s => s.Valor);
            var saldo = totalEntradas - totalSaidas;

            if (reserva.Valor > saldo)
            {
                return BadRequest("O valor da reserva não pode ser maior que o saldo.");
            }

            _context.Set<Reserva>().Add(reserva);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetReserva), new { id = reserva.ReservaId }, reserva);
        }

        [HttpPut("EditarReserva/{usuarioId}/{id}")]
        public async Task<IActionResult> UpdateReserva(int usuarioId, int id, [FromBody] Reserva reserva)
        {
            if (id != reserva.ReservaId || usuarioId != reserva.UsuarioId) return BadRequest();
            _context.Entry(reserva).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("DeletarReserva/{usuarioId}/{id}")]
        public async Task<IActionResult> DeleteReserva(int usuarioId, int id)
        {
            var reserva = await _context.Set<Reserva>().Where(e => e.UsuarioId == usuarioId && e.ReservaId == id).FirstOrDefaultAsync();

            if (reserva == null) return NotFound();
            _context.Set<Reserva>().Remove(reserva);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}