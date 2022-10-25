using System;
namespace StockTracker.Core.Interfaces.Calculations
{
    public interface IResponse
    {
        public DateTime ActivityDate {get; set;}

        public decimal GetDecimalValue(string propertyName);
        public void SetDecimalValue(string propertyName, decimal value);
    }
}
