using System;
using System.Collections.Generic;
using StockTracker.Core.Calculations.Response;
using StockTracker.Core.Domain;

namespace StockTracker.Core.Calculations
{
    public class Macd
    {
        private List<MacdInput> dataList;

        public Macd(List<MacdInput> list)
        {
            dataList = list;
        }

        public List<MacdResponse> Calculate()
        {
            List<MacdResponse> macdResponses = new();

            //If one of these 3 are blank we are going to assume the calculations need to be reseeded
            if(dataList[0].Previous12EMA == 0 || dataList[0].Previous26EMA == 0 || dataList[0].Previous9EMA == 0)
               

            return macdResponses;
        }

        private float CalculateEMA(ushort numberOfPeriods)
        {
            return 0;
        }
    }
}
