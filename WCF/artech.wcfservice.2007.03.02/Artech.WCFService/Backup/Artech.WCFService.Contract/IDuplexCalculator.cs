using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace Artech.WCFService.Contract
{
     [ServiceContract(CallbackContract = typeof(ICalculatorCallback))]
    public interface IDuplexCalculator
    {
         [OperationContract]
        void Add(double x, double y);
    }
}