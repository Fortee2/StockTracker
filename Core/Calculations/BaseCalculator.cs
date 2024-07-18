using System;
using System.Collections.Generic;
using System.Linq;
using StockTracker.Core.Interfaces;


namespace StockTracker.Core.Calculations
{
    public partial class BaseCalculator
    {
        protected readonly IList<ITradingStructure> activities;
        public BaseCalculator(IList<ITradingStructure> collection)
        {
            //Make sure the data is in the proper order;
            this.activities = (from activity in collection
                        orderby activity.ActivityDate ascending
                        select activity).ToList();
        }

        public void Initialize()
        {
            // Initialize the calculator
        }

        protected virtual bool ArrayValidforAverage(int requiredNumberOfElements, string columnToAverage)
        {
            // Is the column name blank
            if (String.IsNullOrEmpty(columnToAverage)) return false;

            // Is the array too small?
            if (activities.Count < requiredNumberOfElements || requiredNumberOfElements == 0) return false;

            return true;
        }

        /// <summary>
        /// Creates a sum of the values in a given range
        /// </summary>
        /// <param name="start">The index in the array to start adding</param>
        /// <param name="stop">The index in the array to stop adding</param>
        /// <param name="columnName">The property name of the object to add</param>
        /// <returns>The sum of all the numbers in the give range</returns>
        protected decimal Sum(int start, int stop, string columnName)
        {
            decimal currentValue = (decimal)activities[start].GetValue(columnName);

            //We have hit the end of the list
            if (start == stop) return currentValue;

            //Add the current value to the next value in sequence
            return currentValue + Sum(start + 1, stop, columnName);
            
        }
    }
}