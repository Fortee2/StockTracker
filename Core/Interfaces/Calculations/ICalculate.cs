using System;
using System.Collections.Generic;

namespace StockTracker.Core.Interfaces.Calculations
{
    public interface ICalculate
    {

        public List<IResponse> Calculate();
        
    }


}
