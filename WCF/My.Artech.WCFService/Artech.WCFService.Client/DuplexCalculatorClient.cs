using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Artech.WCFService.Contract;

namespace Artech.WCFService.Client
{
    class DuplexCalculatorClient:ClientBase<IDuplexCalculator>,IDuplexCalculator
    {
        internal DuplexCalculatorClient(InstanceContext callbackContext,string endpointConfigName)
            :base(callbackContext,endpointConfigName){}

        #region IDuplexCalculator Members

        public void Add(double x, double y)
        {
            //throw new NotImplementedException();
            this.Channel.Add(x, y);
        }

        #endregion
    }
}
