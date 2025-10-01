using ControlRH.Areas.Admin.Models;
using ControlRH.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlRH.Areas.Admin.Data.Configurations;

internal class JornadaTrabalhoHorarioConfig : EntidadeConfig<JornadaTrabalhoHorario>, IEntityTypeConfiguration<JornadaTrabalhoHorario>
{
    public override void Configure(EntityTypeBuilder<JornadaTrabalhoHorario> builder)
    {
        base.Configure(builder);

        builder.ToTable("JornadaTrabalhoHorario");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.DiaSemana)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(c => c.HoraEntrada)
            .IsRequired();

        builder.Property(c => c.HoraSaida)
            .IsRequired();

        builder.Property(c => c.DuracaoIntervalo)
            .IsRequired();
    }
}