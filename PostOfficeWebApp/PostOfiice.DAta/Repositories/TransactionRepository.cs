using PostOffice.Common.ViewModels;
using PostOffice.Model.Models;
using PostOfiice.DAta.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace PostOfiice.DAta.Repositories
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        IEnumerable<Transaction> GetAllByUserName(string userName);
        IEnumerable<Transaction> GetAllByTime(DateTime fromDate, DateTime toDate);
        IEnumerable<Transaction> GetAllByTimeAndUsername(DateTime fromDate, DateTime toDate, string Username);
        IEnumerable<Transaction> GetAllByTimeAndPOID(DateTime fromDate, DateTime toDate, int id);
        IEnumerable<RevenueStatisticViewModel> GetRevenueStatistic(string fromDate, string toDate);
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

        public IEnumerable<RevenueStatisticViewModel> GetRevenueStatistic(string fromDate, string toDate)
        {
            var parameters = new SqlParameter[]{
                new SqlParameter("@fromDate", fromDate),
                new SqlParameter("@toDate", toDate)
            };
            return DbContext.Database.SqlQuery<RevenueStatisticViewModel>("getRevenueStatistic @fromDate,@toDate", parameters);
        }

        public IEnumerable<Transaction> GetAllByTimeAndUsername(DateTime fromDate, DateTime toDate, string userName)
        {
            var query = from u in DbContext.Users
                        join ts in DbContext.Transactions
                        on u.Id equals ts.UserId
                        where u.UserName==userName && (ts.TransactionDate >= fromDate && ts.TransactionDate <= toDate)
                        select ts;
            return query;
        }

        public IEnumerable<Transaction> GetAllByTime(DateTime fromDate, DateTime toDate)
        {
            var query = from u in DbContext.Users
                        join ts in DbContext.Transactions
                        on u.Id equals ts.UserId
                        where (ts.TransactionDate >= fromDate && ts.TransactionDate <= toDate)
                        select ts;
            return query;
        }

        public IEnumerable<Transaction> GetAllByTimeAndPOID(DateTime fromDate, DateTime toDate, int id)
        {
            var listTransaction = from u in this.DbContext.Users
                                  join ts in this.DbContext.Transactions
                                  on u.Id equals ts.UserId
                                  where u.POID == id
                                  select ts;

            return listTransaction;
        }
    }
}