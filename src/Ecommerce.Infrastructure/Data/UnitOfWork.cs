using Ecommerce.Application.Interfaces;

namespace Ecommerce.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DBConnection _db;

        public UnitOfWork(DBConnection db)
        {
            _db = db;
        }

        public void BeginTransaction()
        {
            _db.BeginTransaction();
        }

        public void Commit()
        {
            _db.Commit();
        }

        public void Rollback()
        {
            _db.Rollback();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
