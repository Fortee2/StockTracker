using System;
namespace StockTracker.Core.Domain
{
    public class DirectionalMovementData:BaseObject
    {
        public DirectionalMovementData(DateTime ActivityDate, decimal High, decimal Low, decimal PreviousHigh, decimal PreviousLow)
        {
            this.ActivityDate = ActivityDate;
            this.High = High;
            this.Low = Low;
            this.PreviousHigh = PreviousHigh;
            this.PreviousLow = PreviousLow;
        }

        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal PreviousHigh { get; set; }
        public decimal PreviousLow { get; set; }
    }
    
}