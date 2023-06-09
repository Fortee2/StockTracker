using System;
using System.Collections.Generic;
using StockTracker.Core.Calculations.Response;
using StockTracker.Core.Domain;
using StockTracker.Core.Interfaces;
using StockTracker.Core.Interfaces.Calculations;

namespace StockTracker.Core.Calculations
{
    /// <summary>
    /// Class to Calculate Moving Average Convergence Divergence
    /// </summary>
    public class MACD:BaseCalculator, ICalculate<MacdResponse>
    {
        private string ema12Column, ema26Column, closingPriceColumn;

        /// <summary>
        /// Intialize the object
        /// If MACD has never been calculated list should contain all available prices.  If resuming a
        /// MACD calculation the first row should contain the last set of calculated EMAs and all other
        /// items should contain the the closing prices for the new periods.
        /// </summary>
        /// <param name="list">An array containing the Closing Price and poperties to store the required EMAs</param>
        public MACD(List<ITradingStructure> list):base(list)
        {
    
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
        public  List<MacdResponse> Calculate()
        {
            List<MacdResponse> macdResponses = new();

            //We need at least 27 periods to calculate our first point
            if (!ArrayValidforAverage(27, ema12Column)) return macdResponses;

            PopulateClosingPriceEMAs();
            PopulateSignalEMAs();

            return CreateResponse();
        }

        public List<MacdResponse> CreateResponse()
        {
            List<MacdResponse> responses = new();

            if (activities[0].GetType().Name.Equals("MACDData"))
            {
                foreach(ITradingStructure tradingStructure in activities)
                {
                    responses.Add(new MacdResponse((MACDData)tradingStructure));
                }

            }
            else
            {
                foreach(ITradingStructure tradingStructure in activities)
                {
                    responses.Add(
                        new MacdResponse(
                            tradingStructure.ActivityDate,
                            tradingStructure.GetDecimalValue(MACDColumn),
                            tradingStructure.GetDecimalValue(SignalColumn),
                            tradingStructure.GetDecimalValue(EMA12Column),
                            tradingStructure.GetDecimalValue(EMA26Column)
                        )
                    );
                }
            }

            return responses;
        }

        /// <summary>
        /// Calculates missing values for 12, 26 EMAs
        /// </summary>
        private void PopulateClosingPriceEMAs()
        {
            //MACD is auto Calculate when EMA is populated
            //Period 9 is a EMA of MACD
            foreach (ushort periods in new ushort[] { 12, 26 })
            {
                //Set the ema column
                string updateColumn = (periods == 12) ? EMA12Column : ema26Column;
                // Find the first entry where we have missing values
                int startingPoint = FindStartingPostion(0, updateColumn);

                //All EMAs already exist for this period
                if (startingPoint == -1) continue; 

                //We need to seed the base values
                if (startingPoint == 0)  
                {
                    //For 12 periods This is setting the 13th position with the Average of the first 12 periods (0-11)
                    Averages averages = new(this.activities, periods, closingPriceColumn);
                    this.activities[periods].SetDecimalValue(updateColumn,
                        averages.Calculate()
                    );

                    startingPoint = periods + 1;
                }

                // Populate the missing EMAs
                CalculateEMAforMacd(
                    ExponetialMovingAverage.CalculateSmoothingWeight(2, periods),
                    startingPoint,
                    activities.Count,
                    closingPriceColumn,
                    updateColumn
                );
            }
        }

        /// <summary>
        /// Calculates missing values for 9 period MACD EMA (Signal)
        /// </summary>
        private void PopulateSignalEMAs()
        {
            // Find the first entry where we have missing values
            int startingPoint = (this.activities[0].GetDecimalValue(SignalColumn) == 0)? FindPostionWithValue(0, MACDColumn): FindStartingPostion(1,SignalColumn);

            //All EMAs already exist for this period
            if (startingPoint == -1) return;

            //We need to seed the base values
            if (startingPoint == 0)
            {
                //For 9 periods This is setting the 10th position with the Average of the first 9 periods (0-8)
                Averages averages = new(this.activities, 9, MACDColumn, startingPoint);
                this.activities[9].SetDecimalValue(SignalColumn,
                    averages.Calculate()
                );

                startingPoint=10;
            }   

            // Populate the missing EMAs
            CalculateEMAforMacd(
                ExponetialMovingAverage.CalculateSmoothingWeight(2, 9),
                startingPoint,
                activities.Count,
                MACDColumn,
                SignalColumn
            );
            
        }

        /// <summary>
        /// Calculates EMA based on Closing price or MACD for a range of values
        /// </summary>
        /// <param name="smoothingWeight">Weighting Value to apply to prices</param>
        /// <param name="start">The postion in the array AFTER the last EMA value or 1 if never calculated.</param>
        /// <param name="stop">The postion in the array to stop calculating EMA.</param>
        /// <param name="columnToCalculate">The column in the array to calculate the EMA for.  For MACD that is the Closing Price or MACD column</param>
        /// <param name="columnToUpdate">The column in the array to update with the calculated EMA</param>
        private void CalculateEMAforMacd(decimal smoothingWeight, int start, int stop, string columnToCalculate, string columnToUpdate)
        {
            int previous = start - 1;
            decimal ema = this.activities[previous].GetDecimalValue(columnToUpdate);

            for (int i = start; i < stop; i++)
            {
                this.activities[i].SetDecimalValue(
                    columnToUpdate,
                    ExponetialMovingAverage.CalculateEMA
                    (
                        this.activities[i].GetDecimalValue(columnToCalculate),
                        ema,
                        smoothingWeight
                    )
                );

                ema = this.activities[i].GetDecimalValue(columnToUpdate);
            }
        }

        /// <summary>
        /// Find the index of the first missing value
        /// </summary>
        /// <param name="start">The position in the array to start</param>
        /// <param name="propertyToCheck">The property to check</param>
        /// <returns>Returns -1 if there are no missing values, otherwise the first postion in the array where no value exists</returns>
        private int FindStartingPostion(int start,  string propertyToCheck)
        {
            if (start == this.activities.Count) return -1;  //EMA Has been calculated for all periods

            //This will give us the first row where there are no calculations.  We will resume from here.
            if (this.activities[start].GetDecimalValue(propertyToCheck) == 0) return start;

            //Keep Looking
            return FindStartingPostion(start + 1, propertyToCheck);
        }

        /// <summary>
        /// Find the index of the first missing value
        /// </summary>
        /// <param name="start">The position in the array to start</param>
        /// <param name="propertyToCheck">The property to check</param>
        /// <returns>Returns -1 if there are no missing values, otherwise the first postion in the array where no value exists</returns>
        private int FindPostionWithValue(int start, string propertyToCheck)
        {
            if (start == this.activities.Count) return -1;  //EMA Has been calculated for all periods

            //This will give us the first row where there are no calculations.  We will resume from here.
            if (this.activities[start].GetDecimalValue(propertyToCheck) != 0) return start;

            //Keep Looking
            return FindStartingPostion(start + 1, propertyToCheck);
        }
    }
}
