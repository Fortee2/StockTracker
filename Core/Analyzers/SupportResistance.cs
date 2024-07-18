using System;
using System.Collections.Generic;
using StockTracker.Core.Interfaces;

namespace StockTracker.Core.Analyzers
{
	public class SupportResistance
	{
		public SupportResistance()
		{
		}

		private List<ITradingStructure> activities;
		private Decimal[] valuesArray;
		private Decimal factor = 0.05M;

        public List<ITradingStructure> SecurityActivity {
            get
            {
				return activities;
            }
			set {
				if(value == null || value.Count == 0)
                {
					throw (new Exception("List contains no data."));
                }

				activities = value;
			}
		}

        public String ColumnToAnalyze { get; set; }

        public void CheckTrend()
        {
			CreateArray();
			Analyze();

        }

		private void Analyze()
        {
			int count = 0;
			decimal maxValue = 0;
			
			for(int i = 0; i < valuesArray.Length; i++)
            {
				int currCount = 0;
				decimal currMax = 0;

				decimal[] ceilingFloor = CalculateCeilingFloor(valuesArray[1]);
				for(int j = 0; j < valuesArray.Length; j++)
                {
                    if (j == i)
                    {
						continue;
                    }

					if (InRange(valuesArray[j], ceilingFloor[0], ceilingFloor[1]))
                    {
						currCount = currCount++;
                    }
                }

				if(currCount > count)
				{
					count = currCount;
					maxValue = currMax;
				}
            }


			Console.WriteLine(String.Format("Points Matched: {0}  Max Value: {1}", count, maxValue));
        }

		private void CreateArray()
        {
			valuesArray = new decimal[activities.Count];
			for(int i =0; i < activities.Count; i++)
            {
				valuesArray[i] = activities[i].GetDecimalValue(ColumnToAnalyze);
            }

			Array.Sort(valuesArray);
        }

		/// <summary>
        /// Creates Ceiling and Floor for a value
        /// </summary>
        /// <param name="value">The value to create a ceiling and floor for</param>
        /// <returns>Array with 2 entries.  Position 0 is the floor and Position 1 is the ceiling.</returns>
		private decimal[] CalculateCeilingFloor(decimal value)
        {
			decimal[] result = new decimal[2];
			decimal factor = CalculateFactor(value);
			result[1] = value + (value * factor); //ceiling
			result[0] = value - (value * factor);  //floor

			return result;
		}

		private bool InRange(decimal value, decimal min, decimal max)
        {
			return ((value - min) * (max - value) >= 0);
        }

		private decimal CalculateFactor(decimal value)
        {
			return (value * factor);
        }
    }
}

