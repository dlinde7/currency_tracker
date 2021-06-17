using currency_tracker.Utility;
using System;
using System.Globalization;

namespace currency_tracker.Models
{
    public class CurrencyDetail
    {
        public string ISO { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public bool IsVirtual { get; set; } = false;

        internal RegionInfo regionInfo = null;
        internal CultureInfo cultureInfo = null;

        public CurrencyDetail SetName(string name)
        {
            this.Name = name;
            return this;
        }

        public static CurrencyDetail GetDetails(string iso)
        {
            try
            {
                string simpleRegion = GetSimpleRegionISO(iso);
                CurrencyDetail detail = new CurrencyDetail();
                try
                {
                    try
                    {
                        //Attempts to use en-##
                        detail.cultureInfo = new CultureInfo(Constants.LANGUAGE + "-" + simpleRegion.ToUpper());
                    }
                    catch (CultureNotFoundException)
                    {
                        //Defaults to ##-##, culture
                        detail.cultureInfo = new CultureInfo(simpleRegion.ToLower() + "-" + simpleRegion.ToUpper());
                    }
                    detail.regionInfo = new RegionInfo(detail.cultureInfo.Name);
                }
                catch (ArgumentNullException)
                {
                    //return null as any attempt to do a check will throw an error
                    //i.e. cultureInfo(exception) && regionInfo(null)
                    return null;
                }
                catch (CultureNotFoundException)
                {
                    //Failed to get currency details from culture
                    //try to get currency details rom region info
                    //i.e. cultureInfo(exception) && regionInfo(null)
                    detail.regionInfo = new RegionInfo(Constants.LANGUAGE + "-" + simpleRegion.ToUpper());
                }
                catch (ArgumentException)
                {
                    //Invalid region from culture
                    //i.e. cultureInfo(set) && regionInfo(exception)
                    return null;
                    //detail.ISO = detail.cultureInfo.ThreeLetterISOLanguageName;
                    //detail.Name = detail.cultureInfo.NativeName;
                    //detail.Symbol = detail.cultureInfo.NumberFormat.CurrencySymbol;
                }

                return CheckISO(detail, iso);
            }
            catch (ArgumentException)
            {
                //iso parameter cannot be made upcase
                return null;
            }
        }

        private static CurrencyDetail CheckISO(CurrencyDetail detail,string iso)
        {
            if (iso.ToUpper() == detail.regionInfo.ISOCurrencySymbol.ToUpper())
            {
                detail.ISO = detail.regionInfo.ISOCurrencySymbol;
                detail.Name = detail.regionInfo.CurrencyEnglishName;
                detail.Symbol = detail.regionInfo.CurrencySymbol;
            }
            else
            {
                detail.cultureInfo = null;
                detail.regionInfo = null;
                detail.IsVirtual = true;
                detail.ISO = iso.ToUpper().Substring(0, Math.Min(iso.Length, 3));
                detail.Name = iso;
                detail.Symbol = iso.ToUpper().Substring(0, Math.Min(iso.Length, 3));
            }
            return detail;
        }

        private static string GetSimpleRegionISO(string region)
        {
            return region.Substring(0, Math.Min(region.Length, 2)).ToUpper();
        }

        public static string FormatValue(double value, CurrencyDetail detail)
        {
            if(detail != null)
            {
                if (detail.cultureInfo != null)
                    return value.ToString("C", detail.cultureInfo.NumberFormat);
                else
                    return detail.ISO + " " + value.ToString("N", CultureInfo.InvariantCulture);
            }
            return value.ToString("N", CultureInfo.InvariantCulture);
        }
    }
}