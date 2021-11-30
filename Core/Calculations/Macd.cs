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

        public override List<Interfaces.Calculations.IResponse> Calculate()
        {
            List<IResponse> macdResponses = new();
            int start = 0;

            ITradingStructure tradingStructure = dataList[0];

            //If one of these 3 are blank we are going to assume the calculations need to be reseeded
            if(tradingStructure.GetFloatValue(EMA12Column) == 0 || tradingStructure.GetFloatValue(EMA26Column) == 0 || tradingStructure.GetFloatValue(EMA9Column) == 0)
            {
                //This is setting the 10th position with the Average of the first 9 periods
                dataList[9].SetFloatValue(EMA9Column, CalculateSimpleAverage(9, "Close"));

                dataList[12].SetFloatValue(EMA12Column1, CalculateSimpleAverage(12, "Close"));
                dataList[26].SetFloatValue(EMA26Column, CalculateSimpleAverage(26, "Close"));
            }
               

            return macdResponses;
        }

        private float CalculateEMA(ushort numberOfPeriods, )
        {
            ExponetialMovingAverage.CalculateEMA()
            return 0;
        }
    }
}
