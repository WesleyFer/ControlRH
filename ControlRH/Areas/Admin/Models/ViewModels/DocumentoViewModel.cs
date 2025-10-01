using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ControlRH.Areas.Admin.Models.ViewModels;

public class DocumentoViewModel
{
    public Guid? Id { get; set; }
    public Guid CarteiraClienteId { get; set; }

    [Required(ErrorMessage = "Nome é obrigatório.")]
    public string Nome { get; set; }
    public string? Descricao { get; set; }
    public IFormFile Arquivo { get; set; }
    public Guid? ColaboradorId { get; set; }

    [ValidateNever]
    public SelectList CarteirasClientesSelectList { get; set; }

    [ValidateNever]
    public SelectList ColaboradoresSelectList { get; set; }

    public Documento ToModel(string? caminhoArquivo = null)
    {
        var model = new Documento(Nome, Descricao ?? string.Empty, caminhoArquivo ?? string.Empty, CarteiraClienteId, ColaboradorId);

        return model;
    }

    public void ToViewModel(Documento entity)
    {
        Id = entity.Id;
        Nome = entity.Nome;
        Descricao = entity.Descricao;
        CarteiraClienteId = entity.CarteiraClienteId;
        ColaboradorId = entity.ColaboradorId;
    }

    public void CarteirasClientes(IEnumerable<CarteiraCliente> carteirasClientes)
    {
        CarteirasClientesSelectList = new SelectList(carteirasClientes, "Id", "Nome");
    }

    public void Colaboradores(IEnumerable<Colaborador> colaboradores)
    {
        ColaboradoresSelectList = new SelectList(colaboradores, "Id", "Nome");
    }
}
