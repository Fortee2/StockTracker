using System;
using System.Collections.Generic;
using StockTracker.Core.Domain;
using StockTracker.Core.Interfaces.Calculations;
namespace StockTracker.Core.Calculations
{
    public class TrueRange:ICalculate<TrueRangeResponse> {
        private List<Quote> _quotes;

        public TrueRange(List<Quote> quotes){
            _quotes = quotes;
        }
        
        public List<TrueRangeResponse> Calculate(){
            List<TrueRangeResponse> trueRangeResponses = new List<TrueRangeResponse>();
        decimal HighMinusLow = 0,
            HighMinusPreviousClose = 0,  
            LowMinusPreviousClose = 0,
            TrueRange = 0;

            foreach(var quote in _quotes){
                HighMinusLow = quote.High - quote.Low;
                HighMinusPreviousClose = Math.Abs( quote.High - quote.Close);
                LowMinusPreviousClose = Math.Abs(quote.Low - quote.Close);

                TrueRange = HighMinusLow > HighMinusPreviousClose ? HighMinusLow : HighMinusPreviousClose;
                TrueRange = TrueRange > LowMinusPreviousClose ? TrueRange : LowMinusPreviousClose;

                trueRangeResponses.Add(new TrueRangeResponse(quote.ActivityDate, TrueRange));
            }

            return trueRangeResponses;
        }

    }
}