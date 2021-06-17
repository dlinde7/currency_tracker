using currency_tracker.Database;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace currency_tracker.Utility
{
    public class Constants
    {
        public const string LANGUAGE = "en";
        public static readonly char[] DELIMINATORS = { ',', ':', '|', ';', '.', '-' };
        public static Database.Database DATABASE = new Database.Database();
    }
}
