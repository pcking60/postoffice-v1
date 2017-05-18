using PostOffice.Model.Models;
using PostOfiice.DAta.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PostOfiice.DAta.Repositories
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        
    }

    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
    {
        public TransactionRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}