using System;
namespace StockTracker.Core.Domain
{
    public class MacdInput:BaseObject
    {
        private float ema12 = 0;
        private float ema26 = 0;

        public MacdInput():base()
        {
        }

        public float Close { get; set; }
        public float MACD { get; set; }
        public float Previous12EMA { get { return ema12; } set { ema12 = value; CalculateMACD(); } }
        public float Previous26EMA { get { return ema26; } set { ema26 = value; CalculateMACD(); } }
        public float Signal { get; set; }

        private void CalculateMACD()
        {
            if(MACD == 0 && ema12 > 0 && ema26 > 0)
            {
                MACD = ema12 - ema26;
            }
        }
    }
}
