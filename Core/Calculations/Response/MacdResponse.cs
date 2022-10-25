using System;
using StockTracker.Core.Domain;
using StockTracker.Core.Interfaces.Calculations;

namespace StockTracker.Core.Calculations.Response
{
    public class MacdResponse: BaseResponse
    {

        public MacdResponse(MACDData calculatedData)
        {
            ActivityDate = calculatedData.ActivityDate;
            MACD = calculatedData.MACD;
            Signal = calculatedData.Signal;
            Previous12EMA = calculatedData.Previous12EMA;
            Previous26EMA = calculatedData.Previous26EMA;
        }

        public MacdResponse(DateTime date, decimal MACDValue, decimal SignalValue, decimal EMA12Value, decimal EMA26Value )
        {
            ActivityDate = date;
            MACD = MACDValue;
            Signal = SignalValue;
            Previous12EMA = EMA12Value;
            Previous26EMA = EMA26Value;
        }

        public MacdResponse()
        {

        }

        public decimal MACD { get; set; }
        public decimal Signal { get; set; }
        public decimal Previous12EMA { get; set; }
        public decimal Previous26EMA { get; set; }

    }
}
