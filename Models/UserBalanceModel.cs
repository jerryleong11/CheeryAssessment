using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CheeryAssessment.Models
{
    [Table("tbl_user_balance")]
    public class UserBalanceModel
    {
        [Key, ForeignKey("UserModel")]
        public long user_id { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public string bal_usd { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public string bal_myr { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public string bal_sgd { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public string bal_aud { get; set; }

        public DateTime? last_update_date { get; set; }

        public DateTime? creation_date { get; set; }

        public virtual UserModel UserModel { get; set; }

        public virtual ICollection<ExchangeLogModel> ExchangeLogModels { get; set; }
    }
}