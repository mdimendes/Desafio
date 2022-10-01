using CRUD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRUD.Infrastructure.Repositories.Mappings
{
    public class ProdutoMapping: IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.ToTable("Produtos");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.CodigoBarras).HasColumnType("VARCHAR(14)").IsRequired();
            builder.Property(p => p.Descricao).HasColumnType("VARCHAR(60)");
            builder.Property(p => p.Valor).HasColumnType("DECIMAL(6,2)").IsRequired();
            builder.Property(p => p.TipoProduto).HasConversion<string>(); //Grava o nome do Enum
        }
    }
}