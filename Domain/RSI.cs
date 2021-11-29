using System;
namespace Domain
{
    public class RSI:BaseObject
    {
        public RSI(DateTime ActivityDate, float Close)
        {
            this.ActivityDate = ActivityDate;
            this.Close = Close;
        }

        public float Close { get; set; }
        public float Gain { get; set; }
        public float Loss { get; set; }
        public float AvgGain { get; set; }
        public float AvgLoss { get; set; }
        public float RSIndex { get; set; }
    }
}
