using System;
using System.Collections.Generic;
using StockTracker.Core.Calculations.Response;
using StockTracker.Core.Domain.Interfaces;
using StockTracker.Core.Interfaces.Calculations;

namespace StockTracker.Core.Calculations
{
    public class MovingAveraage:Averages
    {
        private ushort numberOfPeriods;
        private string columnToAvg;

        public MovingAveraage(IList<ITradingStructure> activities) : base(activities)
        {
        }

        /// <summary>
        /// The number of periods to average
        /// </summary>
        public ushort NumberOfPeriods { get => numberOfPeriods; set => numberOfPeriods = value; }
        /// <summary>
        /// String that represents the name of the property to calculate the average from
        /// </summary>
        public string ColumnToAvg { get => columnToAvg; set => columnToAvg = value; }

        /// <summary>
        /// Creates a moving average for the supplied stock activity list in the order the list was provided
        /// </summary>
        /// <returns>An empty array if the array is smaller than the interval else it returns an array of
        /// AverageResponse objects
        /// </returns>
        public override List<IResponse> Calculate()
        {
            List<IResponse> responses = new();

            //Check to see if the array has enough data to calculate an average
            if (!ArrayValidforAverage(NumberOfPeriods, ColumnToAvg)) return responses;

            //Adjust interval for 0 based array
            int seed = NumberOfPeriods - 1;

            //loop over values calculating a moving average based on interval
            for (int i = seed; i < activities.Count - 1; i++)
            {
                //Create a response object for each moving average
                responses.Add(
                    new AverageResponse(
                        activities[i].ActivityDate,
                        (float)Math.Round(Sum(i - seed, i, ColumnToAvg) / NumberOfPeriods, 2)
                     )
                 );
            }

            return responses;
        }
    }
}
