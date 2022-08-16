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
    public class AcionamentoRepository : Repository<Acionamento>
    {
		private readonly IUserProvider userProvider;
		public AcionamentoRepository(PicDbContext context, IUserProvider _userProvider) : base(context)
		{
			userProvider = _userProvider;
		}

		public override IQueryable<Acionamento> GetAll()
		{
			var acionamento = _db.Acionamento;

			SetAllIncludes(acionamento);

			acionamento.OrderBy(o => o.Id);

			return acionamento;
		}

		public override ValueTask<Acionamento> GetById(long id, CancellationToken cancellationToken)
		{
			var acionamento = _db.Acionamento
				.Include(c => c.PedidoVeiculo)
				.Include(a => a.AcionamentoPedidoItens).ThenInclude(a => a.Arquivos)
				.Where(a => a.Id == id).FirstOrDefault();
			var retorno = new ValueTask<Acionamento>(acionamento);
			return retorno;
		}

		public override void AddOrUpdate(Acionamento obj, CancellationToken cancellationToken)
		{
			var acionamentoFromDb = _db.Acionamento.Find(obj.Id);

			if (acionamentoFromDb != null)
			{

				var typeObj = acionamentoFromDb.GetType();

				foreach (PropertyInfo propertyInfo in typeObj.GetProperties())
				{
					if (propertyInfo.CanRead)
					{
						object firstValue = propertyInfo.GetValue(acionamentoFromDb, null);
						object secondValue = propertyInfo.GetValue(obj, null);
						if (!Equals(firstValue, secondValue))
						{
							propertyInfo.SetValue(acionamentoFromDb, secondValue);
						}
					}
				}

				obj = acionamentoFromDb;
				Update(obj);
			}
			else
			{

				Add(obj, cancellationToken);
			}

		}

		public override void DeletarAcionamento(Acionamento obj, CancellationToken cancellationToken)
		{
			Acionamento objAcionamento = new Acionamento();
			objAcionamento.Id = obj.Id;
			var acionamentoFromDb = _db.Acionamento.Find(objAcionamento.Id);

            PedidoVeiculo objPedido = new PedidoVeiculo();
			objPedido.Id = obj.IdPedido;
            var pedidoVeiculoFromDb = _db.PedidoVeiculo.Find(objPedido.Id);

            if (acionamentoFromDb != null)
			{
				objAcionamento = acionamentoFromDb;
				objAcionamento.DataCancelamento = DateTime.Now;
				objAcionamento.JustificativaCancelamento = obj.JustificativaCancelamento;
				objAcionamento.OutraJustificativa = obj.OutraJustificativa;

				Update(objAcionamento);

				if (pedidoVeiculoFromDb != null)
                {
					objPedido = pedidoVeiculoFromDb;
					objPedido.IdStatus = 5;

					_db.PedidoVeiculo.Update(objPedido);

				}
			}
		}


		void SetAllIncludes(DbSet<Acionamento> dbAcionamento)
		{
			dbAcionamento
				.Include(c => c.PedidoVeiculo)
				.Include(a => a.AcionamentoPedidoItens).ThenInclude(a => a.Arquivos);
		}
	}
}
