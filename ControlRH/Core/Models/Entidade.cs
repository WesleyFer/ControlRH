using Flunt.Notifications;

namespace ControlRH.Core.Models
{
    public abstract class Entidade : Notifiable<Notification>
    {
        protected Entidade(Guid? id = null)
        {
            Id = id.HasValue && id.Value != Guid.Empty ? id.Value : Guid.NewGuid();
        }
        public Guid Id { get; private set; }

        public DateTime DataCriacao { get; private set; }

        public DateTime DataAlteracao { get; private set; }
    }
}
