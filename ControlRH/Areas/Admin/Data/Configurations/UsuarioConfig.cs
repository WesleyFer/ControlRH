using ControlRH.Areas.Admin.Models;
using ControlRH.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlRH.Areas.Admin.Data.Configurations;

internal class UsuarioConfig : AggregateRootConfig<Usuario>, IEntityTypeConfiguration<Usuario>
{
    public override void Configure(EntityTypeBuilder<Usuario> builder)
    {
        base.Configure(builder);

        builder.ToTable("Usuario");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Login)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.SenhaHash)
            .HasMaxLength(256)
            .IsRequired();

        builder.HasOne(c => c.Colaborador)
            .WithOne()
            .HasForeignKey<Usuario>(c => c.ColaboradorId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasMany(c => c.UsuariosGrupos)
            .WithOne(c => c.Usuario)
            .HasForeignKey(c => c.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}