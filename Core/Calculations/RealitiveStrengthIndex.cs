using System;
using System.Collections.Generic;
using StockTracker.Core.Domain;

namespace StockTracker.Core.Calculations
{
    public class RelativeStrengthIndex
    {
        protected readonly IList<RelativeStrength> dataList;
        private const int Periods = 14; // Standard period for RSI calculation

        public RelativeStrengthIndex(IList<RelativeStrength> rsiData)
        {
            dataList = rsiData;
        }

        public void Calculate()
        {
            if (dataList.Count < Periods) return; // Ensure enough data

            CalculateInitialAverages();
            CalculateSubsequentValues();
        }

        private void CalculateInitialAverages()
        {
            decimal totalGain = 0, totalLoss = 0;
            for (int i = 1; i <= Periods; i++)
            {
                var change = dataList[i].Close - dataList[i - 1].Close;
                if (change > 0) totalGain += change;
                else totalLoss -= change; // Losses are positive numbers
            }

            dataList[Periods - 1].AvgGain = totalGain / Periods;
            dataList[Periods - 1].AvgLoss = totalLoss / Periods;
        }

        private void CalculateSubsequentValues()
        {
            for (int i = Periods; i < dataList.Count; i++)
            {
                var change = dataList[i].Close - dataList[i - 1].Close;
                var gain = change > 0 ? change : 0;
                var loss = change < 0 ? -change : 0;

                // Apply smoothing formula
                dataList[i].AvgGain = (dataList[i - 1].AvgGain * (Periods - 1) + gain) / Periods;
                dataList[i].AvgLoss = (dataList[i - 1].AvgLoss * (Periods - 1) + loss) / Periods;

                if (dataList[i].AvgLoss == 0)
                {
                    dataList[i].RSIndex = 100; // If no losses, RSI is 100
                }
                else
                {
                    var rs = dataList[i].AvgGain / dataList[i].AvgLoss;
                    dataList[i].RSIndex = 100 - (100 / (1 + rs));
                }
            }
        }
    }
}