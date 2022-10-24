using System;

namespace StockTracker.Core.Domain
{
    /// <summary>
    /// Represents a security's price with supplemental data for a given time
    /// </summary>
    public class Quote:BaseObject
    {

        public Quote(int id, DateTime activityDate, decimal high, decimal low, decimal open, decimal close, int volume) : base()
        {
            Id = id;
            ActivityDate = activityDate;
            High = high;
            Low = low;
            Open = open;
            Close = close;
            Volume = volume;
        }

        //Properties
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Open { get; set; }
        public decimal Close { get; set; }

        public int Volume { get; set; }
    }
}
