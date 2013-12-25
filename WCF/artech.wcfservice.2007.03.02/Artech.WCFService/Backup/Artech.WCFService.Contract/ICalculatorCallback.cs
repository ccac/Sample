using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace Artech.WCFService.Contract
{
    [ServiceContract]
    public interface ICalculatorCallback
    {
        [OperationContract]
        void ShowResult(double x, double y, double result);
    }
}
