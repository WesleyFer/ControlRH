using ControlRH.Core.Contracts;
using ControlRH.Core.Enums;
using ControlRH.Core.Models;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace ControlRH.Areas.Admin.Models;

public class PontoEletronico : Entidade, IAggregateRoot
{
    private readonly List<PontoMarcacao> _marcacoes = new();

    protected PontoEletronico() { }

    public PontoEletronico(Guid colaboradorId, DateOnly data)
    {
        ColaboradorId = colaboradorId;
        Data = data;
    }

    public Guid ColaboradorId { get; private set; }

    public Colaborador Colaborador { get; private set; }

    public DateOnly Data { get; private set; }

    public IReadOnlyCollection<PontoMarcacao> Marcacaos => _marcacoes.AsReadOnly();

    public void RegistrarMarcacao(MarcacaoPontoType tipo, TimeOnly horario)
    {
        _marcacoes.Add(new PontoMarcacao(tipo, horario));
    }

}
