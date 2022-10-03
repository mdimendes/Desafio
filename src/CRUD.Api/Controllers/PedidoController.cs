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
    public class PedidoController : ControllerBase
    {
        private readonly AppDbContext _AppDbContext;

        public PedidoController(AppDbContext AppDbContext)
        {
            _AppDbContext = AppDbContext;
        }

        /// <summary>
        /// List a specific Pedido.
        /// </summary>
        /// <param name="Id"></param>        
        /// <returns>The corresponding Pedido</returns>
        /// <response code="200">Returns the corresponding Pedido</response>
        /// <response code="204">If the Pedido was not found</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("{id}")]
        public async Task<IActionResult> GetPedido([FromRoute] int id)
        {
            var pedidoFound = _AppDbContext.Pedidos
                .Where(p => p.Id == id)
                .OrderBy(p => p.Id)
                .ToList();

            if (pedidoFound == null)
            {
                return new BadRequestResult();
            }
            else if (pedidoFound.Count == 0)
            {
                return new NoContentResult();
            }

            return Ok(new
            {
                success = true,
                data = _AppDbContext.Pedidos.ToListAsync()
            });
        }

        /// <summary>
        /// List all Pedidos of an specific Cliente.
        /// </summary>    
        /// <param name="clienteId"></param>    
        /// <returns>The List of all Pedidos of an specific Cliente</returns>
        /// <response code="200">Returns the List of Pedidos of the provided Cliente</response>
        /// <response code="204">If the Pedidos were not found</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("[action]")]
        public async Task<IActionResult> GetAllPedidosByClienteId(int clienteId)
        {
            var pedidosCliente = _AppDbContext.Pedidos
                .Where(p => p.Cliente.Id == clienteId)
                .OrderBy(p => p.Id)
                .ToList();

            if (pedidosCliente == null)
            {
                return new BadRequestResult();
            }
            else if (pedidosCliente.Count == 0)
            {
                return new NoContentResult();
            }

            return Ok(new
            {
                success = true,
                data = pedidosCliente
            });
        }
        
        /// <summary>
        /// List Pedidos.
        /// </summary>        
        /// <returns>The List of Pedidos</returns>
        /// <response code="200">Returns the List of Pedidos</response>
        /// <response code="204">If the Pedidos were not found</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("[action]")]
        public async Task<IActionResult> GetAllPedidos()
        {
            return Ok(new
            {
                success = true,
                data = _AppDbContext.Pedidos.ToListAsync()
            });
        }        

        /// <summary>
        /// Creates a Pedido record.
        /// </summary>
        /// <param name="pedido"></param>        
        /// <returns>The Pedido record created</returns>
        /// <response code="201">Returns the Pedido created</response>
        /// <response code="400">If Pedido could not be created</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Updates a specific Pedido.
        /// </summary>
        /// <param name="pedidoNew"></param>        
        /// <returns>The updated Pedido</returns>
        /// <response code="200">Returns the updated Pedido</response>
        /// <response code="204">If the Pedido was not found</response>
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("[action]")]
        public async Task<IActionResult> AtualizaPedido(Pedido pedidoNew)
        {
            var pedidoOld = _AppDbContext.Pedidos.Find(pedidoNew.Id);

            if (pedidoOld == null)
            {
                return new NotFoundResult();
            }

            pedidoOld.Observacao = string.IsNullOrEmpty(pedidoNew.Observacao) ? pedidoOld.Observacao : pedidoNew.Observacao;
            pedidoOld.TipoFrete = (pedidoNew.TipoFrete == null) ? pedidoOld.TipoFrete : pedidoNew.TipoFrete;
            pedidoOld.Status = (pedidoNew.Status == null) ? pedidoOld.Status : pedidoNew.Status;
            pedidoOld.Itens = (pedidoNew.Itens == null) ? pedidoOld.Itens : pedidoNew.Itens;

            _AppDbContext.Pedidos.Update(pedidoOld);
            await _AppDbContext.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = pedidoOld
            });
        }

        /// <summary>
        /// Deletes a Pedido.
        /// </summary>
        /// <param name="Id"></param>        
        /// <returns>List of Pedidos</returns>
        /// <response code="200">Returns list of Pedidos</response>
        /// <response code="404">If the Pedido was not found</response>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("{Id}")]
        public async Task<IActionResult> RemovePedido(int Id)
        {
            _AppDbContext.Entry(_AppDbContext.Pedidos.Find(Id)).State = EntityState.Deleted;
            await _AppDbContext.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = _AppDbContext.Pedidos.ToListAsync()
            });
        }

    }
}