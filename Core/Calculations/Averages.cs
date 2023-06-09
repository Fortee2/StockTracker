using System;
using System.Collections.Generic;
using StockTracker.Core.Calculations.Response;
using StockTracker.Core.Interfaces;
using StockTracker.Core.Interfaces.Calculations;


namespace StockTracker.Core.Calculations
{
    /// <summary>
    /// Takes an array of Trading Structures and creates different types of averages from the data
    /// </summary>
    public class Averages:BaseCalculator, IAverage
    {

        private readonly ushort numberOfPeriods;
        private readonly string columnToAvg;
        private readonly int startPostion;

        /// <summary>
        /// Intialize Object
        /// </summary>
        /// <param name="collection">The data to average</param>
        public Averages(IList<ITradingStructure> collection, ushort NumberofPeriods, string ColumnToAvg, int StartPostion = 0):base(collection)
        {
            numberOfPeriods = NumberofPeriods;
            columnToAvg = ColumnToAvg;
            startPostion = StartPostion;
        }

        /// <summary>
        /// Calculates an average for a set of numbers
        /// </summary>
        /// <param name="numberOfPeriods">The number of items to average</param>
        /// <param name="columnToAvg">String that represents the name of the property to calculate the average from</param>
        /// <param name="startPostion">The postion in the array to start.  Needs to be 0 based. Defaults to 0</param>
        /// <returns>Returns the average</returns>
        public decimal Calculate()
        {
            if (!ArrayValidforAverage(numberOfPeriods, columnToAvg)) return 0;

            //subtract 1 from end to adjust for 0 based array
            int adjustedEnd = startPostion + numberOfPeriods - 1;

            return (decimal)Math.Round(Sum(startPostion, adjustedEnd, columnToAvg) / numberOfPeriods, 2);
        }



    }
}
     