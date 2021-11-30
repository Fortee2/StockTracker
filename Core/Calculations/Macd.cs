using System;
using System.Collections.Generic;
using StockTracker.Core.Calculations.Response;
using StockTracker.Core.Domain;
using StockTracker.Core.Domain.Interfaces;
using StockTracker.Core.Interfaces.Calculations;

namespace StockTracker.Core.Calculations
{
    public class Macd:Averages
    {
        private string EMA12Column, EMA26Column, EMA9Column;


        private List<ITradingStructure> dataList;

        public Macd(List<ITradingStructure> list):base(list)
        {
            dataList = list;
        
        }

        public string EMA12Column1 { get => EMA12Column; set => EMA12Column = value; }
        public string EMA26Column1 { get => EMA26Column; set => EMA26Column = value; }
        public string EMA9Column1 { get => EMA9Column; set => EMA9Column = value; }

        public override List<IResponse> Calculate()
        {
            List<IResponse> macdResponses = new();

            //We need at least 27 periods to calculate our first point
            if (!ArrayValidforAverage(27, EMA12Column)) return macdResponses;

            ITradingStructure tradingStructure = dataList[0];

            //If one of these 3 are blank we are going to assume the calculations need to be reseeded
            if(tradingStructure.GetFloatValue(EMA12Column) == 0 || tradingStructure.GetFloatValue(EMA26Column) == 0 || tradingStructure.GetFloatValue(EMA9Column) == 0)
            { 
                foreach(ushort p in new ushort[] { 9, 12, 26 })
                {
                    string updateColumn = (p == 9) ? EMA9Column : (p == 12) ? EMA12Column1 : EMA26Column;
                    //For 9 periods This is setting the 10th position with the Average of the first 9 periods (0-8)
                    dataList[p].SetFloatValue(updateColumn,
                        CalculateSimpleAverage(
                            p,
                            "Close"
                        )
                    );
                    CalculateEMAforMacd(ExponetialMovingAverage.CalculateSmoothingWeight(2, p), p, 0, 27, updateColumn);
                }
                
            }
               

            return macdResponses;
        }

        /// <summary>
        /// Calculates EMA based on Closing price for a range of values
        /// </summary>
        /// <param name="smoothingWeight">Weighting Value to apply to prices</param>
        /// <param name="numberOfPeriods">Number of periods.  Used to create a simple average if EMA has never been calculated.</param>
        /// <param name="start">The postion in the array AFTER the last EMA value or 1 if never calculated.</param>
        /// <param name="stop">The postion in the array to stop calculating EMA.</param>
        /// <param name="columnToUpdate">The column in the array to update with the calculated EMA</param>
        private void CalculateEMAforMacd(float smoothingWeight, ushort numberOfPeriods, int start, int stop, string columnToUpdate)
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
                    dataList[start].GetFloatValue("close"),
                    ema,
                    smoothingWeight
                )
            );

            //Repeat
            CalculateEMAforMacd(smoothingWeight, 0, start + 1, stop, columnToUpdate);
        }
    }
}
