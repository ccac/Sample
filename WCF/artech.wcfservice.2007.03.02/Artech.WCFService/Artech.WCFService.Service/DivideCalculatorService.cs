using System;
using System.Collections.Generic;
using System.Text;
using Artech.WCFService.Contract;
using System.ServiceModel;

namespace Artech.WCFService.Service
{
    public class DivideCalculatorService:IDivideCalculator
    {
        #region IDivideCalculator Members

        public double Divide(double x, double y)
        {
            //throw new NotImplementedException();
            if (y == 0)
            {
                //throw new DivideByZeroException("Divide by Zero");
                MathError error = new MathError("Divide", "Divided by zero");
                throw new FaultException<MathError>(error, new FaultReason("Parameters passed are not valid"), new FaultCode("sender"));
            }

            return x / y;
        }

        #endregion
    }
}
