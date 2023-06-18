using System;
using StockTracker.Core.Calculations.Response;
using StockTracker.Core.Domain;
using StockTracker.Core.Interfaces.Calculations;
using System.Collections.Generic;
using System.Linq;

namespace StockTracker.Core.Calculations
{

    public class DirectionalMovement :ICalculate<DirectionalResponse>
    {
        private IList<DirectionalMovementData>  _directionalMovementData;

        public DirectionalMovement(IList<DirectionalMovementData> directionalMovementData){
            _directionalMovementData = (from dm in directionalMovementData
                                       orderby dm.ActivityDate
                                       select dm).ToList();
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
            decimal HighMinusPreviousHigh = directionalMovement.High - directionalMovement.PreviousHigh;
            decimal PreviousLowMinusLow = directionalMovement.PreviousLow - directionalMovement.Low;

            decimal PositiveDirectionalMovement = 0M;
            decimal NegativeDirectionalMovement = 0M;

            if (HighMinusPreviousHigh > 0 && HighMinusPreviousHigh > PreviousLowMinusLow)
            {
                PositiveDirectionalMovement = HighMinusPreviousHigh;
            }

            if (PreviousLowMinusLow > 0 && PreviousLowMinusLow > HighMinusPreviousHigh)
            {
                NegativeDirectionalMovement = PreviousLowMinusLow;
            }

            return new DirectionalResponse(directionalMovement.ActivityDate, PositiveDirectionalMovement, NegativeDirectionalMovement);
        }

    }

}