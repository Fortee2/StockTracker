using System;
namespace StockTracker.Core.Domain
{
    public class MacdInput:BaseObject
    {
        public MacdInput():base()
        {
        }

        public float Close { get; set; }
        public float Previous12EMA { get; set; }
        public float Previous26EMA { get; set; }
        public float Previous9EMA { get; set; }
    }
}
