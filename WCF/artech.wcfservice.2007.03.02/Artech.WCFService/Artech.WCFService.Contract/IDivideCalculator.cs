using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace Artech.WCFService.Contract
{
    [ServiceContract]
    public interface IDivideCalculator
    {
        [OperationContract]
        [FaultContract(typeof(MathError))]
        double Divide(double x, double y);
    }
}
