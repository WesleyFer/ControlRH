using ControlRH.Areas.Admin.Models;
using ControlRH.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlRH.Areas.Admin.Data.Configurations;


internal class PermissaoConfig : EntidadeConfig<Permissao>, IEntityTypeConfiguration<Permissao>
{
    public override void Configure(EntityTypeBuilder<Permissao> builder)
    {
        base.Configure(builder);

        builder.ToTable("Permissao");

        builder.HasKey(c => c.Id);

        builder.Property(d => d.Chave)
           .HasMaxLength(100)
           .IsRequired();

        builder.Property(d => d.Valor)
           .HasMaxLength(255)
           .IsRequired();

        builder.Property(d => d.Tipo)
            .HasConversion<string>()
            .IsRequired();
    }
}