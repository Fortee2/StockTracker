using System;
using System.Collections.Generic;
using StockTracker.Core.Domain;
using StockTracker.Core.Calculations;
using NUnit.Framework;
using StockTracker.Core.Calculations.Response;
using StockTracker.Core.Domain.Interfaces;

namespace StockTracker.CoreTests
{
    public class MovingAverageTests
    {
        private IList<ITradingStructure> stockHistory;
        private Averages averages;

        [SetUp]
        public void Setup() {

            //Make up some test data
            stockHistory = new List<ITradingStructure>();
            Random random = new Random();

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
                List<AverageResponse> responses = (List<AverageResponse>) averages.CreateMovingAverage(3, "close");

                Assert.AreEqual(7, responses.Count);
                Assert.AreEqual(DateTime.Now.AddDays(2).Date, responses[0].ActivityDate);

                float avg = (float) Math.Round( (stockHistory[0].GetFloatValue("Close") + stockHistory[1].GetFloatValue("Close") + stockHistory[2].GetFloatValue("Close")) / 3,2);
                Assert.AreEqual(avg, responses[0].Value);

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
                List<AverageResponse> responses = (List<AverageResponse>)averages.CreateMovingAverage(100, "close");

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
            float response = averages.CalculateSimpleAverage(3, "open");

            float avg = (float)Math.Round((stockHistory[0].GetFloatValue("Open") + stockHistory[1].GetFloatValue("Open") + stockHistory[2].GetFloatValue("Open")) / 3, 2);
            Assert.AreEqual(avg, response);
        }

        [Test]
        public void CheckAverageWithOffset()
        {
            float response = averages.CalculateSimpleAverage(3, "open", 2);

            float avg = (float)Math.Round((stockHistory[2].GetFloatValue("Open") + stockHistory[3].GetFloatValue("Open") + stockHistory[4].GetFloatValue("Open")) / 3, 2);
            Assert.AreEqual(avg, response);
        }

    }
}
