using Globo.PIC.Domain.Entities;
using MediatR;

namespace Globo.PIC.Domain.Types.Events
{

	/// <summary>
	/// 
	/// </summary>
	public class OnCompradorAlterado : INotification
	{

		/// <summary>
		/// 
		/// </summary>
		public Pedido Pedido { get; set; }

		/// <summary>
        /// 
        /// </summary>
		public string CompradoPorLogin { get; set; }

	}
}
