using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using WCF.QueuedService.Contract;

namespace WCF.QueuedService.Service
{
    public class OrderProcessService:IOrderProcess
    {
        #region IOrderProcess Members

        [OperationBehavior(TransactionAutoComplete=true,TransactionScopeRequired=true)]
        public void Submit(Order order)
        {
            //throw new NotImplementedException();
            Orders.Add(order);
            Console.WriteLine("Receive an Order ");
            Console.WriteLine(order);
        }

        #endregion
    }
}
