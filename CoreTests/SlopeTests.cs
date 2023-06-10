using NUnit.Framework;
using System;
using System.Collections.Generic;
using StockTracker.Core.Calculations;
using StockTracker.Core.Domain;

namespace StockTracker.Core.Tests
{
    [TestFixture]
    public class SlopeTests
    {
        private readonly decimal delta = 0.0001m;

        [Test]
        public void TestCalculate_WithIncreasingPrices_ReturnsPositiveSlope()
        {
            // Arrange
            var slopeData = new List<SlopeData>()
            {
                new SlopeData()
                {
                    ActivityDate = new DateTime(2021, 1, 1),
                    Price = 100.0m
                },
                new SlopeData()
                {
                    ActivityDate = new DateTime(2021, 2, 1),
                    Price = 110.0m
                },
                new SlopeData()
                {
                    ActivityDate = new DateTime(2021, 3, 1),
                    Price = 120.0m
                }
            };
            var slope = new Slope(slopeData);

            // Act
            decimal result = slope.Calculate();

            // Assert
            Assert.That(result, Is.GreaterThan(0));
            Assert.That(result, Is.EqualTo(10.0m).Within(delta));
        }

        [Test]
        public void TestCalculate_WithDecreasingPrices_ReturnsNegativeSlope()
        {
            // Arrange
            var slopeData = new List<SlopeData>()
            {
                new SlopeData()
                {
                    ActivityDate = new DateTime(2021, 1, 1),
                    Price = 120.0m
                },
                new SlopeData()
                {
                    ActivityDate = new DateTime(2021, 2, 1),
                    Price = 110.0m
                },
                new SlopeData()
                {
                    ActivityDate = new DateTime(2021, 3, 1),
                    Price = 100.0m
                }
            };
            var slope = new Slope(slopeData);

            // Act
            decimal result = slope.Calculate();

            // Assert
            Assert.That(result, Is.LessThan(0));
            Assert.That(result, Is.EqualTo(-10.0m).Within(delta));
        }

        [Test]
        public void TestCalculate_WithConstantPrices_ReturnsZeroSlope()
        {
            // Arrange
            var slopeData = new List<SlopeData>()
            {
                new SlopeData()
                {
                    ActivityDate = new DateTime(2021, 1, 1),
                    Price = 100.0m
                },
                new SlopeData()
                {
                    ActivityDate = new DateTime(2021, 2, 1),
                    Price = 100.0m
                },
                new SlopeData()
                {
                    ActivityDate = new DateTime(2021, 3, 1),
                    Price = 100.0m
                }
            };
            var slope = new Slope(slopeData);

            // Act
            decimal result = slope.Calculate();

            // Assert
            Assert.That(result, Is.EqualTo(0).Within(delta));
        }
    }
}