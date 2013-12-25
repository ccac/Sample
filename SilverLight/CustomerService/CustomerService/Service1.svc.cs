using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CustomerService
{
    // NOTE: If you change the class name "Service1" here, you must also update the reference to "Service1" in Web.config and in the associated .svc file.
    public class Service1 : IService1
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        #region IService1 Members


        public int CountUsers()
        {
            //throw new NotImplementedException();
            return 2;
        }

        public User GetUser(int id)
        {
            //throw new NotImplementedException();
            if (id == 1)
            {
                return new User() { IsMember = true, Name = "Paul", Age = 24 };
            }
            else
            {
                return new User() { IsMember = false, Name = "John", Age = 64 };
            }

        }

        #endregion
    }
}
