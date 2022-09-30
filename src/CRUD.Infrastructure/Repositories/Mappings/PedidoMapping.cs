using CRUD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRUD.Infrastructure.Repositories.Mappings
{
    public class PedidoMapping: IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.ToTable("Pedidos");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Iniciadoem).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd(); //Configura DefaultValue no momento da geração da instrução SQL 
            builder.Property(p => p.Status).HasConversion<string>();
            builder.Property(p => p.TipoFrete).HasConversion<int>(); //Grava o int do Enum
            builder.Property(p => p.Observacao).HasColumnType("VARCHAR(512)");

            builder.HasMany(p => p.Itens) //Informa Relacionamento 1/N
                .WithOne(p => p.Pedido)
                .OnDelete(DeleteBehavior.Cascade); //Informa a necessidade de fazer o Delete Cascade na deleção do Pedido
        }
    }
}