using System;
using StockTracker.Core.Interfaces.Calculations;

namespace StockTracker.Core.Calculations.Response
{
    public class MacdResponse: BaseResponse
    {


        public MacdResponse()
        {
        }

        public float MACD { get; set; }
        public float Signal { get; set; }
        public float Previous12EMA { get; set; }
        public float Previous26EMA { get; set; }

    }
}
