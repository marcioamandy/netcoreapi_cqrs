using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Infra.Data.Context;

namespace Globo.PIC.Infra.Data.Repositories

{
	/// <summary>
	/// 
	/// </summary>
	public class UserRepository : Repository<Usuario>
	{

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		public UserRepository(PicDbContext context) : base(context) { }

		/// <summary>
        /// 
        /// </summary>
        /// <param name="dbUsuario"></param>
        /// <returns></returns>
		IQueryable<Usuario> SetAllIncludePedido(IQueryable<Usuario> dbUsuario)
		{
			return dbUsuario
				.Include(u => u.Roles)
				.Include(u1 => u1.UsuariosConteudos).ThenInclude(x => x.Conteudo)
				.Include(u2 => u2.UnidadeNegocio)
				.Include(u3 => u3.Departamento);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="login"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public override Task<Usuario> GetByLogin(string login, CancellationToken cancellationToken)
		{
			return SetAllIncludePedido(_db.Usuario)
				.FirstOrDefaultAsync(u => u.Login.ToLower().Equals(login.ToLower()), cancellationToken);	
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override IQueryable<Usuario> GetAll()
		{
			return SetAllIncludePedido(_db.Usuario);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="cancellationToken"></param>
		public override void AddOrUpdate(Usuario obj, CancellationToken cancellationToken)
		{
			var userFromDb = _db.Usuario.Find(new[] { obj.Login });

			if (userFromDb != null)
			{
				var typeObj = userFromDb.GetType();

				foreach (PropertyInfo propertyInfo in typeObj.GetProperties())
				{
					if (propertyInfo.CanRead)
					{
						object firstValue = propertyInfo.GetValue(userFromDb, null);
						object secondValue = propertyInfo.GetValue(obj, null);
						if (!Equals(firstValue, secondValue))
						{
							propertyInfo.SetValue(userFromDb, secondValue);
						}
					}
				}

				obj = userFromDb;

				Update(obj);
			}
			else
			{
				obj.IsActive = obj.IsActive;

				Add(obj, cancellationToken);
			}
		}
	}
}