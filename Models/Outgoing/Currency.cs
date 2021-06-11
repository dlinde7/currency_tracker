using System;

namespace currency_tracker.Models
{
    public class Currency
    {
        public double BaseValue { get; set; } = 1;
        //public double Value { get; set; } = Currency.BASE.Value * Value;
        public CurrencyDetail Details { get; set; }
        public DateTime Date => DateTime.Now;
        public String Pretty {get; set;}
    }
}