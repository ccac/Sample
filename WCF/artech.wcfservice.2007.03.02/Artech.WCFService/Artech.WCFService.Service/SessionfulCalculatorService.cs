using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

using Artech.WCFService.Contract;

namespace Artech.WCFService.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class SessionfulCalculatorService:ISessionfulCalculator
    {
        private double _result;

        #region ISessionfulCalculator Members

        public void Add(double x)
        {
            Console.WriteLine("The Add method is invoked and the current session ID is: {0}", OperationContext.Current.SessionId);
            this._result += x;
        }

        public double GetResult()
        {
            Console.WriteLine("The GetResult method is invoked and the current session ID is: {0}", OperationContext.Current.SessionId);
            return this._result;
        }

        #endregion

        public SessionfulCalculatorService()
        {
            Console.WriteLine("Calculator object has been created");
        }

        ~SessionfulCalculatorService()
        {
            Console.WriteLine("Calculator object has been destoried");
        }

    }
}
