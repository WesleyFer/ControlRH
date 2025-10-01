using ControlRH.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace ControlRH.Areas.Admin.Models.ViewModels;

public class GrupoViewModel
{
    public Guid? Id { get; set; }

    [Required(ErrorMessage = "Nome é obrigatória.")]
    public string Nome { get; set; }

    public List<Guid> PermissoesSelecionadas { get; set; } = new();

    public List<PermissaoViewModel> PermissoesDisponiveis { get; set; } = new();

    public Grupo ToModel(Grupo? entity = null)
    {
        if (entity is not null)
        {
            entity.AtualizarNome(Nome);

            // Atualiza permissões: remove as antigas e adiciona as novas
            entity.LimparGrupoPermissoes();

            foreach (var permissaoId in PermissoesSelecionadas)
            {
                entity.AdicionarGrupoPermissao(permissaoId);
            }

            return entity;
        }

        var grupo = new Grupo(Nome, GrupoType.Usuario);

        foreach (var permissaoId in PermissoesSelecionadas)
        {
            grupo.AdicionarGrupoPermissao(permissaoId);

        }
        return grupo;
    }

    public void ToViewModel(Grupo entity)
    {
        Id = entity.Id;
        Nome = entity.Nome;
        PermissoesSelecionadas = entity.GruposPermissoes
        .Select(gp => gp.PermissaoId)
        .ToList();
    }
}


public class PermissaoViewModel
{
    public Guid? Id { get; set; }
    public string Chave { get; set; }
    public string Valor { get; set; }
    public string Categoria { get; set; }
}