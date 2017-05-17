using PostOffice.Model.Models;
using PostOfiice.DAta.Infrastructure;

namespace PostOfiice.DAta.Repositories
{
    public interface ITransactionDetailRepository : IRepository<TransactionDetail> { }

    public class TransactionDetailRepository : RepositoryBase<TransactionDetail>, ITransactionDetailRepository
    {
        public TransactionDetailRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}