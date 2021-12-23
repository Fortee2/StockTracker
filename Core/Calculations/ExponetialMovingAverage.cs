using System;
using System.Collections.Generic;
using StockTracker.Core.Calculations.Response;
using StockTracker.Core.Domain.Interfaces;
using StockTracker.Core.Interfaces.Calculations;

namespace StockTracker.Core.Calculations
{
    public class ExponetialMovingAverage:Averages { 
        private ushort numberOfPeriods;
        private string columnToAvg, columnPreviousEma;
        private int startPosition = 0, smoothingFactor = 2;

        /// <summary>
        /// Intialize Object
        /// </summary>
        /// <param name="activities">Data to calculate averages from</param>
        public ExponetialMovingAverage(IList<ITradingStructure> activities) : base(activities)
        {
        }

        /// <summary>
        /// The number of items to average
        /// </summary>
        public ushort NumberOfPeriods { get => numberOfPeriods; set => numberOfPeriods = value; }
        /// <summary>
        /// String that represents the name of the property to calculate the average from
        /// </summary>
        public string ColumnToAvg { get => columnToAvg; set => columnToAvg = value; }
        /// <summary>
        /// String that represents the name of the property to retrieve the previous periods EMA from.
        /// Looks for this value in the previous row.
        /// Set field to 0 if it has never been calculated.
        /// </summary>
        public string ColumnPreviousEma { get => columnPreviousEma; set => columnPreviousEma = value; }
        /// <summary>
        /// The poisition int the array to start calculating.  Defaults to 0.
        /// </summary>
        public int StartPosition { get => startPosition; set => startPosition = value; }
        /// <summary>
        /// Number to calculate the weight from.  The larger the number the greater importance put on current prices.  Defaults to 2.
        /// </summary>
        public int SmoothingFactor { get => smoothingFactor; set => smoothingFactor = value; }

        /// <summary>
        /// Calculates a weighted average giving more weight to current price movements
        /// </summary>
        /// <returns>A list of averages</returns>
        public override List<IResponse> Calculate()
        {
            List<IResponse> responses = new();

            int startPos = StartPosition;
            decimal smoothingWeight = CalculateSmoothingWeight(SmoothingFactor, NumberOfPeriods);
            decimal prevEma = activities[startPos].GetDecimalValue(ColumnPreviousEma);

            //never been calculated before
            if (prevEma == 0)
            {
                //Check to see if the array has enough data to calculate an average
                if (!ArrayValidforAverage(NumberOfPeriods, ColumnToAvg)) return responses;

                //if no EMA exists calculate a simple average as start
                //and place it into the prevEma variable for the next calculation
                prevEma = CalculateSimpleAverage(NumberOfPeriods, ColumnToAvg);
                startPos = NumberOfPeriods; // Move index to correct position in the array   
            }

            return CalculateEMA(startPos, activities.Count - 1, ColumnToAvg, prevEma, smoothingWeight);
        }

        /// <summary>
        /// Creates a weight averafe for the supplied values
        /// </summary>
        /// <param name="currentValue"></param>
        /// <param name="lastEma"></param>
        /// <param name="smoothingWeight"></param>
        /// <returns></returns>
        public static decimal CalculateEMA(decimal currentValue, decimal lastEma, decimal smoothingWeight)
        {
            //Create the weighted Average
            return (smoothingWeight * currentValue) + (lastEma * (1 - smoothingWeight));
        }

        public static decimal CalculateSmoothingWeight(int smoothingFactor, ushort periods)
        {
            return smoothingFactor / (decimal)(periods + 1);
        }

        /// <summary>
        /// Creates a weight average for a given range
        /// </summary>
        /// <param name="start">The begining of the range.  Corresponds to an array index.</param>
        /// <param name="end">The end of the range.  Corresponds to an array index.</param>
        /// <param name="columnToAverage">The propety name to find the value to average</param>
        /// <param name="lastEma">The weighted average from the previous calculation</param>
        /// <param name="smoothingWeight">A weight to be applied in the average calculation</param>
        /// <returns></returns>
        private List<IResponse> CalculateEMA(int start, int end, string columnToAverage, decimal lastEma, decimal smoothingWeight)
        {
            List<IResponse> responses = new();

            if (start > end) return responses; //We heave passed the end time to stop;

            //Create the weighted Average
            decimal ema = CalculateEMA(activities[start].GetDecimalValue(columnToAverage), lastEma, smoothingWeight); 
            responses.Add(new AverageResponse(activities[start].ActivityDate, ema));

            //Move to the next position
            responses.AddRange(CalculateEMA(start + 1, end, columnToAverage, ema, smoothingWeight));

            return responses;
        }
    }
}
