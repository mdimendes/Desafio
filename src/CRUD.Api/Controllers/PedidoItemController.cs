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
        private readonly AppContext _appContext;

        public PedidoItemController(AppContext appContext)
        {
            _appContext = appContext;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetPedidoItens()
        {
            return Ok(new
            {
                success = true,
            //    data = _appContext.PedidoItens.ToListAsync()
            });
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> InserePedidoItem(PedidoItem pedidoItem)
        {
            //_appContext.PedidoItens.Add(pedidoItem);
            await _appContext.SaveChangesAsync();

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
            // var pedidoItemOld = _appContext.PedidoItens.Find(pedidoItemNew.Id);
           // _appContext.PedidoItens.Update(pedidoItemNew);

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
            //_appContext.Entry(_appContext.PedidoItens.Find(PedidoItem)).State = EntityState.Deleted;
            await _appContext.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                //data = _appContext.PedidoItens.ToListAsync()
            });
        }

    }
}