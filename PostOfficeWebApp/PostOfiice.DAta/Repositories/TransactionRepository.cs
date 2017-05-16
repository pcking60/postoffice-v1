using PostOffice.Model.Models;
using PostOfiice.DAta.Infrastructure;
using System;

namespace PostOfiice.DAta.Repositories
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
    }

    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
    {
        public TransactionRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }

        public override Transaction Add(Transaction entity)
        {
            entity.CreatedDate = DateTime.Now;
            return base.Add(entity);
        }

        public override void Update(Transaction entity)
        {
            entity.UpdatedDate = DateTime.Now;
            base.Update(entity);
        }
    }
}