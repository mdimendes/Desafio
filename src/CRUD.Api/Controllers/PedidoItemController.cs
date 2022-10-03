using System.Threading.Tasks;
using CRUD.Domain.Entities;
using CRUD.Infrastructure.Repositories.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PedidoItemController : ControllerBase
    {
        private readonly AppDbContext _AppDbContext;

        public PedidoItemController(AppDbContext AppDbContext)
        {
            _AppDbContext = AppDbContext;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetPedidoItens()
        {
            return Ok(new
            {
                success = true,
                data = _AppDbContext.PedidoItens.ToListAsync()
            });
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> InserePedidoItem(PedidoItem pedidoItem)
        {
            _AppDbContext.PedidoItens.Add(pedidoItem);
            await _AppDbContext.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = pedidoItem
            });
        }

        [HttpPatch]
        [Route("[action]")]
        public async Task<IActionResult> AtualizaPedidoItem(PedidoItem pedidoItemNew)
        {
            var pedidoItemOld = _AppDbContext.PedidoItens.Find(pedidoItemNew.Id);

            if (pedidoItemOld == null)
            {
                return new NoContentResult();
            }

            pedidoItemOld.Desconto = (pedidoItemNew.Desconto == null) ? pedidoItemOld.Desconto: pedidoItemNew.Desconto;
            pedidoItemOld.Quantidade = (pedidoItemNew.Quantidade == null) ? pedidoItemOld.Quantidade : pedidoItemNew.Quantidade;
            pedidoItemOld.Valor = (pedidoItemNew.Valor == null) ? pedidoItemOld.Valor : pedidoItemNew.Valor;

            _AppDbContext.PedidoItens.Update(pedidoItemOld);
            await _AppDbContext.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = pedidoItemOld
            });
        }

        [HttpDelete]
        [Route("[action]")]
        public async Task<IActionResult> RemovePedidoItem(PedidoItem pedidoItem)
        {
            _AppDbContext.Entry(_AppDbContext.PedidoItens.Find(pedidoItem.Id)).State = EntityState.Deleted;
            await _AppDbContext.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = _AppDbContext.PedidoItens.ToListAsync()
            });
        }

    }
}