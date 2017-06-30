using PostOffice.Model.Models;
using PostOfiice.DAta.Infrastructure;

namespace PostOfiice.DAta.Repositories
{
    public interface ITKBDHistoryRepository : IRepository<TKBDHistory> { }

    public class TKBDHistoryRepository : RepositoryBase<TKBDHistory>, ITKBDHistoryRepository
    {
        public TKBDHistoryRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}