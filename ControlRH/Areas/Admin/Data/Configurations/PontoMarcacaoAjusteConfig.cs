using ControlRH.Areas.Admin.Models;
using ControlRH.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlRH.Areas.Admin.Data.Configurations;

internal class PontoMarcacaoAjusteConfig : AggregateRootConfig<PontoMarcacaoAjuste>, IEntityTypeConfiguration<PontoMarcacaoAjuste>
{
    public override void Configure(EntityTypeBuilder<PontoMarcacaoAjuste> builder)
    {
        builder.ToTable("PontoMarcacaoAjuste");

        builder.HasKey(c => c.Id);
    }
}