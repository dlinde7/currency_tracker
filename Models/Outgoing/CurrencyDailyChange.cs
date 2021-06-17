using System;

namespace currency_tracker.Models
{
    public class CurrencyDailyChange
    {
      public double change {get; set;} = 1;
      public Currency currency {get; set;}
    }
}