using System.Threading.Tasks;
using CRUD.Infrastructure.Repositories.Context;
using Microsoft.AspNetCore.Mvc;
using CRUD.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetClientes()
        {
            return Ok(new
            {
                success = true,
                data = _AppDbContext.Clientes.ToListAsync()
            });
        }

        [HttpPost]
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

        [HttpPatch]
        [Route("[action]")]
        public async Task<IActionResult> AtualizaCliente(Cliente clienteNew)
        {
            var clienteOld = _AppDbContext.Clientes.Find(clienteNew.Id);

            if (clienteOld == null)
            {
                return new NoContentResult();
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

        [HttpDelete]
        [Route("[action]")]
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