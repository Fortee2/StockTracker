using System;
using System.Collections.Generic;
using StockTracker.Core.Calculations.Response;
using StockTracker.Core.Domain.Interfaces;
using StockTracker.Core.Interfaces.Calculations;

namespace StockTracker.Core.Calculations
{
    /// <summary>
    /// Takes an array of Trading Structures and creates different types of averages from the data
    /// </summary>
    public partial class Averages:ICalculate
    {
        protected readonly IList<ITradingStructure> activities;

        /// <summary>
        /// Intialize Object
        /// </summary>
        /// <param name="collection">The data to average</param>
        public Averages(IList<ITradingStructure> collection)
        {
            this.activities = collection;
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
        /// Creates a sum of the values in a given range
        /// </summary>
        /// <param name="start">The index in the array to start adding</param>
        /// <param name="stop">The index in the array to stop adding</param>
        /// <param name="columnName">The property name of the object to add</param>
        /// <returns>The sum of all the numbers in the give range</returns>
        protected float Sum(int start, int stop, string columnName)
        {
            float currentValue = (float)activities[start].GetValue(columnName);

            //We have hit the end of the list
            if (start == stop) return currentValue;

            //Add the current value to the next value in sequence
            return currentValue + Sum(start + 1, stop, columnName);
          
        }

        protected bool ArrayValidforAverage(int requiredNumberOfElements, string columnToAverage)
        {
            // Is the array too small?
            if (activities.Count < requiredNumberOfElements || requiredNumberOfElements == 0) return false;
            // Is the column name blank
            if (String.IsNullOrEmpty(columnToAverage)) return false;

            return true;
        }

        public virtual List<IResponse> Calculate()
        {
            throw new NotImplementedException();
        }
    }
}
     