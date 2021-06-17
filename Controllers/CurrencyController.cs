using currency_tracker.Models;
using currency_tracker.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace currency_tracker.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly ILogger<CurrencyController> _logger;
        static HttpClientHandler handler = new HttpClientHandler();
        HttpClient client = new HttpClient(handler);
        public CurrencyController(ILogger<CurrencyController> logger)
        {
            _logger = logger;
            client.BaseAddress = new Uri("https://cdn.jsdelivr.net/gh/fawazahmed0/currency-api@1/latest/currencies/");
        }

        [HttpGet("")]
        [HttpGet("all")]
        public IEnumerable<Currency> GetAll(string iso = null)
        {
            List<Currency> outList = new List<Currency>();
            List<Models.Database.Currency> values = Constants.DATABASE.Select();
            if (iso == null)
            {
                foreach (var item in values)
                {
                    outList.Add(new Currency
                    {
                        Value = item.Value2,
                        Details = CurrencyDetail.GetDetails(item.Iso, item.Name)
                    });
                }
                return outList;
            }
            else
            {
                string[] ISOs = iso.ToUpper().Split(Constants.DELIMINATORS);
                foreach (var item in values)
                {
                    if (ISOs.Contains(item.Iso.ToUpper()))
                        outList.Add(new Currency
                        {
                            Value = item.Value2,
                            Details = CurrencyDetail.GetDetails(item.Iso, item.Name)
                        });
                }
                return outList;
            }
        }

        [HttpGet("{iso}")]
        public Currency Get(string iso)
        {
            List<Models.Database.Currency> values = Constants.DATABASE.Select();
            foreach (var item in values)
            {
                if (item.Iso.ToUpper() == iso.ToUpper())
                    return new Currency
                    {
                        Value = item.Value2,
                        Details = CurrencyDetail.GetDetails(item.Iso, item.Name)
                    };
            }
            return null;
        }

        [HttpGet("convert")]
        public async Task<CurrencyRatios> GetRatio([FromQuery] string isoFrom, [FromQuery] string isoTo, double baseValue)
        {
            HttpResponseMessage response = await client.GetAsync(isoFrom + ".json");

            response.EnsureSuccessStatusCode();

            string jsonString = await response.Content.ReadAsStringAsync();

            var myJObject = JObject.Parse(jsonString);

            double from = myJObject.SelectToken(isoFrom + "." + isoTo).Value<double>() * baseValue;

            return new CurrencyRatios
            {
                From = baseValue,
                To = from,
                Ratio = (baseValue.ToString()) + ":" + (from.ToString()),
                FromCurrency = new Currency
                {
                    Value = baseValue,
                    Details = CurrencyDetail.GetDetails(isoFrom)
                },
                ToCurrency = new Currency
                {
                    Value = from,
                    Details = CurrencyDetail.GetDetails(isoTo)
                }
            };
        }

        [HttpGet("dailychange")]
        public CurrencyDailyChange GetDailyChange([FromQuery] string iso)
        {
            if (iso != null)
                try
                {
                    Models.Database.Currency row = Constants.DATABASE.SelectRow(iso);

                    double change = Ratio.Calculate(row.Value2, row.Value1);

                    return new CurrencyDailyChange
                    {
                        change = change,
                        currency = new Currency
                        {
                            Value = row.Value1,
                            Details = CurrencyDetail.GetDetails(iso)
                        }
                    };
                }
                catch (Exception)
                {
                    return null;
                }
            else
                return null;
        }
    }
}
//