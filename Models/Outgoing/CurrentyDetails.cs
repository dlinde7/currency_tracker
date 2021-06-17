using System.Globalization;

namespace currency_tracker.Models
{
    public class CurrencyDetail
    {
        public string ISO { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public bool IsVirtual { get; set; } = false;

        public static CurrencyDetail FromISO(string iso, string fallback = null)
        {
            try
            {
                string simpleRegion = iso;
                if (simpleRegion.Length > 2)
                    simpleRegion = simpleRegion.ToCharArray()[0].ToString() + simpleRegion.ToCharArray()[1].ToString();
                return FromRegion(simpleRegion, fallback);
            }
            catch (System.ArgumentException)
            {
                return new CurrencyDetail()
                {
                    ISO = iso,
                    Name = fallback == null ? iso : fallback,
                    Symbol = iso,
                    IsVirtual = true
                };
            }
        }

        public static CurrencyDetail FromRegion(string region, string fallback = null)
        {
            region = region.ToUpper();
            try
            {
                //Will thrown an exception if the region is not valid
                //see https://docs.microsoft.com/en-us/dotnet/api/system.globalization.regioninfo.isocurrencysymbol?view=net-5.0#remarks for valid regions
                RegionInfo regionInfo = new RegionInfo(region);

                return new CurrencyDetail()
                {
                    ISO = regionInfo.ISOCurrencySymbol,
                    Name = regionInfo.CurrencyEnglishName,
                    Symbol = regionInfo.CurrencySymbol,
                    IsVirtual = false
                };
            }
            catch (System.ArgumentNullException)
            {
                return null;
            }
            catch (System.ArgumentException)
            {
                //the region entered was not a valid currency
                throw;
            }
        }
    }
}