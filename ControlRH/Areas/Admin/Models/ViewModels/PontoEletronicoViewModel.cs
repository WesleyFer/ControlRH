using System.ComponentModel.DataAnnotations;

namespace ControlRH.Areas.Admin.Models.ViewModels;

public class PontoEletronicoViewModel
{
    public Guid? Id { get; set; }

    [Required(ErrorMessage = "Colaborador é obrigatório.")]
    public Guid ColaboradorId { get; set; }

    [Required(ErrorMessage = "Data é obrigatória.")]
    public DateOnly Data { get; set; }

    public PontoEletronico ToModel(PontoEletronico? entity = null)
    {
        if (entity is not null)
        {
            return entity;
        }

        var pontoEletronico = new PontoEletronico(ColaboradorId, Data);

        return pontoEletronico;
    }

    public void ToViewModel(PontoEletronico entity)
    {
        Id = entity.Id;
    }
}
