using System;
using System.Collections.Generic;
using StockTracker.Core.Domain;
using StockTracker.Core.Calculations;
using NUnit.Framework;
using StockTracker.Core.Calculations.Response;
using StockTracker.Core.Domain.Interfaces;
using StockTracker.Core.Interfaces.Calculations;

namespace StockTracker.CoreTests
{
    public class MovingAverageTests
    {
        private IList<ITradingStructure> stockHistory;
        private MovingAveraage averages;

        [SetUp]
        public void Setup() {

            //Make up some test data
            stockHistory = new List<ITradingStructure>();
            Random random = new();

            for(int i =0;i < 10; i++)
            {
                stockHistory.Add(
                    new Activity(1,
                        DateTime.Now.Date.AddDays(i),
                        random.Next(1,500),
                        random.Next(1, 500),
                        random.Next(1, 500),
                        random.Next(1, 500),
                        random.Next(100, 5000)
                    )
                );
            }

            //Intialize our test class
            averages = new(stockHistory);
        }

        [Test]
        public void ChecMovingkAverageCal()
        {
            try
            {
                averages.ColumnToAvg = "close";
                averages.NumberOfPeriods = 3;

                List<IResponse> responses = averages.Calculate();

                Assert.AreEqual(7, responses.Count);
                Assert.AreEqual(DateTime.Now.AddDays(2).Date, responses[0].ActivityDate);

                decimal avg = (decimal) Math.Round( (stockHistory[0].GetDecimalValue("Close") + stockHistory[1].GetDecimalValue("Close") + stockHistory[2].GetDecimalValue("Close")) / 3,2);
                Assert.AreEqual(avg, responses[0].GetDecimalValue("Value"));

            }
            catch(Exception e )
            { 
                Assert.Fail(e.Message);
            }
        }

        [Test]
        public void CheckInvalidInterval()
        {
            try
            {
                averages.ColumnToAvg = "close";
                averages.NumberOfPeriods = 100;

                List<IResponse> responses = averages.Calculate();

                Assert.AreEqual(0, responses.Count);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [Test]
        public void CheckAverageCal()
        { 
            decimal response = averages.CalculateSimpleAverage(3, "open");

            decimal avg = (decimal)Math.Round((stockHistory[0].GetDecimalValue("Open") + stockHistory[1].GetDecimalValue("Open") + stockHistory[2].GetDecimalValue("Open")) / 3, 2);
            Assert.AreEqual(avg, response);
        }

        [Test]
        public void CheckAverageWithOffset()
        {
            decimal response = averages.CalculateSimpleAverage(3, "open", 2);

            decimal avg = (decimal)Math.Round((stockHistory[2].GetDecimalValue("Open") + stockHistory[3].GetDecimalValue("Open") + stockHistory[4].GetDecimalValue("Open")) / 3, 2);
            Assert.AreEqual(avg, response);
        }

    }
}
