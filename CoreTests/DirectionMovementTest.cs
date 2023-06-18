using NUnit.Framework;
using StockTracker.Core.Calculations;
using StockTracker.Core.Calculations.Response;
using StockTracker.Core.Domain;
using System;
using System.Collections.Generic;

namespace StockTracker.Core.UnitTests.Calculations
{
    public class DirectionalMovementTests
    {
        [Test]
        public void Calculate_ReturnsListOfDirectionalResponseWithCorrectValues()
        {
            // Arrange
            var directionalMovementData = new List<DirectionalMovementData>
            {
                new DirectionalMovementData( DateTime.Now.Date, 100m,  50m,  90m, 40m),
                new DirectionalMovementData( DateTime.Now.Date.AddDays(-1), 110m, 60m, 100m, 50m)
            };
            var sut = new DirectionalMovement(directionalMovementData);

            // Act
            var result = sut.Calculate();

            // Assert
            Assert.AreEqual(result[0].ActivityDate, DateTime.Now.Date.AddDays(-1));
            Assert.AreEqual(result[0].PositiveDirectionalMovement, 60);
            Assert.AreEqual(result[0].NegativeDirectionalMovement, 10);
            Assert.AreEqual(result[1].ActivityDate, DateTime.Now.Date);
            Assert.AreEqual(result[1].PositiveDirectionalMovement, 50);
            Assert.AreEqual(result[1].NegativeDirectionalMovement, 10);
        }

    }
}
