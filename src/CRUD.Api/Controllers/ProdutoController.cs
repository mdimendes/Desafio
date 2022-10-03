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

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllProdutos()
        {
            return Ok(new
            {
                success = true,
                data = _AppDbContext.Produtos.ToListAsync()
            });
        }

        [HttpPost]
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

        [HttpPatch]
        [Route("[action]")]
        public async Task<IActionResult> AtualizaProduto(Produto produtoNew)
        {
            var produtoOld = _AppDbContext.Produtos.Find(produtoNew.Id);

            if (produtoOld == null)
            {
                return new NoContentResult();
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

        [HttpDelete]
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