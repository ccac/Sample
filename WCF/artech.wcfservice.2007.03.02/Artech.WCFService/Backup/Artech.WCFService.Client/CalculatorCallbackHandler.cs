using System;
using System.Collections.Generic;
using System.Text;
using Artech.WCFService.Contract;

namespace Artech.WCFService.Client
{
    class CalculatorCallbackHandler:ICalculatorCallback
    {
        #region ICalculatorCallback Members

        public void ShowResult(double x, double y, double result)
        {
            Console.WriteLine("x + y = {2} where x = {0} and y = {1}", x, y, result);
        }

        #endregion
    }
}
