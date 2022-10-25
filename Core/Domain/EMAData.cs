using System;

namespace StockTracker.Core.Domain
{
    /// <summary>
    /// Represents a security's price with supplemental data for a given time
    /// </summary>
    public class EMAData:BaseObject
    {
        public EMAData():base()
        {
            Id = 0;
            ActivityDate = DateTime.UnixEpoch;
            PrevEMA = 0;
            CalculateValue = 0;
        }

        public EMAData(int id, DateTime activityDate, decimal calculateValue, decimal previousEMA) : base()
        {
            Id = id;
            ActivityDate = activityDate;
            PrevEMA = previousEMA;
            CalculateValue = calculateValue;
        }

        //Properties
        public decimal PrevEMA { get; set; }
        public decimal CalculateValue { get; set; }

 
    }
}
