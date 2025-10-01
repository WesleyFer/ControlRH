using System.ComponentModel.DataAnnotations;

namespace ControlRH.Areas.Admin.Models.ViewModels;

public class CargoViewModel
{
    public Guid? Id { get; set; }

    [Required(ErrorMessage = "Informe o nome do cargo.")]
    [Display(Name = "Nome do cargo")]
    public string Nome { get; set; }

    public Cargo ToModel()
    {
        var model = new Cargo(Nome);

        return model;
    }

    public void ToViewModel(Cargo entity)
    {
        Id = entity.Id;
        Nome = entity.Nome;
    }
}
