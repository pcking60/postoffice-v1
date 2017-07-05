﻿using PostOffice.Model.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostOffice.Model.Models
{
    [Table("TKBDHistories")]
    public class TKBDHistory : Auditable
    {
        [Key]
        [Column(Order =1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string CustomerId { get; set; }
        public string Name { get; set; }
        [Key]
        [Column(Order = 2)]
        public string Account { get; set; }
        public DateTimeOffset? TransactionDate { get; set; }
        public decimal? Money { get; set; }
        public decimal? Rate { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        public string UserId { get; set; }
    }
}