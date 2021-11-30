using System;
namespace StockTracker.Core.Interfaces.Calculations
{
    public interface IResponse
    {
        public DateTime ActivityDate {get; set;}

        public float GetFloatValue(string propertyName);
        public void SetFloatValue(string propertyName, float value);
    }
}
