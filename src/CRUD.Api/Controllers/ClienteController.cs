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

        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> AtualizaCliente(Cliente clienteNew)
        {
             var clienteOld = _AppDbContext.Clientes.Find(clienteNew.Id);
            _AppDbContext.Clientes.Update(clienteNew);

            return Ok(new
            {
                success = true,
                data = clienteNew
            });
        }

        [HttpDelete]
        [Route("[action]")]
        public async Task<IActionResult> RemoveCliente(Cliente cliente)
        {
            _AppDbContext.Entry(_AppDbContext.Clientes.Find(cliente)).State = EntityState.Deleted;
            await _AppDbContext.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = _AppDbContext.Clientes.ToListAsync()
            });
        }

    }
}