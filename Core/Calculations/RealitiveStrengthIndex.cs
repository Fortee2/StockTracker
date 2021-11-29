using System;
using System.Collections;
using System.Collections.Generic;
using Domain;
using Domain.Interfaces;

namespace Core.Calculations
{
    public class RealitiveStrengthIndex
    {
        protected readonly IList<RSI> dataList;

        /// <summary>
        /// Creates an instatnce of the RSI caluclation library
        /// </summary>
        /// <param name="rsiData">
        /// A list of RSI objects to update.  If
        /// no previous RSI data exists this object should have just its
        /// activity date and close price populated.
        /// </param>
        public RealitiveStrengthIndex(IList rsiData)
        {
            dataList = (IList<RSI>) rsiData;
        }

        /// <summary>
        /// Calculate RSI based on the data provided in the list
        /// </summary>
        public void Calculate()
        {
            int itemCount = dataList.Count;

            //Not enough data
            if (itemCount < 1) return;

            CalculateGainLoss();

            CalculateIndex();
        }

        /// <summary>
        /// Updates the RSI list with the difference between the current periods
        /// close and the previous periods close
        /// </summary>
        private void CalculateGainLoss()
        {
            int itemCount = dataList.Count;

            for (int i = 1; i < itemCount - 1; i++)
            {
                RSI rSI = dataList[i];

                //If both are zero the entry has never been set or trading was
                //flat
                if (rSI.Gain == 0 && rSI.Loss == 0)
                {
                    float gl = (float)Math.Round(rSI.Close - dataList[i - 1].Close, 2);



                    //Negative number signals loss
                    if (gl < 0)
                    {
                        rSI.Loss = Math.Abs(gl);
                        continue;
                    }

                    //We don't want to record items where the difference is 0
                    //So we test again
                    if (gl > 0)
                    {
                        rSI.Gain = gl;
                    }
                }
            }
        }

        private void CalculateIndex()
        {
            int itemCount = dataList.Count;

            for (int i = 0; i < itemCount - 1; i++)
            {
                RSI rSI = dataList[i];

                //never been calculated before
                if(i == 0 && rSI.AvgGain == 0 && rSI.AvgLoss == 0)
                {
                    if (itemCount < 14) break; //Not enough data

                    //Since we don't have any previous data we start with a simple Average
                    Averages averages = new (ConvertArrayForAvg());
                    i = 13;  //Advance to the correct place in the array
                    dataList[i].AvgGain = (float)Math.Round(averages.CalculateSimpleAverage(14, "Gain"),2);
                    dataList[i].AvgLoss = (float)Math.Round(averages.CalculateSimpleAverage(14, "Loss"),2);

                    continue;
                }

                dataList[i].AvgGain = CalulateWeightedAverage("AvgGain", "Gain", i);
                dataList[i].AvgLoss = CalulateWeightedAverage("AvgLoss", "Loss", i);

                dataList[i].RSIndex = CalculateRsi(i);
            }
        }

        private float CalulateWeightedAverage(string AvgColumnName, string ColumnName, int postion)
        {
            return (float)Math.Round((((dataList[postion - 1].GetFloatValue(AvgColumnName) * 13) + dataList[postion].GetFloatValue(ColumnName)) / 14),2);
        }

        private float CalculateRsi(int idx)
        {
            float rs = dataList[idx].AvgGain / dataList[idx].AvgLoss;

            //Convert Relative Strength into a number between 0 and 100
            return (float) Math.Round(100 - (100 / (rs + 1)), 0);
        }

        private List<ITradingStructure> ConvertArrayForAvg()
        {
            List<ITradingStructure> tradingStructures = new(14);
            short counter = 0;

            foreach(RSI rSI in dataList)
            {
                if (counter == 14) break;

                tradingStructures.Add(rSI);
                counter++;
            }

            return tradingStructures;
        }
    }
}
