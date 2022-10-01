using System.Threading.Tasks;
using CRUD.Domain.Entities;
using CRUD.Infrastructure.Repositories.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PedidoItemController: ControllerBase
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
            //    data = _AppDbContext.PedidoItens.ToListAsync()
            });
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> InserePedidoItem(PedidoItem pedidoItem)
        {
            //_AppDbContext.PedidoItens.Add(pedidoItem);
            await _AppDbContext.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = pedidoItem
            });
        }

        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> AtualizaPedidoItem(PedidoItem pedidoItemNew)
        {
            // var pedidoItemOld = _AppDbContext.PedidoItens.Find(pedidoItemNew.Id);
           // _AppDbContext.PedidoItens.Update(pedidoItemNew);

            return Ok(new
            {
                success = true,
                data = pedidoItemNew
            });
        }

        [HttpDelete]
        [Route("[action]")]
        public async Task<IActionResult> RemovePedidoItem(PedidoItem pedidoItem)
        {
            //_AppDbContext.Entry(_AppDbContext.PedidoItens.Find(PedidoItem)).State = EntityState.Deleted;
            await _AppDbContext.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                //data = _AppDbContext.PedidoItens.ToListAsync()
            });
        }

    }
}