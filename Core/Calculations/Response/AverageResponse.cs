using System;
using StockTracker.Core.Interfaces.Calculations;

namespace StockTracker.Core.Calculations.Response
{
    public class AverageResponse:BaseResponse
    {
        public AverageResponse()
        {
       
        }

        public AverageResponse(DateTime activityDate, decimal value)
        {
            ActivityDate = activityDate;
            Value = value;
        }

        public decimal Value { get; set; }
    }
}
