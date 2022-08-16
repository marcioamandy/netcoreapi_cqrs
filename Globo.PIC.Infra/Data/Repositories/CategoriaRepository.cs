using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Infra.Data.Context;

namespace Globo.PIC.Infra.Data.Repositories
{
    public class CategoriaRepository : Repository<Categoria>
    {
        public CategoriaRepository(PicDbContext context) : base(context) { }

		public override ValueTask<Categoria> GetById(long id, CancellationToken cancellationToken)
		{
			return new ValueTask<Categoria>(_db.Categoria
				.Include(c => c.CategoriaPai)
				.Include(c => c.Categorias)
				.FirstOrDefaultAsync(p => p.Id.Equals(id)));
		}

		public override ValueTask<Categoria> GetByIdTipoVeiculos(long id, CancellationToken cancellationToken)
		{
			return new ValueTask<Categoria>(_db.Categoria
				.Include(c => c.CategoriaPai)
				.Include(c => c.Categorias)
				.FirstOrDefaultAsync(p => p.Id.Equals(id)));
		}

		public override Task<List<Categoria>> ListSubCategoriaIdByIdTipoVeiculo(long idCategoriaVeiculo, CancellationToken cancellationToken)
		{

            var categoria = _db.Categoria
                .Include(c => c.CategoriaPai)
				.Include(c => c.Categorias)
				.Where(a => a.IdCategoria == idCategoriaVeiculo).ToListAsync(cancellationToken);

            return categoria;
        }

		public override Task<List<Categoria>> ListCategoria(CancellationToken cancellationToken)
		{
			var categoria = _db.Categoria
				.Include(c => c.CategoriaPai)
				.Include(c => c.Categorias).ToList();
				//.Where(a => string.IsNullOrWhiteSpace(a.CategoriaPai.ToString())).ToList();

			var retorno = Task.FromResult(categoria);
			return retorno;
		}

		public override IQueryable<Categoria> GetAll()
		{
			return this._db.Categoria
				.Include(c => c.CategoriaPai)
				.Include(c => c.Categorias)
				.OrderBy(o => o.Id);
		}
	}
}
