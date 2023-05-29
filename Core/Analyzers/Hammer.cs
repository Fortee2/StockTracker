using System;
using StockTracker.Core.Domain;
using StockTracker.Core.Interfaces;

namespace StockTracker.Core.Analyzers
{
	public class Hammer
	{
		public Hammer()
		{
		}

        public static bool IsHammerPattern(Quote tradingStructure)
        {
            // Calculate the body and shadow sizes
            decimal bodySize, shadowSize,
            open = tradingStructure.Open,
            close = tradingStructure.Close,
            high = tradingStructure.High,
            low = tradingStructure.Low;

            bodySize = Math.Abs(close - open);
            shadowSize = Math.Min(open, close) - low;

            // Identify the hammer pattern 
            bool isHammer = false;

           if (((high - low) > 3 * (open - close)) && //shadow is significantly larger than the body.
                ((close - low) / (.001m + high - low) > 0.6m) && //ensures that the body is at the upper end of the range.
                ((open - low) / (.001m + high - low) > 0.6m)) //ensures that the open price is also at the upper end of the range
            {
                isHammer = true;
            }

            return isHammer;
        }


    }
}

