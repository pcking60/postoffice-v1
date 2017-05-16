using PostOffice.Model.Models;
using PostOfiice.DAta.Infrastructure;
using System;

namespace PostOfiice.DAta.Repositories
{
    public interface ITransactionDetailRepository : IRepository<TransactionDetail> { }

    public class TransactionDetailRepository : RepositoryBase<TransactionDetail>, ITransactionDetailRepository
    {
        public TransactionDetailRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public override TransactionDetail Add(TransactionDetail entity)
        {
            entity.CreatedDate = DateTime.Now;
            return base.Add(entity);
        }

        public override void Update(TransactionDetail entity)
        {
            entity.UpdatedDate = DateTime.Now;
            base.Update(entity);
        }
    }
}