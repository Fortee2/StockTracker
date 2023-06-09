using System;
using System.Collections;
using System.Collections.Generic;
using StockTracker.Core.Calculations;
using StockTracker.Core.Domain;
using NUnit.Framework;

namespace StockTracker.CoreTests
{
    public class TrueRangeTest
    {
        List<Quote> quotes;

        [SetUp]
        public void Setup()
        {
            quotes = new List<Quote>{
                new Quote (DateTime.Now, 50, 10, 0, 20, 100),
                new Quote (DateTime.Now.AddDays(-1), 30, 10, 0, 25, 100),
                new Quote (DateTime.Now.AddDays(-2), 100, 90, 0, 95, 100)
            };
        }

        [Test]
        public void Calculate_ReturnsAccurateTrueRangeResponseList(){

            var tr = new TrueRange(quotes);
        
            //Act
            var result = tr.Calculate();
        
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count,3);
            Assert.AreEqual(result[0].TrueRange,40);
            Assert.AreEqual(result[1].TrueRange,20);
            Assert.AreEqual(result[2].TrueRange,10);
        }
        [Test]
        public void Calculate_GivenEmptyQuoteList_ReturnsEmptyTrueRangeResponseList(){
            //Arrange
            List<Quote> quotes = new List<Quote>();
            var tr = new TrueRange(quotes);
        
            //Act
            var result = tr.Calculate();
        
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count,0);
        }
        [Test]
        public void Calculate_GivenNullQuoteList_ThrowsArgumentNullException(){
            //Arrange/Act
            var ex = Assert.Throws<ArgumentNullException>(() => new TrueRange(null).Calculate());
        
            //Assert
            Assert.AreEqual(ex.Message,"Value cannot be null. (Parameter 'quotes')");
        }
        [Test]
        public void Calculate_GivenInvalidQuoteData_ThrowsException(){
            //Arrange
            List<Quote> quotes = new List<Quote>{
                new Quote (DateTime.Now, -50, 10, 0, 20, 100),
                new Quote (DateTime.Now.AddDays(-1), 30, 40, 0, 25, 100),
                new Quote (DateTime.Now.AddDays(-2), 100, 95, 0, -80, 100)
            };
            var tr = new TrueRange(quotes);
        
            //Act/Assert
            var ex = Assert.Throws<Exception>(() => tr.Calculate());
            Assert.AreEqual(ex.Message,"Invalid quote data");
        }
        
    }
}