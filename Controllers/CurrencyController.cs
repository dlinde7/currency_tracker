using currency_tracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace currency_tracker.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly ILogger<CurrencyController> _logger;

        public CurrencyController(ILogger<CurrencyController> logger)
        {
            _logger = logger;
        }

        [HttpGet("currencies")]
        [HttpGet("currency/all")]
        public IEnumerable<Currency> GetAll(string? iso)
        {
            var rng = new Random();
            if (iso == null)
            {
                return Enumerable.Range(1, 5).Select(index => new Currency
                {
                    Value = rng.NextDouble(),
                    Details = CurrencyDetail.FromISO("ZAR")
                })
                .ToArray();
            }
            else
            {
                var temp = iso.Split(',', '|', ';',':');
                var outList = new List<Currency>();
                foreach (var item in temp)
                {
                    outList.Add(new Currency
                    {
                        Value = rng.NextDouble(),
                        Details = CurrencyDetail.FromISO(item)
                    });
                }

                return outList;
            }
        }

        [HttpGet("currencies/{iso}")]
        [HttpGet("currency/all/{iso}")]
        public Currency Get(string iso)
        {
            var rng = new Random();
            return new Currency
            {
                Value = rng.NextDouble(),
                Details = CurrencyDetail.FromISO(iso)
            };
        }
    }
}
