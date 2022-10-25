using System;
using System.Collections.Generic;
using StockTracker.Core.Domain;
using StockTracker.Core.Calculations;
using NUnit.Framework;
using StockTracker.Core.Calculations.Response;
using StockTracker.Core.Interfaces;
using StockTracker.Core.Interfaces.Calculations;

namespace StockTracker.CoreTests
{
    public class EMATests
    {
        private IList<ITradingStructure> stockHistory;
        private IList<ITradingStructure> emaList;

        private ExponetialMovingAverage averages;

        [SetUp]
        public void Setup() {

            //Make up some test data
            stockHistory = new List<ITradingStructure>();
            emaList = new List<ITradingStructure>();

            AddStockHistory();
            AddEMAHistory();

            //Intialize our test class
            averages = new(stockHistory);
        }

        [Test]
        public void CheckExponetialMovingkAverageCal()
        {
            try
            {
                averages.NumberOfPeriods = 4;
                averages.ColumnToAvg = "close";

                List<IResponse> responses = averages.Calculate();

                Assert.AreEqual(6, responses.Count);
                Assert.AreEqual(new DateTime(2021, 11, 19, 0, 0, 0), responses[0].ActivityDate);
                Assert.AreEqual((decimal)79.55, Math.Round( responses[0].GetDecimalValue("Value"), 2));

            }
            catch(Exception e )
            { 
                Assert.Fail(e.Message);
            }
        }

        [Test]
        public void CheckExponetialMovingkAverageWithHistory()
        {
            try
            {
                averages = new ExponetialMovingAverage(emaList);
                averages.NumberOfPeriods = 4;
                averages.ColumnPreviousEma = "PrevEMA";
                averages.ColumnToAvg = "CalculateValue";

                List<IResponse> responses = averages.Calculate();

                Assert.AreEqual(15, responses.Count);
                Assert.AreEqual(new DateTime(2017, 12, 29, 0, 0, 0), responses[6].ActivityDate);
                Assert.AreEqual((decimal)37.66, Math.Round(responses[6].GetDecimalValue("Value"), 2));

            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        public void AddStockHistory()
        {

            stockHistory.Add(
                new Quote(1,
                    new DateTime(2021, 11, 15, 0, 0, 0),
                    (decimal)80.4008,
                    (decimal)76.8488,
                    (decimal)80.3913,
                    (decimal)77.2107,
                    111000
                )
            );


            stockHistory.Add(
                new Quote(1,
                    new DateTime(2021, 11, 16, 0, 0, 0),
                    (decimal)80.4008,
                    (decimal)76.8488,
                    (decimal)80.3913,
                    (decimal)77.6964,
                    111000
                )
            );

            stockHistory.Add(
                new Quote(1,
                    new DateTime(2021, 11, 17, 0, 0, 0),
                    (decimal)80.4008,
                    (decimal)76.8488,
                    (decimal)80.3913,
                    (decimal)78.8296,
                    111000
                )
            );


            stockHistory.Add(
                new Quote(1,
                    new DateTime(2021, 11, 18, 0, 0, 0),
                    (decimal)80.4008,
                    (decimal)76.8488,
                    (decimal)80.3913,
                    (decimal)80.1437,
                    111000
                )
            );


            stockHistory.Add(
                new Quote(1,
                    new DateTime(2021, 11, 19, 0, 0, 0),
                    (decimal)80.4008,
                    (decimal)76.8488,
                    (decimal)80.3913,
                    (decimal)81.1627,
                    111000
                )
            );


            stockHistory.Add(
                new Quote(1,
                    new DateTime(2021, 11, 20, 0, 0, 0),
                    (decimal)80.4008,
                    (decimal)76.8488,
                    (decimal)80.3913,
                    (decimal)81.4959,
                    111000
                )
            );


            stockHistory.Add(
                new Quote(1,
                    new DateTime(2021, 11, 21, 0, 0, 0),
                    (decimal)80.4008,
                    (decimal)76.8488,
                    (decimal)80.3913,
                    (decimal)81.2769,
                    111000
                )
            );


            stockHistory.Add(
                new Quote(1,
                    new DateTime(2021, 11, 22, 0, 0, 0),
                    (decimal)80.4008,
                    (decimal)76.8488,
                    (decimal)80.3913,
                    (decimal)80.6464,
                    111000
                )
            );


            stockHistory.Add(
                new Quote(1,
                    new DateTime(2021, 11, 23, 0, 0, 0),
                    (decimal)80.4008,
                    (decimal)76.8488,
                    (decimal)80.3913,
                    (decimal)82.7053,
                    111000
                )
            );


            stockHistory.Add(
                new Quote(1,
                    new DateTime(2021, 11, 24, 0, 0, 0),
                    (decimal)80.4008,
                    (decimal)76.8488,
                    (decimal)80.3913,
                    (decimal)82.2578,
                    111000
                )
            );
        }

        public void AddEMAHistory()
        {
            emaList.Add(new EMAData(0, new DateTime(2017, 12, 20), (decimal)38.728, (decimal)38.19));
            emaList.Add(new EMAData(0, new DateTime(2017, 12, 21), (decimal)38.4001, (decimal)0));
            emaList.Add(new EMAData(0, new DateTime(2017, 12, 22), (decimal)38.2726, (decimal)0));
            emaList.Add(new EMAData(0, new DateTime(2017, 12, 26), (decimal)38.0722, (decimal)0));
            emaList.Add(new EMAData(0, new DateTime(2017, 12, 27), (decimal)37.6259, (decimal)0));
            emaList.Add(new EMAData(0, new DateTime(2017, 12, 28), (decimal)37.6897, (decimal)0));
            emaList.Add(new EMAData(0, new DateTime(2017, 12, 29), (decimal)37.3344, (decimal)0));
            emaList.Add(new EMAData(0, new DateTime(2018, 1, 2), (decimal)38.0722, (decimal)0));
            emaList.Add(new EMAData(0, new DateTime(2018, 1, 3), (decimal)39.0012, (decimal)0));
            emaList.Add(new EMAData(0, new DateTime(2018, 1, 4), (decimal)40.2035, (decimal)0));
            emaList.Add(new EMAData(0, new DateTime(2018, 1, 5), (decimal)40.0851, (decimal)0));
            emaList.Add(new EMAData(0, new DateTime(2018, 1, 8), (decimal)40.2764, (decimal)0));
            emaList.Add(new EMAData(0, new DateTime(2018, 1, 9), (decimal)40.1215, (decimal)0));
            emaList.Add(new EMAData(0, new DateTime(2018, 1, 10),(decimal)39.1652, (decimal)0));
            emaList.Add(new EMAData(0, new DateTime(2018, 1, 11),(decimal)40.2491, (decimal)0));
        }

    }
}
