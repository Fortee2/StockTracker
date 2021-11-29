using System;
using Domain.Interfaces;

namespace Domain
{
    public class Activity:BaseObject
    {

        public Activity(int id, DateTime activityDate, float high, float low, float open, float close, int volume) : base()
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
        public float High { get; set; }
        public float Low { get; set; }
        public float Open { get; set; }
        public float Close { get; set; }

        public int Volume { get; set; }
        //public DateTime ActivityDate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        //public object GetValue(string PropertyName)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
