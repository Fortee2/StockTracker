using System;
using NUnit.Framework;
using StockTracker.Core.Analyzers;
using StockTracker.Core.Domain;
using StockTracker.Core.Interfaces;

namespace StockTracker.Core.Tests.Analyzers
{
    [TestFixture]
    public class HammerTests
    {
        [Test]
        public void IsHammerPattern_ReturnsTrue_WhenGivenValidTradingStructure()
        {
            // Arrange
            Quote tradingStructure = new Quote( 
                0,
                DateTime.Now,
                100m,
                90m,
                98m,
                99m,
                1000000
            );

            // Act
            bool isHammer = Hammer.IsHammerPattern(tradingStructure);

            // Assert
            Assert.That(isHammer, Is.True);
        }

        [Test]
        public void IsHammerPattern_ReturnsFalse_WhenBodySizeGreaterThan30PercentOfShadowSize()
        {
            // Arrange
            Quote tradingStructure = new Quote( 
                0,
                DateTime.Now,
                20m,
                8m,
                10m,
                14m,
                1000000
            );

            // Act
            bool isHammer = Hammer.IsHammerPattern(tradingStructure);

            // Assert
            Assert.That(isHammer, Is.False);
        }

        [Test]
        public void IsHammerPattern_ReturnsFalse_WhenBodySizeGreaterThan10PercentOfHighMinusLow()
        {
            // Arrange
            Quote tradingStructure = new Quote
            (
                0,
                DateTime.Now,
                20m,
                5m,
                10m,
                12m,
                1000000
            );

            // Act
            bool isHammer = Hammer.IsHammerPattern(tradingStructure);

            // Assert
            Assert.That(isHammer, Is.False);
        }

        [Test]
        public void IsHammerPattern_ReturnsFalse_WhenCloseNotGreaterThanOpen()
        {
            // Arrange
            Quote tradingStructure = new Quote
            (
                0,
                DateTime.Now,
                20m,
                5m,
                15m,
                10m,
                1000000
            );

            // Act
            bool isHammer = Hammer.IsHammerPattern(tradingStructure);

            // Assert
            Assert.That(isHammer, Is.False);
        }

        [Test]
        public void IsHammerPattern_ReturnsFalse_WhenCloseMinusLowGreaterThan25PercentOfHighMinusLow()
        {
            // Arrange
            Quote tradingStructure = new Quote
            (
                0,

                DateTime.Now,
                20m,
                5m,
                10m,
                11m,
                1000000
            );

            // Act
            bool isHammer = Hammer.IsHammerPattern(tradingStructure);

            // Assert
            Assert.That(isHammer, Is.False);
        }
    }
}
