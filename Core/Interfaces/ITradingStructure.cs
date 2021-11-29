using System;
namespace StockTracker.Core.Domain.Interfaces
{
    public interface ITradingStructure
    {
        //This property is defined in the interface because it is need in the
        //caluclation libraries
        DateTime ActivityDate { get; set; }

        object GetValue(string PropertyName);

        float GetFloatValue(string PropertyName);
    }
}
