using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Linq;
using System.ServiceModel.Web;
using System.Web;

namespace Silverlight20.Web.DataService
{
    public class NorthwindDataService : DataService<NorthwindEntities>
    {
        public static void InitializeService(IDataServiceConfiguration config)
        {
            config.SetEntitySetAccessRule("*", EntitySetRights.All);
        }
    }
}