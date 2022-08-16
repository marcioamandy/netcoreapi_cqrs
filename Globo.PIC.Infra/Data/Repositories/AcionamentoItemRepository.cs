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
    public class AcionamentoItemRepository : Repository<AcionamentoItem>
    {
		private readonly IUserProvider userProvider;
		public AcionamentoItemRepository(PicDbContext context, IUserProvider _userProvider) : base(context)
		{
			userProvider = _userProvider;
		}

		public override IQueryable<AcionamentoItem> GetAll()
		{
			var acionamentoItem = _db.AcionamentoItem;

			SetAllIncludes(acionamentoItem);

			acionamentoItem.OrderBy(o => o.Id);

			return acionamentoItem;
		}

		public override ValueTask<AcionamentoItem> GetById(long id, CancellationToken cancellationToken)
		{
			var acionamentoItem = _db.AcionamentoItem
				.Include(c => c.Arquivos)
				.Include(c => c.PedidoItem)
				.Where(a => a.Id == id).FirstOrDefault();
			var retorno = new ValueTask<AcionamentoItem>(acionamentoItem);
			return retorno;
		}

		public override void AddOrUpdate(AcionamentoItem obj, CancellationToken cancellationToken)
		{
			var acionamentoItemFromDb = _db.AcionamentoItem.Find(obj.Id);

			if (acionamentoItemFromDb != null)
			{

				var typeObj = acionamentoItemFromDb.GetType();

				foreach (PropertyInfo propertyInfo in typeObj.GetProperties())
				{
					if (propertyInfo.CanRead)
					{
						object firstValue = propertyInfo.GetValue(acionamentoItemFromDb, null);
						object secondValue = propertyInfo.GetValue(obj, null);
						if (!Equals(firstValue, secondValue))
						{
							propertyInfo.SetValue(acionamentoItemFromDb, secondValue);
						}
					}
				}

				obj = acionamentoItemFromDb;
				Update(obj);
			}
			else
			{

				Add(obj, cancellationToken);
			}

		}

		public override void DeletarAcionamentoItem(AcionamentoItem obj, CancellationToken cancellationToken)
		{
			AcionamentoItem objAcionamentoItem = new AcionamentoItem();
			objAcionamentoItem.Id = obj.Id;
			var acionamentoFromDb = _db.AcionamentoItem.Find(objAcionamentoItem.Id);

			PedidoItem objPedidoItem = new PedidoItem();
			objPedidoItem.Id = obj.IdPedidoItem;
			var pedidoItemFromDb = _db.PedidoItem.Find(objPedidoItem.Id);

			if (acionamentoFromDb != null)
			{
				objAcionamentoItem = acionamentoFromDb;
				objAcionamentoItem.DataCancelamento = DateTime.Now;
				objAcionamentoItem.JustificativaCancelamento = obj.JustificativaCancelamento;
				objAcionamentoItem.OutraJustificativa = obj.OutraJustificativa;

				Update(objAcionamentoItem);

				if (pedidoItemFromDb != null)
                {
					objPedidoItem = pedidoItemFromDb;
					objPedidoItem.DataCancelamento = DateTime.Now;

					_db.PedidoItem.Update(objPedidoItem);
				}
			}
		}

		void SetAllIncludes(DbSet<AcionamentoItem> dbAcionamentoItem)
		{
			dbAcionamentoItem
				.Include(c => c.Arquivos);
		}
	}
}
