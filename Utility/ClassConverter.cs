using currency_tracker.Models;
using currency_tracker.Models.Outgoing;

namespace currency_tracker.Utility
{
    public partial class ClassConverter
    {
        public static SimpleCurrency ToSimple(Currency currency) {
            return new SimpleCurrency(){
                Name = currency.Details.SimpleName,
                Value = currency.BaseValue, //Update
            };
        }
    }
}