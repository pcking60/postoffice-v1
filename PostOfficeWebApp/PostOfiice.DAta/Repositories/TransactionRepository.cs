using PostOffice.Model.Models;
using PostOfiice.DAta.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PostOfiice.DAta.Repositories
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        IEnumerable<Transaction> GetAllByUserName(string userName);
        
    }

    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
    {
        public TransactionRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public override Transaction Add(Transaction entity)
        {
            entity.CreatedDate = DateTime.Now;
            return base.Add(entity);
        }

        public IEnumerable<Transaction> GetAllByUserName(string userName)
        {
            var pos = from po in this.DbContext.PostOffices
                        join u in this.DbContext.Users
                        on po.ID equals u.POID
                        where u.UserName == userName
                        select po.ID;

            int p = pos.First();
            var listTransaction = from u in this.DbContext.Users
                             join ts in this.DbContext.Transactions
                             on u.Id equals ts.UserId
                             where u.POID == p
                             select ts;

            return listTransaction;
        }

        
        public override void Update(Transaction entity)
        {
            entity.UpdatedDate = DateTime.Now;
            base.Update(entity);
        }
    }
}