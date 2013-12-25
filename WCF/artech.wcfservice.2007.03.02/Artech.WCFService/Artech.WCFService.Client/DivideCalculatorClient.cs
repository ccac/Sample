using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using Artech.WCFService.Contract;

namespace Artech.WCFService.Client
{
    public class DivideCalculatorClient : ClientBase<IDivideCalculator>, IDivideCalculator
    {
        #region IDivideCalculator Members

        public double Divide(double x, double y)
        {
            //throw new NotImplementedException();
            return this.Channel.Divide(x, y);
        }

        #endregion
    }
}
