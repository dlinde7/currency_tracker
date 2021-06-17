namespace currency_tracker.Models
{
    public class CurrencyRatios
    {
        public double From { get; set; } = 1;
        public double To { get; set; } = 1;
        public double CalculatedRatio => Utility.Ratio.Calculate(To, From);
        public string Ratio { get; set; } = "1:1";
        public Currency FromCurrency { get; set; }
        public Currency ToCurrency { get; set; }
    }
}