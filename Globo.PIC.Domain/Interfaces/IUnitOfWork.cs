using System;

namespace Globo.PIC.Domain.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        bool Commit();
        public bool SaveChanges();
        public void BeginTransaction();
        public void CommitTransaction();
        public void RollbackTransaction();
        public bool SaveAndCommitTransaction();
        public IPicDbContext GetContextFactory();
    }
}