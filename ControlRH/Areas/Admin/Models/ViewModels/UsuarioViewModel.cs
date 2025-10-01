using ControlRH.Core.Attributes;
using ControlRH.Core.Enums;
using ControlRH.Core.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ControlRH.Areas.Admin.Models.ViewModels;

public class UsuarioViewModel
{
    public Guid? Id { get; set; }

    [Required(ErrorMessage = "Login (CPF) é obrigatório.")]
    [Cpf(ErrorMessage = "CPF inválido.")]
    public string Cpf { get; set; }

    [Required(ErrorMessage = "Senha é obrigatória.")]
    public string Senha { get; set; }

    public Guid GrupoId { get; set; }

    [ValidateNever]
    public List<SelectListItem> Grupos { get; set; }

    public Guid ColaboradorId { get; set; }

    [ValidateNever]
    public List<ColaboradorSelectItem> Colaboradores { get; set; } = new();

    public string? CpfExibicao { get; set; }

    public string? NomeColaborador { get; set; }

    public Usuario ToModel(Usuario? entity = null)
    {
        if (entity is not null)
        {
            entity.AlterarSenha(Senha);



            return entity;
        }

        var usuario = new Usuario(Cpf, ColaboradorId, Senha);
        
        usuario.AdicionarUsuarioGrupo(GrupoId);
        
        return usuario;
    }

    public void ToViewModel(Usuario entity)
    {
        Id = entity.Id;
        ColaboradorId = entity.ColaboradorId; // já deve estar preenchido
        Cpf = entity.Login;
        GrupoId = entity.UsuariosGrupos.FirstOrDefault()?.GrupoId ?? Guid.Empty;

        CpfExibicao = Utils.MascararCpfFormatado(entity.Login);
        NomeColaborador = entity.Colaborador.Nome ?? string.Empty;
    }

    public void SetGrupos(IEnumerable<GrupoViewModel> grupos)
    {
        Grupos = grupos
            .Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Nome
            })
            .ToList();
    }

    public void SetColaboradores(IEnumerable<ColaboradorViewModel> colaboradores)
    {
        Colaboradores = colaboradores
            .Select(c => new ColaboradorSelectItem
            {
                Id = c.Id,
                Nome = c.Nome,
                Cpf = c.Cpf
            })
            .ToList();
    }
}


public class ColaboradorSelectItem
{
    public Guid? Id { get; set; }
    public string Nome { get; set; }
    public string Cpf { get; set; }
}