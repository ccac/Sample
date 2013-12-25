using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Artech.WCFService.Contract
{
    [ServiceContract]
    public interface ICalculator
    {
        [OperationContract]
        double Add(double x, double y);
    }
}
