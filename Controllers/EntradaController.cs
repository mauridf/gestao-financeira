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
    public class EntradaController : ControllerBase
    {
        private readonly FinanceContext _context;

        public EntradaController(FinanceContext context)
        {
            _context = context;
        }

        [HttpGet("Entradas/{usuarioId}")]
        public async Task<IActionResult> GetEntradas(int usuarioId)
        {
            var entradas = await _context.Set<Entrada>().Where(e => e.UsuarioId == usuarioId).ToListAsync();
            return Ok(entradas);
        }

        [HttpGet("Entrada/{usuarioId}/{id}")]
        public async Task<IActionResult> GetEntrada(int usuarioId, int id)
        {
            var entrada = await _context.Set<Entrada>().Where(e => e.UsuarioId == usuarioId && e.EntradaId == id).FirstOrDefaultAsync();
            if (entrada == null) return NotFound();
            return Ok(entrada);
        }

        [HttpPost("RegistrarEntrada")]
        public async Task<IActionResult> CreateEntrada([FromBody] Entrada entrada)
        {
            _context.Set<Entrada>().Add(entrada);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEntrada), new { usuarioId = entrada.UsuarioId, id = entrada.EntradaId }, entrada);
        }

        [HttpPut("EditarEntrada/{usuarioId}/{id}")]
        public async Task<IActionResult> UpdateEntrada(int usuarioId, int id, [FromBody] Entrada entrada)
        {
            if (id != entrada.EntradaId || usuarioId != entrada.UsuarioId) return BadRequest();
            _context.Entry(entrada).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("DeletarEntrada/{usuarioId}/{id}")]
        public async Task<IActionResult> DeleteEntrada(int usuarioId, int id)
        {
            var entrada = await _context.Set<Entrada>().Where(e => e.UsuarioId == usuarioId && e.EntradaId == id).FirstOrDefaultAsync();
            if (entrada == null) return NotFound();
            _context.Set<Entrada>().Remove(entrada);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}