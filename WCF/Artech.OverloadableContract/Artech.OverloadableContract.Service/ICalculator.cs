using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace Artech.OverloadableContract.Service
{
    [ServiceContract]
   public interface ICalculator
    {
        [OperationContract(Name = "AddWithTwoOperands")]
       double Add(double x, double y);

       [OperationContract(Name = "AddWithThreeOperands")]
       double Add(double x, double y, double z);
    }
}
