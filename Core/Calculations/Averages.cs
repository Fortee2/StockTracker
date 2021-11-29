using System;
using System.Collections.Generic;
using StockTracker.Core.Calculations.Response;
using StockTracker.Core.Domain.Interfaces;

namespace StockTracker.Core.Calculations
{
    public class Averages
    {
        private readonly IList<ITradingStructure> activities;

        public Averages(IList<ITradingStructure> collection)
        {
            this.activities = collection;
        }

        /// <summary>
        /// Creates a moving average for the supplied stock activity list in the order the list was provided
        /// </summary>
        /// <param name="numberOfPeriods">The number of periods to average</param>
        /// <param name="columnToAvg">String that represents the name of the property to calculate the average from</param>
        /// <returns>An empty array if the array is smaller than the interval else it returns an array of
        /// AverageResponse objects
        /// </returns>
        public IList<AverageResponse> CreateMovingAverage(ushort numberOfPeriods, string columnToAvg)
        { 
            List<AverageResponse> responses = new();

            // Is the array too small?
            if ( activities.Count < numberOfPeriods) return responses;
            // Is the column name blank
            if (String.IsNullOrEmpty(columnToAvg)) return responses;

            //Adjust interval for 0 based array
            int seed = numberOfPeriods - 1;

            //loop over values calculating a moving average based on interval
            for (int i = seed; i < activities.Count -1; i++)
            {
                //Create a response object for each moving average
               responses.Add(
                   new AverageResponse(
                       activities[i].ActivityDate,
                       (float)Math.Round(Sum(i - seed, i, columnToAvg) / numberOfPeriods, 2)
                    )
                );
            }

            return responses;
        }

        /// <summary>
        /// Calculates an average for a set of numbers
        /// </summary>
        /// <param name="numberOfPeriods">The number of items to average</param>
        /// <param name="columnToAvg">String that represents the name of the property to calculate the average from</param>
        /// <param name="startPostion">The postion in the array to start.  Needs to be 0 based. Defaults to 0</param>
        /// <returns>Returns the average</returns>
        public float CalculateSimpleAverage(ushort numberOfPeriods, string columnToAvg, int startPostion = 0)
        {
            if (!ArrayValidforAverage(numberOfPeriods, columnToAvg)) return 0;

            //subtract 1 from end to adjust for 0 based array
            int adjustedEnd = startPostion + numberOfPeriods - 1;

            return (float)Math.Round(Sum(startPostion, adjustedEnd, columnToAvg) / numberOfPeriods, 2);
        }

        /// <summary>
        /// Calculates a weighted average giving more weight to current price movements
        /// </summary>
        /// <param name="numberOfPeriods">The number of items to average</param>
        /// <param name="columnToAvg">String that represents the name of the property to calculate the average fro</param>
        /// <param name="columnPreviousEma">String that represents the name of the property to retrieve the previous periods EMA from.
        /// Looks for this value in the previous row.
        /// Set field to 0 if it has never been calculated.</param>
        /// <param name="startPosition">The poisition int the array to start calculating.  Defaults to 0.</param>
        /// <param name="smoothingFactor">Number to calculate the weight from.  The larger the number the greater importance put on current prices.  Defaults to 2.</param>
        /// <returns></returns>
        public List<AverageResponse> CalculateExponentialMovingAverage (ushort numberOfPeriods, string columnToAvg, string columnPreviousEma,
            int startPosition = 0, int smoothingFactor = 2)
        {
            List<AverageResponse> responses = new();

            int startPos = startPosition;
            float smoothingWeight = smoothingFactor / (float)(numberOfPeriods + 1);
            float prevEma = activities[startPos].GetFloatValue(columnPreviousEma);
  
            //never been calculated before
            if (prevEma == 0 )
            {               
                //Check to see if the array has enough data to calculate an average
                if (!ArrayValidforAverage(numberOfPeriods, columnToAvg)) return responses;

                //if no EMA exists calculate a simple average as start
                //and place it into the prevEma variable for the next calculation
                prevEma = CalculateSimpleAverage(numberOfPeriods, columnToAvg);
                startPos = numberOfPeriods; // Move index to correct position in the array   
            }

            return CalculateEMA(startPos, activities.Count - 1, columnToAvg, prevEma, smoothingWeight);
        }

        /// <summary>
        /// Creates a sum of the values in a given range
        /// </summary>
        /// <param name="start">The index in the array to start adding</param>
        /// <param name="stop">The index in the array to stop adding</param>
        /// <param name="columnName">The property name of the object to add</param>
        /// <returns>The sum of all the numbers in the give range</returns>
        private float Sum(int start, int stop, string columnName)
        {
            float currentValue = (float)activities[start].GetValue(columnName);

            //We have hit the end of the list
            if (start == stop) return currentValue;

            //Add the current value to the next value in sequence
            return currentValue + Sum(start + 1, stop, columnName);
          
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
        private List<AverageResponse> CalculateEMA(int start, int end,  string columnToAverage, float lastEma, float smoothingWeight)
        {
            List<AverageResponse> responses = new();

            if (start > end) return responses; //We heave passed the end time to stop;

            //Create the weighted Average
            float ema = (smoothingWeight * activities[start].GetFloatValue(columnToAverage)) + (lastEma * (1 - smoothingWeight));  
            responses.Add(new AverageResponse(activities[start].ActivityDate, ema));

            //Move to the next position
            responses.AddRange(CalculateEMA(start + 1, end, columnToAverage, ema, smoothingWeight));

            return responses;
        }

        private bool ArrayValidforAverage(int requiredNumberOfElements, string columnToAverage)
        {
            // Is the array too small?
            if (activities.Count < requiredNumberOfElements || requiredNumberOfElements == 0) return false;
            // Is the column name blank
            if (String.IsNullOrEmpty(columnToAverage)) return false;

            return true;
        }
    }
}
     