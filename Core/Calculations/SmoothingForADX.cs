using System;
using System.Collections.Generic;
using StockTracker.Core.Calculations.Response;
using StockTracker.Core.Interfaces;
using StockTracker.Core.Interfaces.Calculations;

namespace StockTracker.Core.Calculations
{
    public class SmoothingForADX:BaseCalculator, ICalculate<AverageResponse> { 
        private ushort _numberOfPeriods;
        private string _columnToSmooth, _columnPrevValue;
        private int startPosition = 0;

        /// <summary>
        /// Intialize Object
        /// </summary>
        /// <param name="activities">Data to calculate averages from</param>
        public SmoothingForADX(IList<ITradingStructure> activities) : base(activities)
        {
        }

        /// <summary>
        /// The number of items to average
        /// </summary>
        public ushort NumberOfPeriods { get => _numberOfPeriods; set => _numberOfPeriods = value; }
        /// <summary>
        /// String that represents the name of the property to calculate the average from
        /// </summary>
        public string ColumnToSmooth { get => _columnToSmooth; set => _columnToSmooth = value; }
        /// <summary>
        /// String that represents the name of the property to retrieve the previous periods EMA from.
        /// Looks for this value in the previous row.
        /// Set field to 0 if it has never been calculated.
        /// </summary>
        public string ColumnPrevSmoothedValue { get => _columnPrevValue; set => _columnPrevValue = value; }
        /// <summary>
        /// The poisition int the array to start calculating.  Defaults to 0.
        /// </summary>
        public int StartPosition { get => startPosition; set => startPosition = value; }


        /// <summary>
        /// Calculates a weighted average giving more weight to current price movements
        /// </summary>
        /// <returns>A list of averages</returns>
        public  List<AverageResponse> Calculate()
        {
            List<AverageResponse> responses = new();

            int startPos = StartPosition;

            decimal prevSymbol = activities[startPos].GetDecimalValue(ColumnPrevSmoothedValue);

            //never been calculated before
            if (prevSymbol == 0)
            {
                //Check to see if the array has enough data to calculate an average
                if (!ArrayValidforAverage(_numberOfPeriods, _columnToSmooth)) return responses;

                //if no EMA exists calculate a simple average as start
                //and place it into the prevEma variable for the next calculation
                Averages simpleAverage = new(activities, _numberOfPeriods, _columnToSmooth);
                prevSymbol = simpleAverage.Calculate();
                startPos = _numberOfPeriods; // Move index to correct position in the array   
            }
            else
            {
                startPos = 1;
            }

            return SmoothValues(startPos, activities.Count, _columnToSmooth, prevSymbol);
        }

        private static decimal Smooth(decimal currentValue, decimal previous)
        {
            //Create the weighted Smoothed Average
            return ((previous *13) + currentValue) / 14;
        }

        private List<AverageResponse> SmoothValues(int start, int end, string columnToAverage, decimal lastValue)
        {
            List<AverageResponse> responses = new();
            decimal holdValue = lastValue;

            for(int i = start; i < end; i++) {
                //Create the weighted Average
                decimal ema = Smooth(activities[i].GetDecimalValue(columnToAverage), holdValue);
                holdValue = ema;
                responses.Add(new AverageResponse(activities[i].ActivityDate, ema));
            }

            return responses;
        }
    }
}
