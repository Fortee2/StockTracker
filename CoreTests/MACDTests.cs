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
    public class MACDTests
    {
        private List<ITradingStructure> stockHistory;
        private MACD averages;

        [SetUp]
        public void Setup() {

            //Make up some test data
            stockHistory = new List<ITradingStructure>
            {
                new MACDData(DateTime.Parse("2021-11-19"), (decimal)343.1100, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-11-18"), (decimal)341.2700, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-11-17"), (decimal)339.1200, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-11-16"), (decimal)338.8900, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-11-15"), (decimal)335.4563, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-11-12"), (decimal)336.7200, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-11-11"), (decimal)332.4300, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-11-10"), (decimal)330.8000, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-11-09"), (decimal)335.9500, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-11-08"), (decimal)336.9900, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-11-05"), (decimal)336.0600, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-11-04"), (decimal)336.4400, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-11-03"), (decimal)334.0000, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-11-02"), (decimal)333.1300, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-11-01"), (decimal)329.3700, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-10-29"), (decimal)331.6200, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-10-28"), (decimal)324.3500, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-10-27"), (decimal)323.1700, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-10-26"), (decimal)310.1100, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-10-25"), (decimal)308.1300, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-10-22"), (decimal)309.1600, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-10-21"), (decimal)310.7600, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-10-20"), (decimal)307.4100, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-10-19"), (decimal)308.2300, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-10-18"), (decimal)307.2900, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-10-15"), (decimal)304.2100, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-10-14"), (decimal)302.7500, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-10-13"), (decimal)296.3100, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-10-12"), (decimal)292.8800, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-10-11"), (decimal)294.2300, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-10-08"), (decimal)294.8500, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-10-07"), (decimal)294.8500, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-10-06"), (decimal)293.1100, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-10-05"), (decimal)288.7600, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-10-04"), (decimal)283.1100, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-10-01"), (decimal)289.1000, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-09-30"), (decimal)281.9200, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-09-29"), (decimal)284.0000, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-09-28"), (decimal)283.5200, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-09-27"), (decimal)294.1700, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-09-24"), (decimal)299.3500, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-09-23"), (decimal)299.5600, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-09-22"), (decimal)298.5800, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-09-21"), (decimal)294.8000, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-09-20"), (decimal)294.3000, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-09-17"), (decimal)299.8700, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-09-16"), (decimal)305.2200, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-09-15"), (decimal)304.8200, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-09-14"), (decimal)299.7900, 0, 0, 0, 0),
                new MACDData(DateTime.Parse("2021-09-13"), (decimal)296.9900, 0, 0, 0, 0)
            };
            //Intialize our test class
            averages = new(stockHistory);

            #region Set calculation columns
            averages.ClosingPriceColumn = "Close";
            averages.MACDColumn = "MACD";
            averages.SignalColumn = "Signal";
            averages.EMA12Column = "Previous12EMA";
            averages.EMA26Column = "Previous26EMA";
            #endregion
        }

        [Test]
        public void CheckExponetialMovingkAverageCal()
        {
            try
            {
                List<IResponse> responses = averages.Calculate();

                MacdResponse macdResponse = (MacdResponse)responses[49];

                Assert.AreEqual(Math.Round(macdResponse.MACD, 2), 9.05);
                Assert.AreEqual(Math.Round(macdResponse.Signal, 2), 9.31);
                Assert.AreEqual(Math.Round(macdResponse.Previous12EMA, 2), 335.98);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

    }
}
