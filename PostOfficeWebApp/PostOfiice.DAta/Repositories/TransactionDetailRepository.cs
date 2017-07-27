using PostOffice.Model.Models;
using PostOfiice.DAta.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System;

namespace PostOfiice.DAta.Repositories
{
    public interface ITransactionDetailRepository : IRepository<TransactionDetail>
    {
        IEnumerable<TransactionDetail> GetAllByCondition(string condition);

        IEnumerable<TransactionDetail> GetAllByCondition(string condition, int transactionId);

        decimal? GetQuantityByCondition(string condition, int transactionId);
    }

    public class TransactionDetailRepository : RepositoryBase<TransactionDetail>, ITransactionDetailRepository
    {
        public TransactionDetailRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<TransactionDetail> GetAllByCondition(string condition)
        {
            condition = "Sản lượng";
            var query = from ps in DbContext.PropertyServices
                        join td in DbContext.TransactionDetails
                        on ps.ID equals td.PropertyServiceId
                        where ps.Name != condition
                        select td;
            return query;
        }

        public IEnumerable<TransactionDetail> GetAllByCondition(string condition, int transactionId)
        {
            condition = "Sản lượng";
            var query = from ps in DbContext.PropertyServices
                        join td in DbContext.TransactionDetails
                        on ps.ID equals td.PropertyServiceId
                        where ps.Name != condition && td.TransactionId == transactionId
                        select td;
            return query;
        }

        public decimal? GetQuantityByCondition(string condition, int transactionId)
        {          
            var query = from ps in DbContext.PropertyServices
                        join td in DbContext.TransactionDetails
                        on ps.ID equals td.PropertyServiceId
                        where ps.Name == condition && td.TransactionId == transactionId
                        select td;
            int n = query.Count();
            
            if(n>0)
            {
                return query.FirstOrDefault().Money;
            }
            else
            {
                return 0;
            }
        }
    }
}