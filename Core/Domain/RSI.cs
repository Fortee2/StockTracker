using System;
namespace StockTracker.Core.Domain
{
    public class RSI:BaseObject
    {
        public RSI(DateTime ActivityDate, decimal Close)
        {
            this.ActivityDate = ActivityDate;
            this.Close = Close;
        }

        public decimal Close { get; set; }
        public decimal Gain { get; set; }
        public decimal Loss { get; set; }
        public decimal AvgGain { get; set; }
        public decimal AvgLoss { get; set; }
        public decimal RSIndex { get; set; }
    }
}
