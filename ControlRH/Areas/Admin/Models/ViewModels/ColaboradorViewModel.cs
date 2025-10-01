using ControlRH.Core.Attributes;
using ControlRH.Core.Contracts;
using ControlRH.Core.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ControlRH.Areas.Admin.Models.ViewModels;

public class ColaboradorViewModel
{
    public Guid? Id { get; set; }

    [Required(ErrorMessage = "CPF é obrigatório.")]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "CPF inválido.")]
    [Cpf(ErrorMessage = "CPF inválido.")]
    public string Cpf { get; set; }

    [Required(ErrorMessage = "PIS é obrigatório.")]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "PIS inválido.")]
    public string Pis { get; set; }

    [Required(ErrorMessage = "Matrícula é obrigatória.")]
    [RegularExpression(@"^\d{5}$", ErrorMessage = "Matrícula deve ter 5 dígitos.")]
    public string Matricula { get; set; }

    [Required(ErrorMessage = "Nome é obrigatório.")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "Cargo é obrigatório.")]
    public Guid CargoId { get; set; }

    [ValidateNever]
    public SelectList CargosSelectList { get; set; }

    [Required(ErrorMessage = "Carteira de Cliente é obrigatório.")]
    [Display(Name = "Carteira")]
    public Guid CarteiraClienteId { get; set; }

    [ValidateNever]
    public SelectList CarteirasClientesSelectList { get; set; }

    public Guid JornadaTrabalhoId { get; set; }

    [ValidateNever]
    public SelectList JornadasTrabalhosSelectList { get; set; }

    public Colaborador ToModel(Colaborador? entity = null)
    {
        if (entity is not null)
        {
            entity.AtualizarCpf(Cpf);
            entity.AtualizarPis(Pis);
            entity.AtualizarMatricula(Matricula);
            entity.AtualizarNome(Nome);
            entity.AtualizarCargo(CargoId);
            entity.AtualizarCarteiraCliente(CarteiraClienteId);

            return entity;
        }

        var colaborador = new Colaborador(Cpf, Pis, Matricula, Nome, CargoId, CarteiraClienteId);

        if (JornadaTrabalhoId != Guid.Empty)
            colaborador.AdicionarJornadaTrabalho(JornadaTrabalhoId);

        return colaborador;
    }

    public void ToViewModel(Colaborador entity)
    {
        Id = entity.Id;
        Cpf = Utils.FormatarCpf(entity.Cpf);
        Pis = Utils.FormatarPis(entity.Pis);
        Matricula = Utils.FormatarMatricula(entity.Matricula);
        Nome = entity.Nome;
        CargoId = entity.CargoId;
        CarteiraClienteId = entity.CarteiraClienteId;
        JornadaTrabalhoId = Guid.Empty; //entity.ColaboradoresJornadas.LastOrDefault().JornadaTrabalhoId;
    }

    public void CarteirasClientes(IEnumerable<CarteiraCliente> carteirasClientes)
    {
        CarteirasClientesSelectList = new SelectList(carteirasClientes, "Id", "Nome");
    }

    public void JornadasTrabalhos(IEnumerable<JornadaTrabalho> jornadasTrabalhos)
    {
        JornadasTrabalhosSelectList = new SelectList(jornadasTrabalhos, "Id", "Nome");
    }

    public void Cargos(IEnumerable<Cargo> cargos)
    {
        CargosSelectList = new SelectList(cargos, "Id", "Nome");
    }
}
