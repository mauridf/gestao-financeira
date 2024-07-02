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
    public class SaidaController : ControllerBase
    {
        private readonly FinanceContext _context;

        public SaidaController(FinanceContext context)
        {
            _context = context;
        }

        [HttpGet("Saidas/{usuarioId}")]
        public async Task<IActionResult> GetSaidas(int usuarioId)
        {
            var saidas = await _context.Set<Saida>().Where(s => s.UsuarioId == usuarioId).ToListAsync();
            return Ok(saidas);
        }

        [HttpGet("Saida/{usuarioId}/{id}")]
        public async Task<IActionResult> GetSaida(int usuarioId, int id)
        {
            var saida = await _context.Set<Saida>().Where(s => s.UsuarioId == usuarioId && s.SaidaId == id).FirstOrDefaultAsync();
            if (saida == null) return NotFound();
            return Ok(saida);
        }

        [HttpPost("RegistrarSaida")]
        public async Task<IActionResult> CreateSaida([FromBody] Saida saida)
        {
            _context.Set<Saida>().Add(saida);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSaida), new { usuarioId = saida.UsuarioId, id = saida.SaidaId }, saida);
        }

        [HttpPut("EditarSaida/{usuarioId}/{id}")]
        public async Task<IActionResult> UpdateSaida(int usuarioId, int id, [FromBody] Saida saida)
        {
            if (id != saida.SaidaId || usuarioId != saida.UsuarioId) return BadRequest();
            _context.Entry(saida).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("DeletarSaida/{usuarioId}/{id}")]
        public async Task<IActionResult> DeleteSaida(int usuarioId, int id)
        {
            var saida = await _context.Set<Saida>().Where(s => s.UsuarioId == usuarioId && s.SaidaId == id).FirstOrDefaultAsync();
            if (saida == null) return NotFound();
            _context.Set<Saida>().Remove(saida);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}