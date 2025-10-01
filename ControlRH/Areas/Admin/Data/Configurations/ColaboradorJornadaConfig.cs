using ControlRH.Areas.Admin.Models;
using ControlRH.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlRH.Areas.Admin.Data.Configurations;

internal class ColaboradorJornadaConfig : EntidadeConfig<ColaboradorJornada>, IEntityTypeConfiguration<ColaboradorJornada>
{
    public override void Configure(EntityTypeBuilder<ColaboradorJornada> builder)
    {
        base.Configure(builder);

        builder.ToTable("ColaboradorJornada");

        builder.HasKey(c => c.Id);

        builder.HasOne(c => c.Colaborador)
            .WithMany(c => c.ColaboradoresJornadas)
            .HasForeignKey(c => c.ColaboradorId)
            .IsRequired();

        builder.HasOne(c => c.JornadaTrabalho)
            .WithMany(c => c.ColaboradoresJornadas)
            .HasForeignKey(c => c.JornadaTrabalhoId)
            .IsRequired();
    }
}