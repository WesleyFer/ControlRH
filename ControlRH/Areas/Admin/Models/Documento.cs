using ControlRH.Core.Models;

namespace ControlRH.Areas.Admin.Models;

public class Documento : AggregateRoot
{
    protected Documento() { }

    public Documento(string nome, string descricao, string caminhoArquivo, Guid carteiraClienteId, Guid? colaboradorId = null)
    {
        Nome = nome;
        Descricao = descricao;
        CaminhoArquivo = caminhoArquivo;
        CarteiraClienteId = carteiraClienteId;
        ColaboradorId = colaboradorId;
    }

    public string Nome { get; private set; }
    public string? Descricao { get; private set; }
    public string CaminhoArquivo { get; private set; }
    public Guid CarteiraClienteId { get; private set; }
    public CarteiraCliente CarteiraCliente { get; private set; }
    public Guid? ColaboradorId { get; private set; }
    public Colaborador? Colaborador { get; private set; }
}
