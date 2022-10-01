using System.Threading.Tasks;
using CRUD.Domain.Entities;
using CRUD.Infrastructure.Repositories.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutoController: ControllerBase
    {
        private readonly AppDbContext _AppDbContext;

        public ProdutoController(AppDbContext AppDbContext)
        {
            _AppDbContext = AppDbContext;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetProdutos()
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
            _AppDbContext.Produtos.Add(produto);
            await _AppDbContext.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = produto
            });
        }

        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> AtualizaProduto(Produto produtoNew)
        {
             var produtoOld = _AppDbContext.Produtos.Find(produtoNew.Id);
            _AppDbContext.Produtos.Update(produtoNew);

            return Ok(new
            {
                success = true,
                data = produtoNew
            });
        }

        [HttpDelete]
        [Route("[action]")]
        public async Task<IActionResult> RemoveProduto(Produto produto)
        {
            _AppDbContext.Entry(_AppDbContext.Produtos.Find(produto)).State = EntityState.Deleted;
            await _AppDbContext.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = _AppDbContext.Produtos.ToListAsync()
            });
        }
    }
}