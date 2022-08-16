using Globo.PIC.Domain.Entities;
using MediatR;

namespace Globo.PIC.Domain.Types.Events
{
    public class OnUsuarioAlterado : INotification
    {
        public Usuario Usuario { get; set; }
    }
}
