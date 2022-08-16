using Globo.PIC.Infra.Data.Context;
using System.Reflection;
using System.Threading;
using Globo.PIC.Domain.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Globo.PIC.Infra.Data.Repositories
{
    public class RoleRepository : Repository<UserRole>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public RoleRepository(PicDbContext context) : base(context) { }

        public override ValueTask<List<UserRole>> ListByLogin(string login, CancellationToken cancellationToken)
        {
            var userRoles = _db.UserRole
                .Include(a=>a.User)
                .Where(a => a.Login == login).ToList();

            var retorno = new ValueTask<List<UserRole>>(userRoles);
            return retorno;
        }

        public override Task<UserRole> GetByRoleName(string login, string roleName, CancellationToken cancellationToken)
        {
            var userRole = _db.UserRole
                .Include(a => a.User)
                .Where(a => a.Login == login && a.Name==roleName).FirstOrDefault();
            var retorno = new ValueTask<UserRole>(userRole);
            return retorno.AsTask();
        }

        public override IQueryable<UserRole> GetAll()
        {
            return _db.UserRole;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="cancellationToken"></param>
        public override void AddOrUpdate(UserRole obj, CancellationToken cancellationToken)
        {
            var roleUserTask = _db.UserRole.Find(new object[] { obj.Name, obj.Login });
            if (roleUserTask != null)
            {
                var typeObj = roleUserTask.GetType();

                foreach (PropertyInfo propertyInfo in typeObj.GetProperties())
                {
                    if (propertyInfo.CanRead)
                    {
                        object firstValue = propertyInfo.GetValue(roleUserTask, null);
                        object secondValue = propertyInfo.GetValue(obj, null);
                        if (!Equals(firstValue, secondValue))
                        {
                            propertyInfo.SetValue(roleUserTask, secondValue);
                        }
                    }
                }

                obj = roleUserTask;
                Update(obj);
            }
            else
            {
                Add(obj, cancellationToken);
            }
        }
    }
}
