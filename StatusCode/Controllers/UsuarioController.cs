using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StatusCode.Models;

namespace StatusCode.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private SistemaContext DbSistema = new SistemaContext();

        [HttpGet]
        public ActionResult RequererTodos()
        {
            var DbUsuarios = DbSistema.Usuario;
            if (!DbUsuarios.Any())
            {
                return NotFound(new { mensenger = "Sem usuários cadastrados." });
            }
            return Ok(DbUsuarios);

        }

        [HttpPost]
        [Route("Autenticar")]
        //[AllowAnonymous]
        public ActionResult<dynamic> Autenticar(Usuario usuario)
        {
            var DbUsuario = DbSistema.Usuario.Where(Usuario => Usuario.Nome == usuario.Nome);

            if (DbUsuario == null)
            {
                return NotFound(new { mensenger = "Usuário não encontrado." });
            }
            else
            {
                usuario.Senha = String.Empty;
                return Ok(usuario);
            }
        }

        [HttpGet("{Id}")]
        public ActionResult RequererUmPelaId(int Id)
        {
            var DbUsuario = DbSistema.Usuario
                .Where(Usuario => Usuario.Id == Id);
            if (!DbUsuario.Any())
            {
                return NotFound(new { mensenger = "Usuário não encontrado." });
            }
            return Ok(DbUsuario);

        }

        [HttpPost]
        public ActionResult PublicarUm(Usuario Usuario)
        {
            if (ExisteCpf(Usuario.Cpf))
            {
                return Conflict();
            }
            else
            {
                DbSistema.Usuario.Add(Usuario);
                DbSistema.SaveChanges();

                return CreatedAtAction("Publicado um usuario", new { id = Usuario.Id }, Usuario);
            }


        }

        [HttpDelete("{Id}")]
        public ActionResult DeletarUmPelaId(int Id, Usuario Usuario)
        {
            var DbUsuario = DbSistema.Usuario
                .Where(Usuario => Usuario.Id == Id);
            if (!DbUsuario.Any())
            {
                return NotFound(new { mensenger = "Usuário não encontrado." });
            }

            return Ok(DbUsuario);

        }

        [HttpPut]
        public void SubstituirUmPelaId(int Id, Usuario Usuario)
        {

        }

        private bool ExisteCpf(string Cpf)
        {
            return DbSistema.Usuario.Any(Coluna => Coluna.Cpf == Cpf);
        }
    }


}

