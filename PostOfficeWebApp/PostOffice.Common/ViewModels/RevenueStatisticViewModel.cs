using System;

namespace PostOffice.Common.ViewModels
{
    public class RevenueStatisticViewModel
    {
        public DateTimeOffset Date { get; set; }
        public decimal Revenues { get; set; }
    }
}