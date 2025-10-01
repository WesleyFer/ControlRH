using ControlRH.Core.Models;

namespace ControlRH.Areas.Admin.Models;

public class CarteiraCliente : AggregateRoot
{
    private readonly List<Colaborador> _colaboradores = new();
    private readonly List<Documento> _documentos = new();

    protected CarteiraCliente() { }

    public CarteiraCliente(string nome)
    {
        Nome = nome;
    }

    public string Nome { get; private set; }

    public IReadOnlyCollection<Colaborador> Colaboradores => _colaboradores.AsReadOnly();

    public IReadOnlyCollection<Documento> Documentos => _documentos.AsReadOnly();

    public void AtualizarNome(string nome)
    {
        if (!string.IsNullOrWhiteSpace(nome) && Nome != nome)
            Nome = nome;
    }

    //public void AdicionarDocumentoCompartilhado(string titulo, string caminhoArquivo)
    //{
    //    var doc = new Documento(titulo, caminhoArquivo, this.Id);
    //    _documentos.Add(doc);
    //}

    //public void AdicionarDocumentoRestrito(string titulo, string caminhoArquivo, Guid colaboradorId)
    //{
    //    var doc = new Documento(titulo, caminhoArquivo, this.Id, colaboradorId);
    //    _documentos.Add(doc);
    //}
}
