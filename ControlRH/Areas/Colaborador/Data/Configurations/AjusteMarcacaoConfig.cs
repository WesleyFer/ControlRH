using ControlRH.Areas.Colaborador.Models;
using ControlRH.Core.Data;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ControlRH.Areas.Colaborador.Data.Configurations;

internal class AjusteMarcacaoConfig : AggregateRootConfig<AjusteMarcacao>, IEntityTypeConfiguration<AjusteMarcacao>
{
    public override void Configure(EntityTypeBuilder<AjusteMarcacao> builder)
    {
        base.Configure(builder);

        builder.ToTable("AjusteMarcacao");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.PontoEletronicoId).IsRequired();
        builder.Property(c => c.DataHora).IsRequired();
        builder.Property(c => c.Justificativa).HasMaxLength(255).IsRequired();
        builder.Property(c => c.NomeResponsavelAjuste).HasMaxLength(255).IsRequired();
        builder.Property(c => c.CpfResponsavelAjuste).HasMaxLength(255).IsRequired();

        builder.HasOne(c => c.PontoEletronico)
              .WithMany(c => c.AjustesMarcacoes) // ou WithOne() se for 1:1
              .HasForeignKey(c => c.PontoEletronicoId)
              .OnDelete(DeleteBehavior.Restrict);

    }
}