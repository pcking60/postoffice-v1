using PostOffice.Common.ViewModels;
using PostOfiice.DAta.Repositories;
using System.Collections.Generic;

namespace PostOffice.Service
{
    public interface IStatisticService
    {
        IEnumerable<RevenueStatisticViewModel> GetRevenueStatistic(string fromDate, string toDate);
        IEnumerable<UnitStatisticViewModel> GetUnitStatistic(string fromDate, string toDate);
    }

    public class StatisticService : IStatisticService
    {
        private ITransactionRepository _transactionRepository;
        private IStatisticRepository _statisticRepository;

        public StatisticService(ITransactionRepository transactionRepository, IStatisticRepository statisticRepository)
        {
            _transactionRepository = transactionRepository;
            _statisticRepository = statisticRepository;
        }

        public IEnumerable<RevenueStatisticViewModel> GetRevenueStatistic(string fromDate, string toDate)
        {
            return _transactionRepository.GetRevenueStatistic(fromDate, toDate);
        }
        public IEnumerable<UnitStatisticViewModel> GetUnitStatistic(string fromDate, string toDate)
        {
            return _statisticRepository.GetUnitStatistic(fromDate, toDate);
        }
    }
}