using System;
using System.Diagnostics;

namespace currency_tracker.Models.Database
{
    [DebuggerDisplay("Name:{Name} ISO:{Iso}")]
    public class Currency
    {
       public string Name { get; set; }
       public string Iso { get; set; }
       public double Value1 { get; set; }
       public double Value2 { get; set; }

       public Currency(string Name, string Iso, double Value1,double Value2){
           this.Name = Name;
           this.Iso = Iso;
           this.Value1 = Value1;
           this.Value2 = Value2;
       }
    }
}