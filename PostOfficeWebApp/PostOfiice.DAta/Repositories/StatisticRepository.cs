using PostOffice.Common.ViewModels;
using PostOfiice.DAta.Infrastructure;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Data.SqlClient;

namespace PostOfiice.DAta.Repositories
{
    public interface IStatisticRepository : IRepository<UnitStatisticViewModel>
    {
        IEnumerable<UnitStatisticViewModel> GetUnitStatistic(string fromDate, string toDate);
        IEnumerable<ReportFunction1> ReportFunction1(string fromDate, string toDate);
    }

    public class StatisticRepository : RepositoryBase<UnitStatisticViewModel>, IStatisticRepository
    {
        public StatisticRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<UnitStatisticViewModel> GetUnitStatistic(string fromDate, string toDate)
        {
            var parameters = new SqlParameter[]{
                new SqlParameter("@fromDate", fromDate),
                new SqlParameter("@toDate", toDate)
            };
            return DbContext.Database.SqlQuery<UnitStatisticViewModel>("getUnitStatistic @fromDate,@toDate", parameters);
        }

        public IEnumerable<ReportFunction1> ReportFunction1(string fromDate, string toDate)
        {
            var parameters = new SqlParameter[] {
                new SqlParameter("@fromDate", fromDate),
                new SqlParameter("@toDate",toDate)
            };
            return DbContext.Database.SqlQuery<ReportFunction1>("reportFunction1 @fromDate,@toDate", parameters);
        }
    }
}