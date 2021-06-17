using System;

namespace currency_tracker.Models
{
    public class Currency
    {
        public CurrencyDetail Details { get; set; }
        public DateTime Date => DateTime.Now;
        public string Pretty {get; set;}
        public double Value {get; set;}
    }
}