using ControlRH.Core.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ControlRH.Areas.Colaborador.Models.ViewModels;

public class PontoEletronicoViewModel
{
    public Guid? Id { get; set; }

    [Required(ErrorMessage = "Data e hora é obrigatória.")]
    public DateTime DataHora { get; set; }

    public DateTime? UltimaMarcacao { get; set; }

    public MarcacaoType TipoMarcacao { get; set; }

    [ValidateNever]
    public SelectList TiposMarcacoesSelectList { get; set; }

    public PontoEletronico ToModel(string cpf, string pis)
    {
        var pontoEletronico = new PontoEletronico(DataHora, TipoMarcacao);

        pontoEletronico.AdicionarCpf(cpf);
        pontoEletronico.AdicionarPis(pis);

        return pontoEletronico;
    }

    public void ToViewModel(PontoEletronico entity)
    {
        Id = entity.Id;
        DataHora = entity.DataHora;
    }

    public void TiposMarcacoes()
    {
        var tiposLeiautes = Enum.GetValues(typeof(MarcacaoType))
            .Cast<MarcacaoType>()
            .Select(e => new SelectListItem
            {
                Value = ((int)e).ToString(),
                Text = e.GetType()
                        .GetField(e.ToString())?
                        .GetCustomAttribute<DescriptionAttribute>()?.Description
                        ?? e.ToString()

            })
            .ToList();

        TiposMarcacoesSelectList = new SelectList(tiposLeiautes, "Value", "Text");
    }
}

public class TabelaPontoEletronicoViewModel
{
    public Dictionary<string, string> Columns { get; set; } = new();
    public IEnumerable<RegistroPontoViewModel> Data { get; set; } = Enumerable.Empty<RegistroPontoViewModel>();
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int TotalItems { get; set; }
    public DateTime? DePeriodo { get; set; }
    public DateTime? AtePeriodo { get; set; }
}

public class RegistroPontoViewModel
{
    public DateTime Dia { get; set; }
    public string DiaSemana { get; set; } = string.Empty;
    public string Entrada { get; set; } = "-:-";
    public string SaidaIntervalo { get; set; } = "-:-";
    public string RetornoIntervalo { get; set; } = "-:-";
    public string Saida { get; set; } = "-:-";

    // Novos campos
    public string HorasTrabalhadas { get; set; } = "-:-";
    public string HorasJustificadas { get; set; } = "-:-";
    public string HorasEmAtraso { get; set; } = "-:-";
}

public class DocumentoPontoViewModel
{
    public Guid? Id { get; set; }

    public string Nome { get; set; }

}

public class DocumentoDownloadViewModel
{
    public byte[] Conteudo { get; set; } = Array.Empty<byte>();
    public string Nome { get; set; } = string.Empty;
}

public class TabelaDocumentoViewModel
{
    public Dictionary<string, string> Columns { get; set; } = new();
    public IEnumerable<DocumentoPontoViewModel> Data { get; set; } = Enumerable.Empty<DocumentoPontoViewModel>();
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int TotalItems { get; set; }
}