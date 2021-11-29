using System;
using StockTracker.Core.Interfaces.Calculations;

namespace StockTracker.Core.Calculations.Response
{
    public class MacdResponse: IResponse
    {
        public MacdResponse()
        {
        }

        public DateTime ActivityDate { get; set; }
        public float MACD { get; set; }
        public float Signal { get; set; }
        public float EMA { get; set; }
        public float Previous12EMA { get; set; }
        public float Previous26EMA { get; set; }
        public float Previous9EMA { get; set; }
    }
}
