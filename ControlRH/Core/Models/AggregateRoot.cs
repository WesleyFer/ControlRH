using ControlRH.Core.Contracts;

namespace ControlRH.Core.Models;

public class AggregateRoot : Entidade, IAggregateRoot
{
    protected AggregateRoot(Guid id = default) : base(id) { }
}