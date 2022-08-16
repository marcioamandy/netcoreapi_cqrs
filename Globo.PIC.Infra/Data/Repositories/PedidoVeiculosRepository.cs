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

	public class PedidoVeiculoRepository : Repository<PedidoVeiculo>
	{
		private readonly IUserProvider userProvider;
		public PedidoVeiculoRepository(PicDbContext context, IUserProvider _userProvider) : base(context)
		{
			userProvider = _userProvider;
		}

		public override IQueryable<PedidoVeiculo> GetAll()
		{
			var pedidoVeiculo = _db.PedidoVeiculo;

			SetAllIncludes(pedidoVeiculo);

			pedidoVeiculo.OrderBy(o => o.Id);

			return pedidoVeiculo;
		}

		public override ValueTask<PedidoVeiculo> GetById(long id, CancellationToken cancellationToken)
		{
			var pedidoVeiculos = _db.PedidoVeiculo
				.Include(c => c.Pedido).ThenInclude(t => t.Arquivos)
				.Include(a => a.Status)
				.Where(a => a.Id == id).FirstOrDefault();
			var retorno = new ValueTask<PedidoVeiculo>(pedidoVeiculos);
			return retorno;
		}

		public override void AddOrUpdate(PedidoVeiculo obj, CancellationToken cancellationToken)
		{
			var pedidoFromDb = _db.PedidoVeiculo.Find(obj.Id);

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
				obj.Pedido.IdTipo = (int)TipoPedido.VEICULOCENA;
				obj.Pedido.DataCriacao = DateTime.Now;
				obj.Pedido.CriadoPorLogin = userProvider.User.Login;

				Add(obj, cancellationToken);
			}

		}

		public override void DeletarPedido(long id, CancellationToken cancellationToken)
		{
			PedidoVeiculo obj = new PedidoVeiculo();
			obj.Id = id;
			var pedidoFromDb = _db.PedidoVeiculo.Find(obj.Id);

			if (pedidoFromDb != null)
			{
				obj = pedidoFromDb;
				Remove(obj);
			}
		}


		void SetAllIncludes(DbSet<PedidoVeiculo> dbPedidoVeiculo)
		{
			dbPedidoVeiculo
				.Include(c => c.Pedido).ThenInclude(t => t.Arquivos)
				.Include(a => a.Status);
		}

	}
}
