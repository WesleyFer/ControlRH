using ControlRH.Core.Contracts;
using System.ComponentModel.DataAnnotations;

namespace ControlRH.Areas.Admin.Models.ViewModels;

public class CarteiraClienteViewModel
{
    public Guid? Id { get; set; }

    [Required(ErrorMessage = "Informe o nome da carteira.")]
    [Display(Name = "Nome da carteira")]
    public string Nome { get; set; }

    public CarteiraCliente ToModel()
    {
        var model = new CarteiraCliente(Nome);

        return model;
    }

    public void ToViewModel(CarteiraCliente entity)
    {
        Id = entity.Id;
        Nome = entity.Nome;
    }
}
