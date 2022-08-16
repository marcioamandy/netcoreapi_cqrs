using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Enums;
using Globo.PIC.Infra.Data.Context;
using Globo.PIC.Domain.Interfaces;

namespace Globo.PIC.Infra.Data.Repositories
{

	public class PedidoArteRepository : Repository<PedidoArte>
	{
		private readonly IUserProvider userProvider;
		public PedidoArteRepository(PicDbContext context, IUserProvider _userProvider) : base(context)
		{
			userProvider = _userProvider;
		}

		public override ValueTask<PedidoArte> GetById(long id, CancellationToken cancellationToken)
		{
			var pedidoArte = _db.PedidoArte;

			SetAllIncludes(pedidoArte);

			return new ValueTask<PedidoArte>(pedidoArte.Find(id));
		}

		public override void AddOrUpdate(PedidoArte obj, CancellationToken cancellationToken)
		{
			var pedidoFromDb = _db.PedidoArte.Find(obj.Id);

			if (pedidoFromDb != null)
			{

				var typeObj = pedidoFromDb.GetType();

				foreach (PropertyInfo propertyInfo in typeObj.GetProperties())
				{
					if (propertyInfo.CanRead)
					{
						object firstValue = propertyInfo.GetValue(pedidoFromDb, null);
						object secondValue = propertyInfo.GetValue(obj, null);
						if (!Equals(firstValue, secondValue))
						{
							propertyInfo.SetValue(pedidoFromDb, secondValue);
						}
					}
				}

				obj = pedidoFromDb;

				Update(obj);
			}
			else
			{
				obj.Pedido.Ativo = true;
				obj.IdStatus = (int)PedidoVeiculoStatus.PEDIDO_RASCUNHO;
				obj.Pedido.IdTipo = (int)TipoPedido.ARTE;
				obj.Pedido.DataCriacao = DateTime.Now;
				obj.Pedido.CriadoPorLogin = userProvider.User.Login;

				Add(obj, cancellationToken);
			}

		}

		public override void DeletarPedido(long id, CancellationToken cancellationToken)
		{
			PedidoArte obj = new PedidoArte();
			obj.Id = id;
			var pedidoFromDb = _db.PedidoArte.Find(obj.Id);

			if (pedidoFromDb != null)
			{
				obj = pedidoFromDb;
				Remove(obj);
			}
		}

		void SetAllIncludes(DbSet<PedidoArte> dbPedidoArte)
		{
			dbPedidoArte
				.Include(x => x.Base)
				.Include(x => x.Pedido)
				.Include(x => x.Status);
		}
	}
}
