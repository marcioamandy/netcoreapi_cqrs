using Globo.PIC.Domain.Entities;
using MediatR;

namespace Globo.PIC.Domain.Types.Events
{
    public class OnUsuarioCriado : INotification
    {
        public Usuario Usuario { get; set; }
    }
}
