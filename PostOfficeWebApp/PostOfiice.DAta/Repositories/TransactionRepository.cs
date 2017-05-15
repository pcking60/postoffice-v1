using PostOffice.Model.Models;
using PostOfiice.DAta.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PostOfiice.DAta.Repositories
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        IEnumerable<Transaction> GetAllByTag(string tag, int index, int pageSize, out int total);
    }

    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
    {
        public TransactionRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<Transaction> GetAllByTag(string tag, int index, int pageSize, out int total)
        {
            throw new NotImplementedException();
        }
    }
}