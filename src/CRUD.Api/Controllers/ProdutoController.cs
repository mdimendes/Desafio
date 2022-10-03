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
    public class ProdutoController : ControllerBase
    {
        private readonly AppDbContext _AppDbContext;

        public ProdutoController(AppDbContext AppDbContext)
        {
            _AppDbContext = AppDbContext;
        }

        /// <summary>
        /// List a specific Product.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="codigoBarras"></param>
        /// <returns>The corresponding Product</returns>
        /// <response code="200">Returns the corresponding Product</response>
        /// <response code="204">If the Product was not found</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("{id},{codigoBarras}")]
        public async Task<IActionResult> GetProduto([FromRoute] int id, [FromRoute] string codigoBarras)
        {

            var produtoFound = _AppDbContext.Produtos
                .Where(p => p.Id == id || p.CodigoBarras == codigoBarras)
                .OrderBy(p => p.Id)
                .ToList();
            //_AppDbContext.Produtos.Find(produto.Id);

            if (produtoFound == null)
            {
                return new BadRequestResult();
            }
            else if (produtoFound.Count == 0)
            {
                return new NoContentResult();
            }

            return Ok(new
            {
                success = true,
                data = produtoFound
            });
        }

        /// <summary>
        /// List Products.
        /// </summary>        
        /// <returns>The List of Products</returns>
        /// <response code="200">Returns the List of Product</response>
        /// <response code="204">If the Products were not found</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("[action]")]
        public async Task<IActionResult> GetAllProdutos()
        {
            return Ok(new
            {
                success = true,
                data = _AppDbContext.Produtos.ToListAsync()
            });
        }

        /// <summary>
        /// Creates a Product record.
        /// </summary>
        /// <param name="produto"></param>        
        /// <returns>The Product record created</returns>
        /// <response code="201">Returns the Product created</response>
        /// <response code="400">If Product could not be created</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("[action]")]
        public async Task<IActionResult> InsereProduto(Produto produto)
        {
            produto.Ativo = (produto.Ativo == null) ? true : produto.Ativo;
            _AppDbContext.Produtos.Add(produto);
            await _AppDbContext.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = produto
            });
        }

        /// <summary>
        /// Updates a specific Product.
        /// </summary>
        /// <param name="produtoNew"></param>        
        /// <returns>The updated Product</returns>
        /// <response code="200">Returns the updated Product</response>
        /// <response code="204">If the Product was not found</response>
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("[action]")]
        public async Task<IActionResult> AtualizaProduto(Produto produtoNew)
        {
            var produtoOld = _AppDbContext.Produtos.Find(produtoNew.Id);

            if (produtoOld == null)
            {
                return new NotFoundResult();
            }

            produtoOld.Descricao = string.IsNullOrEmpty(produtoNew.Descricao) ? produtoOld.Descricao : produtoNew.Descricao;
            produtoOld.CodigoBarras = string.IsNullOrEmpty(produtoNew.CodigoBarras) ? produtoOld.CodigoBarras : produtoNew.CodigoBarras;
            produtoOld.TipoProduto = (produtoNew.TipoProduto == null) ? produtoOld.TipoProduto : produtoNew.TipoProduto;
            produtoOld.Valor = (produtoNew.Valor == null) ? produtoOld.Valor : produtoNew.Valor;
            produtoOld.Ativo = (produtoNew.Ativo == null) ? produtoOld.Ativo : produtoNew.Ativo;

            _AppDbContext.Produtos.Update(produtoOld);
            await _AppDbContext.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = produtoOld
            });
        }

        /// <summary>
        /// Deletes a Product.
        /// </summary>
        /// <param name="Id"></param>        
        /// <returns>List of products</returns>
        /// <response code="200">Returns list of Products</response>
        /// <response code="404">If the Product was not found</response>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("{Id}")]
        public async Task<IActionResult> RemoveProduto(int Id)
        {
            _AppDbContext.Entry(_AppDbContext.Produtos.Find(Id)).State = EntityState.Deleted;
            await _AppDbContext.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = _AppDbContext.Produtos.ToListAsync()
            });
        }
    }
}