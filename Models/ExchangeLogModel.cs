using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace CheeryAssessment.Models
{
    [Table("tbl_exchange_log")]
    public class ExchangeLogModel
    {
        [Key, Column(Order = 1)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        [ForeignKey("UserBalanceModel")]
        public long user_id { get; set; }

        public string from_currency_code { get; set; }

        public string from_amount { get; set; }

        public string conversion_rate { get; set; }

        public string to_currency_code { get; set; }

        public IEnumerable<SelectListItem> avail_currency_code { get; set; }

        public string to_amount { get; set; }

        public DateTime? transaction_date { get; set; }

        public virtual UserBalanceModel UserBalanceModel { get; set; }
    }
}