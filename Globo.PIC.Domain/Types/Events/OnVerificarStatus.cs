using Globo.PIC.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Globo.PIC.Domain.Types.Events
{

	/// <summary>
	/// 
	/// </summary>
	public class OnVerificarStatus : INotification
	{

		/// <summary>
		/// 
		/// </summary>
		public Pedido Pedido { get; set; }

	}
}
