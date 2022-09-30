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
        private readonly AppContext _appContext;

        public ProdutoController(AppContext appContext)
        {
            _appContext = appContext;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetProdutos()
        {
            return Ok(new
            {
                success = true,
                data = _appContext.Produtos.ToListAsync()
            });
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> InsereProduto(Produto produto)
        {
            _appContext.Produtos.Add(produto);
            await _appContext.SaveChangesAsync();

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
             var produtoOld = _appContext.Produtos.Find(produtoNew.Id);
            _appContext.Produtos.Update(produtoNew);

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
            _appContext.Entry(_appContext.Produtos.Find(produto)).State = EntityState.Deleted;
            await _appContext.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = _appContext.Produtos.ToListAsync()
            });
        }
    }
}