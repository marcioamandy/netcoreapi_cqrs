namespace Globo.PIC.Domain.Types.Commands
{
    public class DeletePedidoItemArte: DomainCommand
    {
        /// <summary>
        /// id do pedido item
        /// </summary>
        public long IdPedidoItem { get; set; }

        /// <summary>
        /// id do pedido
        /// </summary>
        public long IdPedido { get; set; }

    }
}
