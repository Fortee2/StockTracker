using System;
using StockTracker.Core.Calculations.Response;

namespace StockTracker.Core.Domain{
    public class SmoothTRData:BaseObject{
        public SmoothTRData(DateTime date, decimal trueRangeValue, decimal smoothedTrueRangeValue){
            ActivityDate = date;
            TrueRange = trueRangeValue;
            SmoothedTrueRange = smoothedTrueRangeValue;
        }
        public SmoothTRData(){}
        public decimal TrueRange { get; set; }
        public decimal SmoothedTrueRange { get; set; }
    }
}