using System;
namespace StockTracker.Core.Calculations.Response
{
    public class AverageResponse
    {
        public AverageResponse()
        {
       
        }

        public AverageResponse(DateTime activityDate, float value)
        {
            ActivityDate = activityDate;
            Value = value;
        }

        public DateTime ActivityDate { get; set; }
        public float Value { get; set; }
    }
}
