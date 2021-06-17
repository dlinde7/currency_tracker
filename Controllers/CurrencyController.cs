using currency_tracker.Models;
using currency_tracker.Utility;
using currency_tracker.Database;
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
            if (iso == null)
            {
                return Constants.DATABASE.Select().ToArray().Select(value => new Currency
                {
                    Value = value.Value2,
                    Details = CurrencyDetail.GetDetails(value.Iso)
                })
                .ToArray();
            }
            else
            {
                var outList = new List<Currency>();
                foreach (var item in iso.Split(Constants.DELIMINATORS))
                {
                    outList.Add(new Currency
                    {
                        Value = 1,
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
