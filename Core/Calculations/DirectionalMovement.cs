using System;
using StockTracker.Core.Calculations.Response;
using StockTracker.Core.Domain;
using StockTracker.Core.Interfaces.Calculations;
using System.Collections.Generic;

namespace StockTracker.Core.Calculations
{

    public class DirectionalMovement :ICalculate<DirectionalResponse>
    {
        private IList<DirectionalMovementData>  _directionalMovementData;

        public DirectionalMovement(IList<DirectionalMovementData> directionalMovementData){
            _directionalMovementData = directionalMovementData;
        }

        public List<DirectionalResponse> Calculate()
        {
            List<DirectionalResponse> directionalResponses = new List<DirectionalResponse>();

            foreach (var highLowData in _directionalMovementData)
            {
                directionalResponses.Add(CalculateDirectionalMovement(highLowData));
            }
            
            return directionalResponses;
        }

        private DirectionalResponse CalculateDirectionalMovement(DirectionalMovementData directionalMovement)
        {
            decimal HighMinusLow = directionalMovement.High - directionalMovement.Low;
            decimal HighMinusPreviousHigh = Math.Abs(directionalMovement.High - directionalMovement.PreviousHigh);
            decimal PreviousLowMinusLow = Math.Abs(directionalMovement.PreviousLow - directionalMovement.Low);

            decimal PositiveDirectionalMovement = HighMinusLow > HighMinusPreviousHigh ? HighMinusLow : HighMinusPreviousHigh;
            decimal NegativeDirectionalMovement = HighMinusLow > PreviousLowMinusLow ? HighMinusLow : PreviousLowMinusLow;

            return new DirectionalResponse(directionalMovement.ActivityDate, PositiveDirectionalMovement, NegativeDirectionalMovement);
        }
    }

}