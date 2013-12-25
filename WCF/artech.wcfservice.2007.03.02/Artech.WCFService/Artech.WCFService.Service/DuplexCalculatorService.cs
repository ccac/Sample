using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using Artech.WCFService.Contract;

namespace Artech.WCFService.Service
{
  
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class DuplexCalculatorService:IDuplexCalculator
    {
        #region IDuplexCalculator Members

        public void Add(double x, double y)
        {
            double result = x + y;
            ICalculatorCallback callback = OperationContext.Current.GetCallbackChannel<ICalculatorCallback>();
            callback.ShowResult(x, y, result);
        }

        #endregion
    }
}
