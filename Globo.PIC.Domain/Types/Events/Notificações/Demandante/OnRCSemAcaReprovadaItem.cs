﻿using Globo.PIC.Domain.Entities;
using MediatR;

namespace Globo.PIC.Domain.Types.Events.Notificações
{
    public class OnRCSemAcaReprovadaItem : INotification
	{

		/// <summary>
		/// 
		/// </summary>
		public Pedido Pedido { get; set; }
	}
}
