using System;
using System.Runtime.Serialization;

namespace currency_tracker.Models
{
    public class Currency
    {
        public CurrencyDetail Details { get; set; }
        //public DateTime Date { get; set; } = DateTime.Now;
        public double Value { get; set; }

        public string Pretty
        {
            get => CurrencyDetail.FormatValue(Value, Details);
        }
    }
}