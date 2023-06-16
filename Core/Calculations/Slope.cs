using System.Collections.Generic;
using StockTracker.Core.Interfaces;
using StockTracker.Core.Interfaces.Calculations;
using System.Linq;
using StockTracker.Core.Domain;

namespace StockTracker.Core.Calculations
{

    public class Slope : BaseCalculator, ISlope
    {
        public Slope(IList<SlopeData> collection) : base(collection.Cast<ITradingStructure>().ToList())
        {

        }

    
        /// <summary>
        /// Calculate the slope of the line of best fit for the prices in the SlopeData objects
        /// Data needs to be in ascending order by date for the calculation to be correct
        /// </summary>
        public decimal Calculate()
        {
            // Get the list of SlopeData objects
            IList<SlopeData> slopeData = this.activities.Cast<SlopeData>().ToList();

            // Calculate the mean (average) of the dates and the prices
            // We're converting the DateTime to ticks (a long representing 100-nanosecond intervals since 0001-01-01)
            // because we need a numerical representation of the dates to calculate the slope
            decimal datesMean = (decimal)slopeData.Select(x => x.ActivityDate.Ticks).Average();
            decimal emaPricesMean = slopeData.Select(x=> x.Price).Average();

            // These variables will hold the sums that we need to calculate the slope
            decimal sumNumerator = 0.0m;
            decimal sumDenominator = 0.0m;

            // Loop over the SlopeData objects
            for (int i = 0; i < slopeData.Count; i++)
            {
                // Calculate the difference between the current date and the mean date,
                // and the difference between the current price and the mean price
                decimal dateDiff = slopeData[i].ActivityDate.Ticks - datesMean;
                decimal emaPriceDiff = slopeData[i].Price - emaPricesMean;

                // Add to the sums for the numerator and denominator of the slope formula
                sumNumerator += dateDiff * emaPriceDiff;
                sumDenominator += dateDiff * dateDiff;
            }

            // Calculate the slope by dividing the numerator sum by the denominator sum
            // This gives us the slope of the line of best fit for the prices over time
            decimal slope = sumNumerator / sumDenominator;

            // Return the slope, which indicates the overall direction of prices
            // A positive slope indicates an upward trend, and a negative slope indicates a downward trend
            return slope;
        }

    }
}