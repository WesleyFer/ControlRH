using ControlRH.Core.Data;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ControlRH.Areas.Colaborador.Models;

namespace ControlRH.Areas.Colaborador.Data.Configurations;

internal class PontoEletronicoConfig : AggregateRootConfig<PontoEletronico>, IEntityTypeConfiguration<PontoEletronico>
{
    public override void Configure(EntityTypeBuilder<PontoEletronico> builder)
    {
        base.Configure(builder);

        builder.ToTable("PontoEletronico");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Cpf).HasMaxLength(14).IsRequired();
        builder.Property(c => c.Pis).HasMaxLength(11).IsRequired();
        builder.Property(c => c.DataHora).IsRequired();
        builder.Property(c => c.Marcacao).HasConversion<string>().IsRequired();
        builder.Property(c => c.EnderecoIp).IsRequired(false);
        builder.Property(c => c.Hostname).IsRequired(false);
        builder.Property(c => c.HashUnico).IsRequired();

    }
}