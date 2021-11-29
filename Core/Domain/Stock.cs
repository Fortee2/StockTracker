using System;
using System.Collections.Generic;

namespace StockTracker.Core.Domain
{
    public class Stock
    {
        public int Id { get; set; }
        public string CompnayName { get; set; }
        public string Ticker { get; set; }

        ICollection<Activity> History { get; set; }
    }
}
