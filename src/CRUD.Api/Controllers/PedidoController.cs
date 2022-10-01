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
        private readonly AppDbContext _AppDbContext;

        public PedidoController(AppDbContext AppDbContext)
        {
            _AppDbContext = AppDbContext;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetPedido()
        {
            return Ok(new
            {
                success = true,
                data = _AppDbContext.Pedidos.ToListAsync()
            });
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> InserePedido(Pedido pedido)
        {
            _AppDbContext.Pedidos.Add(pedido);
            await _AppDbContext.SaveChangesAsync();

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
             var pedidoOld = _AppDbContext.Clientes.Find(pedidoNew.Id);
            _AppDbContext.Pedidos.Update(pedidoNew);

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
            _AppDbContext.Entry(_AppDbContext.Pedidos.Find(pedido)).State = EntityState.Deleted;
            await _AppDbContext.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = _AppDbContext.Pedidos.ToListAsync()
            });
        }

    }
}