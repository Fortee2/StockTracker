using System;
using System.Collections.Generic;
using Domain;
using Core.Calculations.Response;
using Domain.Interfaces;

namespace Core.Calculations
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
        /// <param name="interval">The number of periods to average</param>
        /// <param name="columnToAvg">String that represents the name of the property to calculate the average from</param>
        /// <returns>An empty array if the array is smaller than the interval else it returns an array of
        /// AverageResponse objects
        /// </returns>
        public IList<AverageResponse> CreateMovingAverage(ushort interval, string columnToAvg)
        { 
            List<AverageResponse> responses = new();

            // Is the array too small?
            if ( activities.Count < interval) return responses;
            // Is the column name blank
            if (String.IsNullOrEmpty(columnToAvg)) return responses;

            //Adjust interval for 0 based array
            int seed = interval - 1;

            //loop over values calculating a moving average based on interval
            for (int i = seed; i < activities.Count -1; i++)
            {
                //Create a response object for each moving average
               responses.Add(
                   new AverageResponse(
                       activities[i].ActivityDate,
                       (float)Math.Round(Sum(i - seed, i, columnToAvg) / interval, 2)
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
            // Is the array too small?
            if (activities.Count < numberOfPeriods || numberOfPeriods == 0) return 0;
            // Is the column name blank
            if (String.IsNullOrEmpty(columnToAvg)) return 0;

            //subtract 1 from end to adjust for 0 based array
            int adjustedEnd = startPostion + numberOfPeriods - 1;

            return (float)Math.Round(Sum(startPostion, adjustedEnd, columnToAvg) / numberOfPeriods, 2);
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
    }
}
     