using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TreinoWeb.Data;
using TreinoWeb.Models;

namespace TreinoWeb.Repository
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRepository : ControllerBase
    {
        private readonly CentralContext _context;

        public UserRepository(CentralContext centralContext)
        {
            _context = centralContext;
        }

        [HttpGet]
        public async Task<List<UserModel>> GetUsers()
        {
            return await _context.Usuarios.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserModel>> GetUserById(int id)
        {
            var user = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return NotFound(); // Retornar 404 Not Found caso o usuário não seja encontrado.
            }

            return user;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserModel>> CreateUser([FromBody] UserModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_context.Usuarios.Any(u => u.Name.ToLower() == user.Name.ToLower()))
            {
                ModelState.AddModelError("CustomError", "User already exists!");
                return BadRequest(ModelState);
            }

            _context.Usuarios.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        public Task<ActionResult<UserModel>> UpdateUser(int id, [FromBody] JsonPatchDocument<UserModel> patchDocument)
        {
            return UpdateUser(id, patchDocument, ModelState);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<UserModel>> UpdateUser(int id, [FromBody] JsonPatchDocument<UserModel> patchDocument, Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelState)
        {
            var userById = await GetUserById(id);
            if (userById == null)
            {
                return NotFound();
            }
            

            // Aplica o patchDocument no usuário existente.
            patchDocument.ApplyTo(userById.Value, (Microsoft.AspNetCore.JsonPatch.Adapters.IObjectAdapter)modelState);

            // Verifica se houve erro na aplicação do patchDocument.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Salva as alterações no banco de dados.
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            // Retorna o usuário atualizado.
            return userById;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var user = await GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(user.Value);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
