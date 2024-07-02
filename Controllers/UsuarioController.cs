using gestao_financeira.Data;
using gestao_financeira.Models;
using gestao_financeira.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace gestao_financeira.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly FinanceContext _context;

        public UsuarioController(FinanceContext context)
        {
            _context = context;
        }

        [HttpPost("RegistrarUsuario")]
        public async Task<ActionResult<Usuario>> RegistrarUsuario(Usuario usuario)
        {
            usuario.Senha = PasswordHasher.HashPassword(usuario.Senha); // Criptografando a senha
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.UsuarioId }, usuario);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<Usuario>> Login([FromBody] LoginModel login)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == login.Email);

            if (usuario == null || !PasswordHasher.VerifyPassword(login.Senha, usuario.Senha))
            {
                return Unauthorized();
            }

            return Ok(usuario);
        }

        [HttpGet("Usuario/{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }
    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Senha { get; set; }
    }
}
