using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CheeryAssessment.Models
{
    public class ExchangeRateModel
    {
        public string disclaimer;
        public string license;
        public Int32 timestamp;
        public Rate rates;
        
        public class Rate
        {
            [DisplayFormat(DataFormatString = "{0:0.00}")]
            public double AUD;
            [DisplayFormat(DataFormatString = "{0:0.00}")]
            public double SGD;
            [DisplayFormat(DataFormatString = "{0:0.00}")]
            public decimal MYR;
        }
    }
}