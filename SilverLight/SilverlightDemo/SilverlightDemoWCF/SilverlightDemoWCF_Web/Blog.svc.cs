using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SilverlightDemoWCF_Web
{
    // NOTE: If you change the class name "Blog" here, you must also update the reference to "Blog" in Web.config.
    public class Blog : IBlog
    {

        #region IBlog Members

        public Post[] GetPosts()
        {
            //throw new NotImplementedException();
            List<Post> list = new List<Post>()
            {
                new Post(1,"一步一步学Silverlight 2系列（13）：数据与通信之WebRequest","TerryLee"),
                new Post(2,"一步一步学Silverlight 2系列（12）：数据与通信之WebClient","TerryLee"),
                new Post(3,"一步一步学Silverlight 2系列（11）：数据绑定","TerryLee"),
                new Post(4,"一步一步学Silverlight 2系列（10）：使用用户控件","TerryLee"),
                new Post(5,"一步一步学Silverlight 2系列（9）：使用控件模板","TerryLee"),
                new Post(6,"一步一步学Silverlight 2系列（8）：使用样式封装控件观感","TerryLee")

            };

            return list.ToArray() ;
        }

        #endregion
    }
}
