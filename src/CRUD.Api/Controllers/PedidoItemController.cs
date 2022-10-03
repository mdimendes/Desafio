using System.Linq;
using System.Threading.Tasks;
using CRUD.Domain.Entities;
using CRUD.Infrastructure.Repositories.Context;
using Microsoft.AspNetCore.Http;
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

        /// <summary>
        /// List all PedidoItens of a specific Pedido.
        /// </summary>
        /// <param name="pedidoId"></param>        
        /// <returns>The list of PedidoItens of a Pedido</returns>
        /// <response code="200">Returns the List of PedidoItens of a Pedido</response>
        /// <response code="204">If PedidoItens was not found</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("{pedidoId}")]
        public async Task<IActionResult> GetPedidoItensByPedidoId(int pedidoId)
        {
            var itensPedido = _AppDbContext.PedidoItens
                .Where(p => p.PedidoId == pedidoId)
                .OrderBy(p => p.Id)
                .ToList();

            if (itensPedido == null)
            {
                return new BadRequestResult();
            }
            else if (itensPedido.Count == 0)
            {
                return new NoContentResult();
            }

            return Ok(new
            {
                success = true,
                data = _AppDbContext.PedidoItens.ToListAsync()
            });
        }

        /// <summary>
        /// Lists a PedidoItem.
        /// </summary>
        /// <param name="pedidoItemId"></param>        
        /// <returns>The PedidoItem record</returns>
        /// <response code="200">Returns the PedidoItem record</response>
        /// <response code="204">If PedidoItem was not found</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("{pedidoItemId}")]
        public async Task<IActionResult> GetPedidoItens(int pedidoItemId)
        {
            var pedidoItem = _AppDbContext.PedidoItens
                .Where(p => p.Id == pedidoItemId)
                .OrderBy(p => p.Id)
                .ToList();

            if (pedidoItem == null)
            {
                return new BadRequestResult();
            }
            else if (pedidoItem.Count == 0)
            {
                return new NoContentResult();
            }
            
            return Ok(new
            {
                success = true,
                data = pedidoItem
            });
        }

       /// <summary>
        /// Creates a PedidoItem record.
        /// </summary>
        /// <param name="pedidoItem"></param>        
        /// <returns>The PedidoItem record created</returns>
        /// <response code="201">Returns the PedidoItem created</response>
        /// <response code="400">If PedidoItem could not be created</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Updates a specific PedidoItem.
        /// </summary>
        /// <param name="pedidoItemNew"></param>        
        /// <returns>The updated PedidoItem</returns>
        /// <response code="200">Returns the updated PedidoItem</response>
        /// <response code="204">If the PedidoItem was not found</response>
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("[action]")]
        public async Task<IActionResult> AtualizaPedidoItem(PedidoItem pedidoItemNew)
        {
            var pedidoItemOld = _AppDbContext.PedidoItens.Find(pedidoItemNew.Id);

            if (pedidoItemOld == null)
            {
                return new NoContentResult();
            }

            pedidoItemOld.Desconto = (pedidoItemNew.Desconto == null) ? pedidoItemOld.Desconto : pedidoItemNew.Desconto;
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

        /// <summary>
        /// Deletes a PedidoItem.
        /// </summary>
        /// <param name="pedidoItemId"></param>        
        /// <returns>List of PedidoItens</returns>
        /// <response code="200">Returns list of PedidoItens</response>
        /// <response code="404">If the PedidoItem was not found</response>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("{Id}")]
        public async Task<IActionResult> RemovePedidoItem(int pedidoItemId)
        {
            _AppDbContext.Entry(_AppDbContext.PedidoItens.Find(pedidoItemId)).State = EntityState.Deleted;
            await _AppDbContext.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = _AppDbContext.PedidoItens.ToListAsync()
            });
        }

    }
}