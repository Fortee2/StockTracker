using System;
using System.Collections.Generic;
using StockTracker.Core.Calculations.Response;
using StockTracker.Core.Domain;
using StockTracker.Core.Domain.Interfaces;
using StockTracker.Core.Interfaces.Calculations;
using System.Linq;

namespace StockTracker.Core.Calculations
{
    /// <summary>
    /// Class to Calculate Moving Average Convergence Divergence
    /// </summary>
    public class MACD:Averages
    {
        private string ema12Column, ema26Column, ema9Column, closingPriceColumn;

        private List<ITradingStructure> dataList;

        /// <summary>
        /// Intialize the object
        /// If MACD has never been calculated list should contain all available prices.  If resuming a
        /// MACD calculation the first row should contain the last set of calculated EMAs and all other
        /// items should contain the the closing prices for the new periods.
        /// </summary>
        /// <param name="list">An array containing the Closing Price and poperties to store the required EMAs</param>
        public MACD(List<ITradingStructure> list):base(list)
        {
            //Make sure the data is in the proper order;
            dataList = (from activity in list
                       orderby activity.ActivityDate
                       select activity).ToList();
        }

        /// <summary>
        /// Property that contains the Closing Price
        /// </summary>
        public string ClosingPriceColumn { get => closingPriceColumn; set => closingPriceColumn = value; }

        /// <summary>
        /// Property that contains the 12 Day EMA
        /// </summary>
        public string EMA12Column { get => ema12Column; set => ema12Column = value; }

        /// <summary>
        /// Property that contains the 26 Day EMA
        /// </summary>
        public string EMA26Column { get => ema26Column; set => ema26Column = value; }

        public string MACDColumn { get; set; }

        public string SignalColumn { get; set; }

        /// <summary>
        /// Calculates Macd Based on the supplied data
        /// </summary>
        /// <returns>list of MACD Response Objects contain the MACD, Signal, 9 Day EMA, 12 Day EMA, 26 Day EMA</returns>
        public override List<IResponse> Calculate()
        {
            List<IResponse> macdResponses = new();

            //We need at least 27 periods to calculate our first point
            if (!ArrayValidforAverage(27, ema12Column)) return macdResponses;

            PopulateAllEmas();

            //Our starting point is going to be the first postion with a 26 day average
            return CalculateMACD(FindStartingPostion(0, EMA26Column)); 
        }

        /// <summary>
        /// Calculates missing values for 9, 12, 26 EMAs
        /// </summary>
        private void PopulateAllEmas()
        {
            //MACD is auto Calculate when EMA is populated
            //Period 9 is a EMA of MACD
            foreach (ushort periods in new ushort[] { 12, 26, 9 })
            {
                //Set the ema column
                string updateColumn = (periods == 12) ? EMA12Column : (periods == 26) ? ema26Column : SignalColumn;
                // Find the first entry where we have missing values
                int startingPoint = FindStartingPostion(0, updateColumn);

                //All EMAs already exist for this period
                if (startingPoint == -1) continue; 

                //We need to seed the base values
                if (startingPoint == 0)  
                {
                    //For 9 periods This is setting the 10th position with the Average of the first 9 periods (0-8)
                    dataList[periods].SetFloatValue(updateColumn,
                        CalculateSimpleAverage(
                            periods,
                            (periods == 9)? MACDColumn: closingPriceColumn
                        )
                    );

                    startingPoint = periods + 1;
                }

                // Populate the missing EMAs
                CalculateEMAforMacd(
                    ExponetialMovingAverage.CalculateSmoothingWeight(2, periods),
                    periods,
                    startingPoint,
                    updateColumn
                );
            }
        }

        /// <summary>
        /// Calculates EMA based on Closing price for a range of values
        /// </summary>
        /// <param name="smoothingWeight">Weighting Value to apply to prices</param>
        /// <param name="numberOfPeriods">Number of periods.  Used to create a simple average if EMA has never been calculated.</param>
        /// <param name="start">The postion in the array AFTER the last EMA value or 1 if never calculated.</param>
        /// <param name="stop">The postion in the array to stop calculating EMA.</param>
        /// <param name="columnToUpdate">The column in the array to update with the calculated EMA</param>
        private void CalculateEMAforMacd(float smoothingWeight, int start, int stop, string columnToUpdate)
        { 
            //We are passed the end
            if (start > stop) return;

            int previous = start - 1;

            //Prime the pump with the previous value
            float ema = dataList[previous].GetFloatValue(columnToUpdate); 

            //Calculate EMA
            dataList[start].SetFloatValue(
                columnToUpdate,
                ExponetialMovingAverage.CalculateEMA
                (
                    dataList[start].GetFloatValue(closingPriceColumn),
                    ema,
                    smoothingWeight
                )
            );

            //Repeat
            CalculateEMAforMacd(smoothingWeight, start + 1, stop, columnToUpdate);
        }



        /// <summary>
        /// Find the index of the first missing value
        /// </summary>
        /// <param name="start">The position in the array to start</param>
        /// <param name="propertyToCheck">The property to check</param>
        /// <returns>Returns -1 if there are no missing values, otherwise the first postion in the array where no value exists</returns>
        private int FindStartingPostion(int start,  string propertyToCheck)
        {
            if (start == dataList.Count) return -1;  //EMA Has been calculated for all periods

            //This will give us the first row where there are no calculations.  We will resume from here.
            if (dataList[start].GetFloatValue(propertyToCheck) == 0) return start;

            //Keep Looking
            return FindStartingPostion(start + 1, propertyToCheck);
        }
    }
}
