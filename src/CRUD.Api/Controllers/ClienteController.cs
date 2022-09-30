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
        private readonly AppContext _appContext;

        public ClienteController(AppContext appContext)
        {
            _appContext = appContext;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetClientes()
        {
            return Ok(new
            {
                success = true,
                data = _appContext.Clientes.ToListAsync()
            });
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> InsereCliente(Cliente cliente)
        {
            _appContext.Clientes.Add(cliente);
            await _appContext.SaveChangesAsync();

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
             var clienteOld = _appContext.Clientes.Find(clienteNew.Id);
            _appContext.Clientes.Update(clienteNew);

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
            _appContext.Entry(_appContext.Clientes.Find(cliente)).State = EntityState.Deleted;
            await _appContext.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = _appContext.Clientes.ToListAsync()
            });
        }

    }
}