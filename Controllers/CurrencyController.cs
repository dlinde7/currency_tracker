using currency_tracker.Models;
using currency_tracker.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using Newtonsoft.Json.Linq;

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
            var rng = new Random();
            if (iso == null)
            {
                return Enumerable.Range(1, 5).Select(index => new Currency
                {
                    Value = rng.NextDouble(),
                    Details = CurrencyDetail.GetDetails("ZAR")
                })
                .ToArray();
            }
            else
            {
                var outList = new List<Currency>();
                foreach (var item in iso.Split(Constants.DELIMINATORS))
                {
                    var tempValue = rng.NextDouble()*100000;
                    outList.Add(new Currency
                    {
                        Value = tempValue,
                        Details = CurrencyDetail.GetDetails(item)
                    });
                }

                return outList;
            }
        }

        [HttpGet("{iso}")]
        public Currency Get(string iso)
        {
            var rng = new Random();
            return new Currency
            {
                Value = rng.NextDouble(),
                Details = CurrencyDetail.GetDetails(iso)
            };
        }

        [HttpGet("ratio/{iso1}/{iso2}")]
        public async Task<CurrencyRatios> GetRatio(string iso1, string iso2)
        {
          HttpResponseMessage response = await client.GetAsync(iso1+".json");

          response.EnsureSuccessStatusCode();

          string jsonString = await response.Content.ReadAsStringAsync();

          var myJObject = JObject.Parse(jsonString);

          double ratio = myJObject.SelectToken(iso1+"."+iso2).Value<double>();

          return new CurrencyRatios
            {
                From = 1,
                To = ratio,
                ratio = "1:"+(ratio.ToString()),
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
