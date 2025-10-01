using ControlRH.Areas.Admin.Models;
using ControlRH.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlRH.Areas.Admin.Data.Configurations;

internal class JornadaTrabalhoConfig : AggregateRootConfig<JornadaTrabalho>, IEntityTypeConfiguration<JornadaTrabalho>
{
    public override void Configure(EntityTypeBuilder<JornadaTrabalho> builder)
    {
        base.Configure(builder);

        builder.ToTable("JornadaTrabalho");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Nome)
           .HasMaxLength(100)
           .IsRequired();

        builder.HasMany(c => c.JornadasTrabalhosHorarios)
            .WithOne(c => c.JornadaTrabalho)
            .HasForeignKey(c => c.JornadaTrabalhoId)
            .IsRequired();

        builder.HasMany(c => c.ColaboradoresJornadas)
            .WithOne(c => c.JornadaTrabalho)
            .HasForeignKey(c => c.JornadaTrabalhoId)
            .IsRequired();
    }
}