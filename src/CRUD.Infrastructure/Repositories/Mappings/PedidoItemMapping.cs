using CRUD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRUD.Infrastructure.Repositories.Mappings
{
    public class PedidoItemMapping: IEntityTypeConfiguration<PedidoItem>
    {
        public void Configure(EntityTypeBuilder<PedidoItem> builder)
        {
            builder.ToTable("PedidoItens");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Quantidade).HasDefaultValue(1).IsRequired(); //Outra Forma de inserir valor default
            builder.Property(p => p.Valor).HasColumnType("DECIMAL(6,2)").IsRequired();
            builder.Property(p => p.Desconto).HasColumnType("DECIMAL(6,2)").IsRequired();
        }
    }
}