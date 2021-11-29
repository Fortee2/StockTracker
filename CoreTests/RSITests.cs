using System;
using System.Collections;
using System.Collections.Generic;
using StockTracker.Core.Calculations;
using StockTracker.Core.Domain;
using NUnit.Framework;

namespace StockTracker.CoreTests
{
    public class RSITests
    {
        private IList<RSI> rsiList;
        private RealitiveStrengthIndex strengthIndex;

        public RSITests()
        {

        }

        [SetUp]
        public void Setup()
        {
            Random random = new ();

            rsiList = new List<RSI>();

            for(short i = 0; i < 20; i++)
            {
                rsiList.Add(
                    new RSI(
                        DateTime.Now.AddDays(i),
                        random.Next(1, 500)
                    )
                );
            }

            strengthIndex = new RealitiveStrengthIndex((IList)rsiList);
            strengthIndex.Calculate();
        }

        [Test]
        public void TestGainLoss()
        {
            float gl = rsiList[3].Close - rsiList[2].Close;

            if(gl == 0)
            {
                Assert.AreEqual((rsiList[3].Gain), rsiList[3].Loss);
                return;
            }

            if (gl < 0) 
            {
                Assert.AreEqual((float)Math.Abs(gl), rsiList[3].Loss);
                return;
            }

            Assert.AreEqual(gl, rsiList[3].Gain);

        }

        [Test]
        public void TestAverageGains()
        {
            float avgGain = (float)Math.Round(((rsiList[14].AvgGain * 13) + rsiList[15].Gain) / 14 , 2);

            Assert.AreEqual(avgGain, rsiList[15].AvgGain);
        }

        [Test]
        public void TestAverageLoss()
        {
            float avgLoss = (float)Math.Round(((rsiList[14].AvgLoss * 13) + rsiList[15].Loss) / 14, 2);

            Assert.AreEqual(avgLoss, rsiList[15].AvgLoss);
        }
    }
}
