using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheeryAssessment.Models
{
    public class ExchangeIndexModel
    {
        public ExchangeRateModel exchangeRateModel { get; set; }
        public UserBalanceModel userBalanceModel { get; set; }
        public ExchangeLogModel exchangeLogModel { get; set; }
    }
}