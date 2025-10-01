using ControlRH.Areas.Admin.Models;
using ControlRH.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlRH.Areas.Admin.Data.Configurations;

internal class DocumentoConfig : AggregateRootConfig<Documento>, IEntityTypeConfiguration<Documento>
{
    public override void Configure(EntityTypeBuilder<Documento> builder)
    {
        base.Configure(builder);

        builder.ToTable("Documento");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Nome)
           .HasMaxLength(200)
           .IsRequired();

        builder.Property(c => c.Descricao)
          .HasMaxLength(500)
          .IsRequired(false);

        builder.Property(d => d.CaminhoArquivo)
           .HasMaxLength(500)
           .IsRequired();

        builder.HasOne(d => d.CarteiraCliente)
            .WithMany(c => c.Documentos)
            .HasForeignKey(d => d.CarteiraClienteId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(d => d.Colaborador)
            .WithMany(c => c.Documentos)
            .HasForeignKey(d => d.ColaboradorId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}