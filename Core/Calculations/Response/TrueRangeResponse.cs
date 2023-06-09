using System;
using StockTracker.Core.Calculations.Response;

public class TrueRangeResponse:BaseResponse{
    public TrueRangeResponse(DateTime date, decimal trueRangeValue){
        ActivityDate = date;
        TrueRange = trueRangeValue;
    }
    public TrueRangeResponse(){}
    public decimal TrueRange { get; set; }
}