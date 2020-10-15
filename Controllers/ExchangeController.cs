using CheeryAssessment.Attributes;
using CheeryAssessment.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CheeryAssessment.Controllers
{
    [SessionAuthorize]
    public class ExchangeController : Controller
    {
        private DB_Entities _db = new DB_Entities();

        // GET: Exchange
        public ActionResult Index()
        {
            ExchangeIndexModel indexModel = new ExchangeIndexModel();
            indexModel.exchangeRateModel = GetRate();
            indexModel.userBalanceModel = GetBalance();
            indexModel.exchangeLogModel = new ExchangeLogModel
            {
                avail_currency_code = new[]
                {
                    new SelectListItem{ Value = indexModel.exchangeRateModel.rates.MYR.ToString() , Text = "MYR", Selected = true },
                    new SelectListItem{ Value = indexModel.exchangeRateModel.rates.AUD.ToString() , Text = "AUD" },
                    new SelectListItem{ Value = indexModel.exchangeRateModel.rates.SGD.ToString() , Text = "SGD" }
                }
            };
            

            //ViewBag.RateList = ConversionRateSelectList(indexModel.exchangeRateModel, true);
            //ViewData["RateList"] = ConversionRateSelectList(indexModel.exchangeRateModel, true);

            //ViewBag.CurrencyCode = ConversionRateSelectList(indexModel.exchangeRateModel, false);

            return View(indexModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ExchangeIndexModel indexModel, FormCollection formCollection)
        {
            if (ModelState.IsValid)
            {
                DateTime dtNow = DateTime.Now;
                long userid = long.Parse(Session["userid"].ToString());

                indexModel.exchangeLogModel.user_id = userid;
                indexModel.exchangeLogModel.transaction_date = dtNow;
                indexModel.exchangeLogModel.to_currency_code = formCollection["ToCurrencyCode"];

                
                var dataUserBalance = _db.userBalanceModels.Where(b => b.user_id == userid).ToList();

                if (dataUserBalance.Count > 0)
                {
                    decimal toAmount = decimal.Parse(indexModel.exchangeLogModel.to_amount);
                    decimal fromAmount = decimal.Parse(indexModel.exchangeLogModel.from_amount);
                    decimal balanceUSD = decimal.Parse(dataUserBalance.FirstOrDefault().bal_usd.ToString());
                    balanceUSD = decimal.Subtract(balanceUSD, fromAmount);

                    dataUserBalance.FirstOrDefault().bal_usd = balanceUSD.ToString();
                    dataUserBalance.FirstOrDefault().last_update_date = dtNow;

                    switch (indexModel.exchangeLogModel.to_currency_code)
                    {
                        case "MYR":
                            decimal balanceMYR = decimal.Parse(dataUserBalance.FirstOrDefault().bal_myr.ToString());
                            balanceMYR = decimal.Add(balanceMYR, toAmount);
                            dataUserBalance.FirstOrDefault().bal_myr = balanceMYR.ToString();
                            break;
                        case "AUD":
                            decimal balanceAUD = decimal.Parse(dataUserBalance.FirstOrDefault().bal_aud.ToString());
                            balanceAUD = decimal.Add(balanceAUD, toAmount);
                            dataUserBalance.FirstOrDefault().bal_aud = balanceAUD.ToString();
                            break;
                        case "SGD":
                            decimal balanceSGD = decimal.Parse(dataUserBalance.FirstOrDefault().bal_sgd.ToString());
                            balanceSGD = decimal.Add(balanceSGD, toAmount);
                            dataUserBalance.FirstOrDefault().bal_sgd = balanceSGD.ToString();
                            break;
                    }

                }


                //var data = _db.userModels.Where(s => s.email.Equals(email) && s.password.Equals(f_password)).ToList();

                _db.Configuration.ValidateOnSaveEnabled = false;
                _db.exchangeLogModels.Add(indexModel.exchangeLogModel);
                _db.SaveChanges();
            }
            else
            {
                ViewBag.error = "Login failed";
                //return RedirectToAction("Login");
                return View();
            }

            //return View();
            return RedirectToAction("ViewReport");
        }

        public ExchangeRateModel GetRate()
        {
            HttpWebRequest webreq = (HttpWebRequest)WebRequest.Create("https://openexchangerates.org/api/latest.json?app_id=43207fc5dc7148ae836473bbb1815996");
            webreq.Method = "GET";
            string responseFromServer = "";

            WebResponse response = webreq.GetResponse();
            using (var dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                responseFromServer = reader.ReadToEnd();
                Console.WriteLine(responseFromServer);
            }
            response.Close();

            var result = new JsonResult
            {
                Data = JsonConvert.DeserializeObject(responseFromServer)
            };

            ExchangeRateModel exchangeRateModel = new JavaScriptSerializer().Deserialize<ExchangeRateModel>(result.Data.ToString());



            return exchangeRateModel;
        }


        public UserBalanceModel GetBalance()
        {
            long userid = long.Parse(Session["userid"].ToString());
            var data = _db.userBalanceModels.Where(u => u.user_id == userid).ToList();
            UserBalanceModel res = data.FirstOrDefault();

            return res;
        }



        [NonAction]
        public SelectList ConversionRateSelectList(ExchangeRateModel _rateModel, bool isGetRate)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            list.Add(new SelectListItem()
            {
                Text = "MYR",
                Value = ((isGetRate == true) ? _rateModel.rates.MYR.ToString() : "MYR")
            });

            list.Add(new SelectListItem()
            {
                Text = "AUD",
                Value = ((isGetRate == true) ? _rateModel.rates.AUD.ToString() : "AUD")
            });

            list.Add(new SelectListItem()
            {
                Text = "SGD",
                Value = ((isGetRate == true) ? _rateModel.rates.SGD.ToString() : "SGD")
            });

            return new SelectList(list, "Value", "Text");
        }


        public ActionResult ViewReport()
        {
            long userid = long.Parse(Session["userid"].ToString());

            var data = _db.exchangeLogModels
                .Where(b => b.user_id == userid)
                .OrderByDescending(b => b.id)
                .ToList();

            List<ExchangeLogModel> listExchangeLogModel = data;

            return View(listExchangeLogModel);
        }
    }
}