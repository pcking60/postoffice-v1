using PostOffice.Common.ViewModels;
using PostOfiice.DAta.Repositories;
using System.Collections.Generic;

namespace PostOffice.Service
{
    public interface IStatisticService
    {
        IEnumerable<RevenueStatisticViewModel> GetRevenueStatistic(string fromDate, string toDate);
    }

    public class StatisticService : IStatisticService
    {
        private ITransactionRepository _transactionRepository;

        public StatisticService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public IEnumerable<RevenueStatisticViewModel> GetRevenueStatistic(string fromDate, string toDate)
        {
            return _transactionRepository.GetRevenueStatistic(fromDate, toDate);
        }
    }
}