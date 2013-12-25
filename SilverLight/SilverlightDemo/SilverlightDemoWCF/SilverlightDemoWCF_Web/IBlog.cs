using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SilverlightDemoWCF_Web
{
    // NOTE: If you change the interface name "IBlog" here, you must also update the reference to "IBlog" in Web.config.
    [ServiceContract]
    public interface IBlog
    {
        //[OperationContract]
        //void DoWork();

        [OperationContract]
        Post[] GetPosts();
    }
}
