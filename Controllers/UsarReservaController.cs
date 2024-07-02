using gestao_financeira.Data;
using gestao_financeira.Models;
using gestao_financeira.Models.gestao_financeira.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace gestao_financeira.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsarReservaController : ControllerBase
    {
        private readonly FinanceContext _context;

        public UsarReservaController(FinanceContext context)
        {
            _context = context;
        }

        [HttpGet("ReservasUsadas/{usuarioId}")]
        public async Task<IActionResult> GetUsarReservas(int usuarioId)
        {
            var usarReservas = await _context.Set<UsarReserva>().Where(ur => ur.UsuarioId == usuarioId).ToListAsync();
            return Ok(usarReservas);
        }

        [HttpGet("ReservaUsada/{usuarioId}/{id}")]
        public async Task<IActionResult> GetUsarReserva(int usuarioId, int id)
        {
            var usarReserva = await _context.Set<UsarReserva>().Where(ur => ur.UsuarioId == usuarioId && ur.UsarReservaId == id).FirstOrDefaultAsync();
            if (usarReserva == null) return NotFound();
            return Ok(usarReserva);
        }

        [HttpPost("UsarReserva")]
        public async Task<IActionResult> CreateUsarReserva([FromBody] UsarReserva usarReserva)
        {
            var totalReserva = _context.Set<Reserva>().Where(r => r.UsuarioId == usarReserva.UsuarioId).Sum(r => r.Valor);
            var totalUsarReserva = _context.Set<UsarReserva>().Where(ur => ur.UsuarioId == usarReserva.UsuarioId).Sum(ur => ur.Valor);
            var saldoReserva = totalReserva - totalUsarReserva;

            if (usarReserva.Valor > saldoReserva)
            {
                return BadRequest("O valor a ser usado da reserva não pode ser maior que o saldo da reserva.");
            }

            _context.Set<UsarReserva>().Add(usarReserva);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUsarReserva), new { usuarioId = usarReserva.UsuarioId, id = usarReserva.UsarReservaId }, usarReserva);
        }

        [HttpPut("EditarReservaUsada/{usuarioId}/{id}")]
        public async Task<IActionResult> UpdateUsarReserva(int usuarioId, int id, [FromBody] UsarReserva usarReserva)
        {
            if (id != usarReserva.UsarReservaId || usuarioId != usarReserva.UsuarioId) return BadRequest();
            _context.Entry(usarReserva).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("DeletarReservaUsada/{usuarioId}/{id}")]
        public async Task<IActionResult> DeleteUsarReserva(int usuarioId, int id)
        {
            var usarReserva = await _context.Set<UsarReserva>().Where(ur => ur.UsuarioId == usuarioId && ur.UsarReservaId == id).FirstOrDefaultAsync();
            if (usarReserva == null) return NotFound();
            _context.Set<UsarReserva>().Remove(usarReserva);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}