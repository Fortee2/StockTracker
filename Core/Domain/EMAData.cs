using System;

namespace StockTracker.Core.Domain
{
    /// <summary>
    /// Represents a security's price with supplemental data for a given time
    /// </summary>
    public class MAData:BaseObject
    {
        public MAData():base()
        {
            Id = 0;
            ActivityDate = DateTime.UnixEpoch;
            PrevMA = 0;
            CalculateValue = 0;
        }

        public MAData(int id, DateTime activityDate, decimal calculateValue, decimal previousMA) : base()
        {
            Id = id;
            ActivityDate = activityDate;
            PrevMA = previousMA;
            CalculateValue = calculateValue;
        }

        //Properties
        public decimal PrevMA { get; set; }
        public decimal CalculateValue { get; set; }

 
    }
}
