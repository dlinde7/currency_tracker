using currency_tracker.Models;
using currency_tracker.Utility;
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
    }
}
