using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using Artech.WCFService.Contract;

namespace Artech.WCFService.Service
{
    public class TcpDuplexCalculatorService:ITcpDuplexCalculator
    {
        #region ITcpDuplexCalculator Members

        public void Add(double x, double y)
        {
            //throw new NotImplementedException();
            double result = x + y;
            ICallBack callback = OperationContext.Current.GetCallbackChannel<ICallBack>();
            callback.DisplayResult(result);
        }

        #endregion
    }
}
