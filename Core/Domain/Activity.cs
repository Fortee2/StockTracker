using System;

namespace StockTracker.Core.Domain
{
    public class Activity:BaseObject
    {

        public Activity(int id, DateTime activityDate, decimal high, decimal low, decimal open, decimal close, int volume) : base()
        {
            Id = id;
            ActivityDate = activityDate;
            High = high;
            Low = low;
            Open = open;
            Close = close;
            Volume = volume;
        }

        //Properties
        //public int Id { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Open { get; set; }
        public decimal Close { get; set; }

        public int Volume { get; set; }
        //public DateTime ActivityDate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        //public object GetValue(string PropertyName)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
