using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Infra.Data.Context;
using System;

namespace Globo.PIC.Infra.Data.Repositories
{
    public class NotificationRepository : Repository<Notificacao>
    {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		public NotificationRepository(PicDbContext context) : base(context) { }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="login"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public override ValueTask<Notificacao> GetById(long id, CancellationToken cancellationToken)
		{
			return new ValueTask<Notificacao>(_db.Notification
                .Include(a => a.Assigns)
                .Include(r => r.Readers)
                .FirstOrDefaultAsync(u => u.Id.Equals(id), cancellationToken));
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="cancellationToken"></param>
        public override void AddOrUpdate(Notificacao obj, CancellationToken cancellationToken)
        {            
            var notificationFromDb = _db.Notification.Find(obj.Id);

            if (notificationFromDb != null)
            {

                var typeObj = notificationFromDb.GetType();

                obj.CreatedAt = notificationFromDb.CreatedAt;

                foreach (PropertyInfo propertyInfo in typeObj.GetProperties())
                {
                    if (propertyInfo.CanRead)
                    {
                        object firstValue = propertyInfo.GetValue(notificationFromDb, null);
                        object secondValue = propertyInfo.GetValue(obj, null);
                        if (!object.Equals(firstValue, secondValue))
                        {
                            propertyInfo.SetValue(notificationFromDb, secondValue);
                        }
                    }
                }

                obj = notificationFromDb;
                this.Update(obj);
            }
            else
            {
                obj.CreatedAt = DateTime.Now;

                this.Add(obj, cancellationToken);
            }
        }

        public override IQueryable<Notificacao> GetAll()
        {
            return this._db.Notification
                .Include(a => a.Assigns)
                .Include(r => r.Readers);
        }
    }
}
