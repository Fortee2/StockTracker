using System;
using System.Collections.Generic;
using StockTracker.Core.Domain;
using StockTracker.Core.Calculations;
using NUnit.Framework;
using StockTracker.Core.Calculations.Response;
using StockTracker.Core.Domain.Interfaces;

namespace StockTracker.CoreTests
{
    public class ExponetialAverageTests
    {
        private IList<ITradingStructure> stockHistory;
        private Averages averages;

        [SetUp]
        public void Setup() {

            //Make up some test data
            stockHistory = new List<ITradingStructure>();
            Random random = new Random();


            stockHistory.Add(
                new Activity(1,
                    new DateTime(2021,11,15,0,0,0),
                    (float)80.4008,
                    (float)76.8488,
                    (float)80.3913,
                    (float)77.2107,
                    111000
                )
            );


            stockHistory.Add(
                new Activity(1,
                    new DateTime(2021, 11, 16, 0, 0, 0),
                    (float)80.4008,
                    (float)76.8488,
                    (float)80.3913,
                    (float)77.6964,
                    111000
                )
            );


            stockHistory.Add(
                new Activity(1,
                    new DateTime(2021, 11, 17, 0, 0, 0),
                    (float)80.4008,
                    (float)76.8488,
                    (float)80.3913,
                    (float)78.8296,
                    111000
                )
            );


            stockHistory.Add(
                new Activity(1,
                    new DateTime(2021, 11, 18, 0, 0, 0),
                    (float)80.4008,
                    (float)76.8488,
                    (float)80.3913,
                    (float)80.1437,
                    111000
                )
            );


            stockHistory.Add(
                new Activity(1,
                    new DateTime(2021, 11, 19, 0, 0, 0),
                    (float)80.4008,
                    (float)76.8488,
                    (float)80.3913,
                    (float)81.1627,
                    111000
                )
            );


            stockHistory.Add(
                new Activity(1,
                    new DateTime(2021, 11, 20, 0, 0, 0),
                    (float)80.4008,
                    (float)76.8488,
                    (float)80.3913,
                    (float)81.4959,
                    111000
                )
            );


            stockHistory.Add(
                new Activity(1,
                    new DateTime(2021, 11, 21, 0, 0, 0),
                    (float)80.4008,
                    (float)76.8488,
                    (float)80.3913,
                    (float)81.2769,
                    111000
                )
            );


            stockHistory.Add(
                new Activity(1,
                    new DateTime(2021, 11, 22, 0, 0, 0),
                    (float)80.4008,
                    (float)76.8488,
                    (float)80.3913,
                    (float)80.6464,
                    111000
                )
            );


            stockHistory.Add(
                new Activity(1,
                    new DateTime(2021, 11, 23, 0, 0, 0),
                    (float)80.4008,
                    (float)76.8488,
                    (float)80.3913,
                    (float)82.7053,
                    111000
                )
            );


            stockHistory.Add(
                new Activity(1,
                    new DateTime(2021, 11, 24, 0, 0, 0),
                    (float)80.4008,
                    (float)76.8488,
                    (float)80.3913,
                    (float)82.2578,
                    111000
                )
            );
            //Intialize our test class
            averages = new(stockHistory);
        }

        [Test]
        public void CheckExponetialMovingkAverageCal()
        {
            try
            {
                List<AverageResponse> responses = (List<AverageResponse>)averages.CalculateExponentialMovingAverage(4, "close", "nan");

                Assert.AreEqual(6, responses.Count);
                Assert.AreEqual(new DateTime(2021, 11, 19, 0, 0, 0), responses[0].ActivityDate);
                Assert.AreEqual((float)79.547081, responses[0].Value);

            }
            catch(Exception e )
            { 
                Assert.Fail(e.Message);
            }
        }

    }
}
