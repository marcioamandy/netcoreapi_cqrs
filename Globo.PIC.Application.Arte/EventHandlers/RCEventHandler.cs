using System.Threading;
using System.Threading.Tasks;
using Globo.PIC.Domain.Enums;
using Globo.PIC.Domain.Types.Commands;
using Globo.PIC.Domain.Types.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Globo.PIC.Application.Arte.EventHandlers
{

    /// <summary>
    /// 
    /// </summary>
    public class RCEventHandler
        : INotificationHandler<OnStatusRCAlterada>

    {

        /// <summary>
        /// 
        /// </summary>
        private readonly IMediator mediator;

        /// <summary>
		/// 
		/// </summary>
		private readonly ILogger<RCEventHandler> logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_logger"></param>
        /// <param name="_mediator"></param>
        public RCEventHandler(ILogger<RCEventHandler> _logger, IMediator _mediator)
        {
            logger = _logger;
            mediator = _mediator;
        }

        Task INotificationHandler<OnStatusRCAlterada>.Handle(OnStatusRCAlterada notification, CancellationToken cancellationToken)
        {
            //Caso o status da linha da requisição de um pedido de arte tenha sido Aprovada no Oracle
            if (notification.RC.PedidoItem.Pedido.IdTipo == (int)TipoPedido.ARTE &&
                notification.RC.LinhaStatus.ToUpper().Equals("APPROVED"))
            {
                var taskUpdateStatusItem = mediator.Send(new MudarStatusPedidoItemArte()
                {
                    IdPedido = notification.RC.PedidoItem.IdPedido,
                    IdPedidoItens = new long[] { notification.RC.IdPedidoItem },
                    IdStatus = (int)PedidoItemArteStatus.ITEM_APROVADO
                }, cancellationToken);

                taskUpdateStatusItem.Wait();
            }

            return Unit.Task;
        }
    }
}
