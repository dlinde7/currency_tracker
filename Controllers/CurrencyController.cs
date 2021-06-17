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
        public IEnumerable<Currency> GetAll(string? iso = null)
        {
            List<Models.Database.Currency> values = Constants.DATABASE.Select();
            if (iso == null)
            {
                return values.ToArray().Select(value => new Currency
                {
                    Value = value.Value2,
                    Details = CurrencyDetail.GetDetails(value.Iso)
                })
                .ToArray();
            }
            else
            {
                List<Currency> outList = new List<Currency>();
                string[] ISOs = iso.ToUpper().Split(Constants.DELIMINATORS);
                foreach (var item in values)
                {
                    if (ISOs.Contains(item.Iso.ToUpper()))
                        outList.Add(new Currency
                        {
                            Value = item.Value2,
                            Details = CurrencyDetail.GetDetails(item.Iso).SetName(item.Name)
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
                if (item.Iso.ToUpper()==iso.ToUpper())
                    return new Currency
                    {
                        Value = item.Value2,
                        Details = CurrencyDetail.GetDetails(item.Iso).SetName(item.Name)
                    };
            }
            return null;
        }

        [HttpGet("ratio/{iso1}/{iso2}")]
        public async Task<CurrencyRatios> GetRatio(string iso1, string iso2)
        {
            HttpResponseMessage response = await client.GetAsync(iso1 + ".json");

            response.EnsureSuccessStatusCode();

            string jsonString = await response.Content.ReadAsStringAsync();

            var myJObject = JObject.Parse(jsonString);

            double ratio = myJObject.SelectToken(iso1 + "." + iso2).Value<double>();

            return new CurrencyRatios
            {
                From = 1,
                To = ratio,
                ratio = "1:" + (ratio.ToString()),
                FromCurrency = new Currency
                {
                    Value = 1,
                    Details = CurrencyDetail.GetDetails(iso1)
                },
                ToCurrency = new Currency
                {
                    Value = ratio,
                    Details = CurrencyDetail.GetDetails(iso2)
                }
            };
        }
    }
}
