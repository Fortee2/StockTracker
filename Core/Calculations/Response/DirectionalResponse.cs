using System;

namespace StockTracker.Core.Calculations.Response{
    public class DirectionalResponse:BaseResponse{
        public DirectionalResponse(DateTime activityDate, decimal positiveDirectionalMovement, decimal negativeDirectionalMovement)
        {
            ActivityDate = activityDate;
            PositiveDirectionalMovement = positiveDirectionalMovement;
            NegativeDirectionalMovement = negativeDirectionalMovement;
        }

        public decimal PositiveDirectionalMovement { get; set; }
        public decimal NegativeDirectionalMovement { get; set; }
    }
}