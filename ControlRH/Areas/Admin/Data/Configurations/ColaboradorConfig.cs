﻿using ControlRH.Areas.Admin.Models;
using ControlRH.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlRH.Areas.Admin.Data.Configurations;


internal class ColaboradorConfig : AggregateRootConfig<Colaborador>, IEntityTypeConfiguration<Colaborador>
{
    public override void Configure(EntityTypeBuilder<Colaborador> builder)
    {
        builder.ToTable("Colaborador");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Cpf)
           .HasMaxLength(14)
           .IsRequired();

        builder.Property(c => c.Pis)
            .HasMaxLength(11)
            .IsRequired();

        builder.Property(c => c.Matricula)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(c => c.Nome)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(c => c.Cargo)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasOne(c => c.CarteiraCliente)
            .WithMany(c => c.Colaboradores)
            .HasForeignKey(c => c.CarteiraClienteId)
            .IsRequired();

        builder.HasMany(c => c.ColaboradoresJornadas)
            .WithOne(c => c.Colaborador)
            .HasForeignKey(c => c.ColaboradorId)
            .IsRequired();
    }
}