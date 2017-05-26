﻿using PostOffice.Common.ViewModels;
using PostOffice.Model.Models;
using PostOfiice.DAta.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;

namespace PostOfiice.DAta.Repositories
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        IEnumerable<Transaction> GetAllByUserName(string userName);

        IEnumerable<Transaction> GetAll(DateTime fromDate, DateTime toDate);

        IEnumerable<Transaction> GetAll(DateTime fromDate, DateTime toDate, string userId, int serviceId);

        IEnumerable<Transaction> GetAll(DateTime fromDate, DateTime toDate, int serviceId);

        IEnumerable<Transaction> GetAll(DateTime fromDate, DateTime toDate, string userId);

        IEnumerable<Transaction> GetAllByTimeAndPOID(DateTime fromDate, DateTime toDate, int id);

        IEnumerable<Transaction> GetAllByTimeAndPOID(DateTime fromDate, DateTime toDate, int id, string userId, int serviceId);

        IEnumerable<Transaction> GetAllByTimeAndPOID(DateTime fromDate, DateTime toDate, int id, string userId);

        IEnumerable<Transaction> GetAllByTimeAndPOID(DateTime fromDate, DateTime toDate, int id, int serviceId);

        IEnumerable<Transaction> GetAllByTimeAndUsername(DateTime fromDate, DateTime toDate, string Username, int serviceId);

        IEnumerable<Transaction> GetAllByTimeAndUsername(DateTime fromDate, DateTime toDate, string Username);

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
                        where u.UserName == userName && (DbFunctions.TruncateTime(ts.TransactionDate) >= fromDate && DbFunctions.TruncateTime(ts.TransactionDate) <= toDate)
                        select ts;
            return query;
        }

        public IEnumerable<Transaction> GetAllByTimeAndUsername(DateTime fromDate, DateTime toDate, string Username, int serviceId)
        {
            var query = from u in DbContext.Users
                        join ts in DbContext.Transactions
                        on u.Id equals ts.UserId
                        where u.UserName == Username && (DbFunctions.TruncateTime(ts.TransactionDate) >= fromDate && DbFunctions.TruncateTime(ts.TransactionDate) <= toDate) && ts.ServiceId == serviceId
                        select ts;
            return query;
        }

        public IEnumerable<Transaction> GetAll(DateTime fromDate, DateTime toDate)
        {
            var query = from u in DbContext.Users
                        join ts in DbContext.Transactions
                        on u.Id equals ts.UserId
                        where (DbFunctions.TruncateTime(ts.TransactionDate) >= (fromDate) && DbFunctions.TruncateTime(ts.TransactionDate) <= (toDate))
                        select ts;
            return query;
        }

        public IEnumerable<Transaction> GetAll(DateTime fromDate, DateTime toDate, string userId, int serviceId)
        {
            var query = from u in DbContext.Users
                        join ts in DbContext.Transactions
                        on u.Id equals ts.UserId
                        where (DbFunctions.TruncateTime(ts.TransactionDate) >= fromDate && DbFunctions.TruncateTime(ts.TransactionDate) <= toDate) && ts.ServiceId == serviceId && u.Id == userId
                        select ts;
            return query;
        }

        public IEnumerable<Transaction> GetAll(DateTime fromDate, DateTime toDate, int serviceId)
        {
            var query = from u in DbContext.Users
                        join ts in DbContext.Transactions
                        on u.Id equals ts.UserId
                        where (DbFunctions.TruncateTime(ts.TransactionDate) >= fromDate && DbFunctions.TruncateTime(ts.TransactionDate) <= toDate) && ts.ServiceId == serviceId
                        select ts;
            return query;
        }

        public IEnumerable<Transaction> GetAll(DateTime fromDate, DateTime toDate, string userId)
        {
            var query = from u in DbContext.Users
                        join ts in DbContext.Transactions
                        on u.Id equals ts.UserId
                        where (DbFunctions.TruncateTime(ts.TransactionDate) >= fromDate && DbFunctions.TruncateTime(ts.TransactionDate) <= toDate) && u.Id == userId
                        select ts;
            return query;
        }

        public IEnumerable<Transaction> GetAllByTimeAndPOID(DateTime fromDate, DateTime toDate, int id)
        {
            var listTransaction = from u in this.DbContext.Users
                                  join ts in this.DbContext.Transactions
                                  on u.Id equals ts.UserId
                                  where u.POID == id && (DbFunctions.TruncateTime(ts.TransactionDate) >= fromDate && DbFunctions.TruncateTime(ts.TransactionDate) <= toDate)
                                  select ts;

            return listTransaction;
        }

        public IEnumerable<Transaction> GetAllByTimeAndPOID(DateTime fromDate, DateTime toDate, int id, string userId, int serviceId)
        {
            var listTransaction = from u in this.DbContext.Users
                                  join ts in this.DbContext.Transactions
                                  on u.Id equals ts.UserId
                                  where u.POID == id && (DbFunctions.TruncateTime(ts.TransactionDate) >= fromDate && DbFunctions.TruncateTime(ts.TransactionDate) <= toDate) && u.Id == userId && ts.ServiceId == serviceId
                                  select ts;

            return listTransaction;
        }

        public IEnumerable<Transaction> GetAllByTimeAndPOID(DateTime fromDate, DateTime toDate, int id, string userId)
        {
            var listTransaction = from u in this.DbContext.Users
                                  join ts in this.DbContext.Transactions
                                  on u.Id equals ts.UserId
                                  where u.POID == id && (DbFunctions.TruncateTime(ts.TransactionDate) >= fromDate && DbFunctions.TruncateTime(ts.TransactionDate) <= toDate) && u.Id == userId
                                  select ts;

            return listTransaction;
        }

        public IEnumerable<Transaction> GetAllByTimeAndPOID(DateTime fromDate, DateTime toDate, int id, int serviceId)
        {
            var listTransaction = from u in this.DbContext.Users
                                  join ts in this.DbContext.Transactions
                                  on u.Id equals ts.UserId
                                  where u.POID == id && (DbFunctions.TruncateTime(ts.TransactionDate) >= fromDate && DbFunctions.TruncateTime(ts.TransactionDate) <= toDate) && ts.ServiceId == serviceId
                                  select ts;

            return listTransaction;
        }
    }
}