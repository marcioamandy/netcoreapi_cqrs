using Globo.PIC.Domain.Interfaces;
using Globo.PIC.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Globo.PIC.Infra.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly PicDbContext _context;
        private readonly IDbContextFactory<PicDbContext> _contextFactory;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public UnitOfWork(PicDbContext context, IDbContextFactory<PicDbContext> contextFactory)
        {
            _context = context;
            _contextFactory = contextFactory;
        }
        
        public IPicDbContext GetContextFactory()
        {
            return (IPicDbContext)_contextFactory.CreateDbContext();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Commit()
        {
            return SaveChanges();
        }
        public bool SaveChanges()
        {
            return _context.SaveChanges() > 0;
        }
        public void BeginTransaction()
        {
             _context.Database.BeginTransaction();
        }

        public bool SaveAndCommitTransaction()
        {
            if(_context.SaveChanges() > 0)
                _context.Database.CommitTransaction();

            return _context.SaveChanges() > 0;
        }
        public void CommitTransaction()
        {
          _context.Database.CommitTransaction();
        }
        public void RollbackTransaction()
        {
            _context.Database.RollbackTransaction();
        }


        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
