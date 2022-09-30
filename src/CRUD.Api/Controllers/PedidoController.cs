using System.Threading.Tasks;
using CRUD.Domain.Entities;
using CRUD.Infrastructure.Repositories.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly AppContext _appContext;

        public PedidoController(AppContext appContext)
        {
            _appContext = appContext;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetPedido()
        {
            return Ok(new
            {
                success = true,
                data = _appContext.Pedidos.ToListAsync()
            });
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> InserePedido(Pedido pedido)
        {
            _appContext.Pedidos.Add(pedido);
            await _appContext.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = pedido
            });
        }

        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> AtualizaPedido(Pedido pedidoNew)
        {
             var pedidoOld = _appContext.Clientes.Find(pedidoNew.Id);
            _appContext.Pedidos.Update(pedidoNew);

            return Ok(new
            {
                success = true,
                data = pedidoNew
            });
        }

        [HttpDelete]
        [Route("[action]")]
        public async Task<IActionResult> RemovePedido(Pedido pedido)
        {
            _appContext.Entry(_appContext.Pedidos.Find(pedido)).State = EntityState.Deleted;
            await _appContext.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = _appContext.Pedidos.ToListAsync()
            });
        }

    }
}