namespace currency_tracker.Models
{
    public class CurrencyDetail
    {
        public string ISO { get; set; }
        public string Name { get; set; }
        public string SimpleName { get; set; }
        public bool IsVirtual { get; set; } = false;
    }
}