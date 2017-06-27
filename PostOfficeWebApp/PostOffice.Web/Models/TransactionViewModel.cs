using PostOffice.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PostOffice.Web.Models
{
    public class TransactionViewModel
    {
        public int ID { get; set; }

        public int ServiceId { get; set; }

        public string UserId { get; set; }

        public int? Quantity { get; set; }       

        public decimal? TotalMoney { get; set; }

        public decimal? EarnMoney { get; set; }
        public float? VAT { get; set; }
        [Required]
        public DateTimeOffset TransactionDate { get; set; }

        public virtual ICollection<TransactionDetail> TransactionDetails { get; set; }
        #region

        public string ServiceName { get; set; }

        public string CreatedBy
        {
            get; set;
        }

        public DateTime? CreatedDate
        {
            get; set;
        }

        public string MetaDescription
        {
            get; set;
        }

        public string MetaKeyWord
        {
            get; set;
        }

        public bool Status
        {
            get; set;
        }

        public string UpdatedBy
        {
            get; set;
        }

        public DateTime? UpdatedDate
        {
            get; set;
        }

        #endregion
    }
}