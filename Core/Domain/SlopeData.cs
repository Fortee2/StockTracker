using System;
using StockTracker.Core.Interfaces;

namespace StockTracker.Core.Domain
{
    public class SlopeData:BaseObject
    {
        public SlopeData()
        {
        }

        public SlopeData(DateTime date, decimal price)
        {
            ActivityDate = date;
            Price = price;
        }

        public decimal Price { get; set; }
    }
}