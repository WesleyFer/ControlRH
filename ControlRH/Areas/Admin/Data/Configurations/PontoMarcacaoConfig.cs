using ControlRH.Areas.Admin.Models;
using ControlRH.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlRH.Areas.Admin.Data.Configurations;

internal class PontoMarcacaoConfig : AggregateRootConfig<PontoMarcacao>, IEntityTypeConfiguration<PontoMarcacao>
{
    public override void Configure(EntityTypeBuilder<PontoMarcacao> builder)
    {
        builder.ToTable("PontoMarcacao");

        builder.HasKey(c => c.Id);
    }
}