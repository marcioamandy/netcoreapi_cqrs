using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Infra.Data.Context;

namespace Globo.PIC.Infra.Data.Repositories
{
    public class ViewerRepository : Repository<Viewer>
    {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		public ViewerRepository(PicDbContext context) : base(context) { }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="login"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public override ValueTask<Viewer> GetById(long id, CancellationToken cancellationToken)
		{
			return new ValueTask<Viewer>(_db.Viewer
				.FirstOrDefaultAsync(u => u.Id.Equals(id), cancellationToken));
		}

		public override IQueryable<Viewer> GetAll()
		{
			return _db.Viewer;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="cancellationToken"></param>
		public override void AddOrUpdate(Viewer obj, CancellationToken cancellationToken)
		{
			var objFromDb = _db.Viewer.Find(obj.Id);

			if (objFromDb != null)
			{
				var typeObj = objFromDb.GetType();

				foreach (PropertyInfo propertyInfo in typeObj.GetProperties())
				{
					if (propertyInfo.CanRead)
					{
						object firstValue = propertyInfo.GetValue(objFromDb, null);
						object secondValue = propertyInfo.GetValue(obj, null);
						if (!Equals(firstValue, secondValue))
						{
							propertyInfo.SetValue(objFromDb, secondValue);
						}
					}
				}

				obj = objFromDb;
				
				Update(obj);
			}
			else
			{
				Add(obj, cancellationToken);
			}
		}
	}
}
