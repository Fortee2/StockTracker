using System;
namespace StockTracker.Core.Domain
{
    public class MACDData:BaseObject
    {
        private decimal ema12 = 0;
        private decimal ema26 = 0;

        public MACDData():base()
        {
        }

        public MACDData(DateTime date, decimal closingPrice, decimal macd, decimal signal, decimal ema12, decimal ema26)
        {
            ActivityDate = date;
            Close = closingPrice;
            MACD = macd;
            Signal = signal;
            Previous12EMA = ema12;
            Previous26EMA = ema26;
        }

        public decimal Close { get; set; }
        public decimal MACD { get; set; }
        public decimal Previous12EMA { get { return ema12; } set { ema12 = value; CalculateMACD(); } }
        public decimal Previous26EMA { get { return ema26; } set { ema26 = value; CalculateMACD(); } }
        public decimal Signal { get; set; }

        private void CalculateMACD()
        {
            if(MACD == 0 && ema12 > 0 && ema26 > 0)
            {
                MACD = ema12 - ema26;
            }
        }
    }
}
