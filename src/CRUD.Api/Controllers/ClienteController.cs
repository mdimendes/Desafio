using System.Threading.Tasks;
using CRUD.Infrastructure.Repositories.Context;
using Microsoft.AspNetCore.Mvc;
using CRUD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace CRUD.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly AppDbContext _AppDbContext;

        public ClienteController(AppDbContext AppDbContext)
        {
            _AppDbContext = AppDbContext;
        }

        /// <summary>
        /// List a specific Cliente.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="telefone"></param>
        /// <returns>The corresponding Cliente</returns>
        /// <response code="200">Returns the corresponding Cliente</response>
        /// <response code="204">If the Cliente was not found</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("{id},{telefone}")]
        public async Task<IActionResult> GetCliente([FromRoute] int id, [FromRoute] string telefone)
        {
            var clienteFound = _AppDbContext.Clientes
                .Where(p => p.Id == id || p.Telefone == telefone)
                .OrderBy(p => p.Id)
                .ToList();

            if (clienteFound == null)
            {
                return new BadRequestResult();
            }
            else if (clienteFound.Count == 0)
            {
                return new NoContentResult();
            }

            return Ok(new
            {
                success = true,
                data = _AppDbContext.Clientes.ToListAsync()
            });
        }

        /// <summary>
        /// List Clientes.
        /// </summary>        
        /// <returns>The List of Clientes</returns>
        /// <response code="200">Returns the List of Clientes</response>
        /// <response code="204">If the Clientes were not found</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("[action]")]
        public async Task<IActionResult> GetAllClientes()
        {
            return Ok(new
            {
                success = true,
                data = _AppDbContext.Clientes.ToListAsync()
            });
        }

        /// <summary>
        /// Creates a Cliente record.
        /// </summary>
        /// <param name="cliente"></param>        
        /// <returns>The Cliente record created</returns>
        /// <response code="201">Returns the Cliente created</response>
        /// <response code="400">If Cliente could not be created</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("[action]")]
        public async Task<IActionResult> InsereCliente(Cliente cliente)
        {
            _AppDbContext.Clientes.Add(cliente);
            await _AppDbContext.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = cliente
            });
        }

        /// <summary>
        /// Updates a specific Cliente.
        /// </summary>
        /// <param name="clienteNew"></param>        
        /// <returns>The updated cliente</returns>
        /// <response code="200">Returns the updated Cliente</response>
        /// <response code="204">If the Cliente was not found</response>
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("[action]")]
        public async Task<IActionResult> AtualizaCliente(Cliente clienteNew)
        {
            var clienteOld = _AppDbContext.Clientes.Find(clienteNew.Id);

            if (clienteOld == null)
            {
                return new NotFoundResult();
            }

            clienteOld.Nome = string.IsNullOrEmpty(clienteNew.Nome) ? clienteOld.Nome : clienteNew.Nome;
            clienteOld.Telefone = string.IsNullOrEmpty(clienteNew.Telefone) ? clienteOld.Telefone : clienteNew.Telefone;
            clienteOld.Cidade = string.IsNullOrEmpty(clienteNew.Cidade) ? clienteOld.Cidade : clienteNew.Cidade;
            clienteOld.Estado = string.IsNullOrEmpty(clienteNew.Estado) ? clienteOld.Estado : clienteNew.Estado;
            clienteOld.CEP = string.IsNullOrEmpty(clienteNew.CEP) ? clienteOld.CEP : clienteNew.CEP;

            _AppDbContext.Clientes.Update(clienteOld);
            await _AppDbContext.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = clienteOld
            });
        }

        /// <summary>
        /// Deletes a Cliente.
        /// </summary>
        /// <param name="id"></param>        
        /// <returns>List of Clientes</returns>
        /// <response code="200">Returns list of Clientes</response>
        /// <response code="404">If the Cliente was not found</response>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("{Id}")]
        public async Task<IActionResult> RemoveCliente(Cliente cliente)
        {
            _AppDbContext.Entry(_AppDbContext.Clientes.Find(cliente.Id)).State = EntityState.Deleted;
            await _AppDbContext.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = _AppDbContext.Clientes.ToListAsync()
            });
        }

    }
}