/* 
 * 在 Silverlight 2.0 中调用数据服务只能使用异步方式调用
 * System.Data.Services.Client.DataServiceContext - 数据服务上下文
 * System.Data.Services.Client.DataServiceQuery - 以指定的 URI 语法查询数据服务
 * AddObject(), UpdateObject(), DeleteObject() - 本别用于添加, 更新, 删除实体
 * BeginExecute()/EndExecute(), BeginExecuteBatch()/EndExecuteBatch - 用于执行某一个 DataServiceQuery 查询或批量执行（将一组查询一次性地提交到数据服务）
 * BeginSaveChanges()/EndSaveChanges() - 用于提交对实体的修改（增,删,改）
 * BeginLoadProperty()/EndLoadProperty() - 用于加载指定的属性的值，加载导航属性的时候需要用到它
 * AddLink(), SetLink(), DeleteLink() - 分别为创建连接，Added状态（一对多）；创建连接，Added状态（多对一）；删除连接，Deleted状态
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using System.Data.Services.Client;
using System.Collections.ObjectModel;
using Silverlight20.NorthwindDataService;

namespace Silverlight20.Communication
{
    public partial class DataService : UserControl
    {
        // 配置服务地址，数据服务要与 Silverlight 宿主放在相同的域上
        Uri uri = new Uri("DataService/NorthwindDataService.svc", UriKind.Relative);
        NorthwindEntities ctx;
        ObservableCollection<Categories> categories;
        ObservableCollection<Products> products;

        public DataService()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(DataService_Loaded);
        }

        void DataService_Loaded(object sender, RoutedEventArgs e)
        {
            // 实例化 DataServiceContext
            ctx = new NorthwindEntities(uri);
            // 初始化 Categories 集合，为了做 OneWay ，所以是 ObservableCollection<Categories> 类型
            categories = new ObservableCollection<Categories>();
            // 初始化 Products 集合，为了做 OneWay ，所以是 ObservableCollection<Products> 类型
            products = new ObservableCollection<Products>();

            BindCategory();
        }

        private void BindCategory()
        {
            DataServiceQuery<Categories> query = ctx.Categories;

            // IAsyncResult BeginExecute(AsyncCallback callback, object state) - 以异步方式发出请求
            //     AsyncCallback callback - 经典的 AsyncCallback 委托，指定回调方法
            //     object state - 传递给回调方法的自定义对象，此处必须是 DataServiceQuery<T> 类型
            query.BeginExecute(OnBindCategoryCompleted, query);
            // RequestUri - 请求服务的地址，因为数据服务发布的是REST，所以也可以用自己构造 URI 的方式去调用数据服务，详细的 URI 语法请参看 MSDN
            lblMsg.Text = "读取类别数据中。。。" + query.RequestUri.ToString();
        }

        void OnBindCategoryCompleted(IAsyncResult ar)
        {
            try
            {
                var query = ar.AsyncState as DataServiceQuery<Categories>;

                // EndExecute(IAsyncResult ar) - 获取异步查询的结果
                var result = query.EndExecute(ar);

                foreach (var item in result)
                {
                    categories.Add(item);
                }

                this.Dispatcher.BeginInvoke(() =>
                {
                    dataGrid1.DataContext = categories;
                    lblMsg.Text = "";
                });
            }
            catch (DataServiceRequestException ex)
            {
                lblMsg.Text = ex.ToString();
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Categories category = new Categories();
            category.CategoryName = txtCategoryName.Text;
            category.Description = txtDescription.Text;

            ctx.AddToCategories(category);

            for (int i = 0; i < 10; i++)
            {
                var product = new Products() { ProductName = "测试用" + i.ToString() };
                product.Categories = category;
                ctx.AddToProducts(product);
                // 多对一关系，使用 SetLink 建立连接，BeginSaveChanges() 的时候会一起发送到数据服务
                ctx.SetLink(product, "Categories", category);
            }

            ctx.BeginSaveChanges(OnAddCompleted, category);
            lblMsg.Text = "新增数据中。。。";
        }

        void OnAddCompleted(IAsyncResult ar)
        {
            try
            {
                var x = ctx.EndSaveChanges(ar);

                categories.Add(ar.AsyncState as Categories);

                this.Dispatcher.BeginInvoke(() =>
                {
                    lblMsg.Text = "";
                });
            }
            catch (DataServiceRequestException ex)
            {
                lblMsg.Text = ex.ToString();
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var category = e.AddedItems[0] as Categories;
            BindProduct(category.CategoryID);
        }

        private void BindProduct(int categoryId)
        {
            // 可以使用 Lambda 表达式或查询语法，然后将其转换为 DataServiceQuery<T> 再使用
            DataServiceQuery<Products> query =
                (from p in ctx.Products where p.Categories.CategoryID == categoryId select p) as DataServiceQuery<Products>;

            lblMsg.Text = "读取产品数据中。。。";
            query.BeginExecute(OnBindProductCompleted, query);
        }

        void OnBindProductCompleted(IAsyncResult ar)
        {
            try
            {
                var query = ar.AsyncState as DataServiceQuery<Products>;

                var result = query.EndExecute(ar);
                products.Clear();
                foreach (var item in result)
                {
                    products.Add(item);
                }

                this.Dispatcher.BeginInvoke(() =>
                {
                    dataGrid2.DataContext = products;
                    lblMsg.Text = "";
                });
            }
            catch (DataServiceRequestException ex)
            {
                lblMsg.Text = ex.ToString();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid1.SelectedItem != null)
            {
                try
                {
                    Categories category = dataGrid1.SelectedItem as Categories;

                    DeleteCategory(category);
                    lblMsg.Text = "删除中。。。";
                }
                catch (DataServiceRequestException ex)
                {
                    lblMsg.Text = ex.ToString();
                }
            }
        }

        private void DeleteCategory(Categories category)
        {
            try
            {
                // BeginLoadProperty(object entity, string propertyName, AsyncCallback callback, object state) - 开始加载指定属性的值的异步操作
                //     object entity - 需要加载属性的所属实体 
                //     string propertyName - 需要加载属性的名称
                //     AsyncCallback callback - 经典的 AsyncCallback 委托，指定回调方法
                //     object state - 传递给回调方法的自定义对象
                ctx.BeginLoadProperty(category, "Products", OnLoadPropertyCompleted, category);
            }
            catch (DataServiceRequestException ex)
            {
                lblMsg.Text = ex.ToString();
            }
        }

        void OnLoadPropertyCompleted(IAsyncResult ar)
        {
            Categories category = ar.AsyncState as Categories;

            try
            {
                // EndLoadProperty(IAsyncResult ar) - 完成加载指定属性的值的这个异步操作
                ctx.EndLoadProperty(ar);

                foreach (Products product in category.Products)
                {
                    // 在指定的对象上删除指定的连接，BeginSaveChanges() 的时候会一起发送到数据服务
                    ctx.DeleteLink(category, "Products", product);
                }

                ctx.DeleteObject(category);
                ctx.BeginSaveChanges(OnDeleteCategoryCompleted, null);

                categories.Remove(category);
            }
            catch (DataServiceRequestException ex)
            {
                lblMsg.Text = ex.ToString();
            }
        }

        void OnDeleteCategoryCompleted(IAsyncResult ar)
        {
            try
            {
                ctx.EndSaveChanges(ar);
                lblMsg.Text = "";

            }
            catch (DataServiceRequestException ex)
            {
                lblMsg.Text = ex.ToString();
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid1.SelectedItem != null)
            {
                try
                {
                    Categories category = dataGrid1.SelectedItem as Categories;

                    ctx.UpdateObject(category);
                    ctx.BeginSaveChanges(OnUpdateCategoryCompleted, category);
                    lblMsg.Text = "更新中。。。";
                }
                catch (DataServiceRequestException ex)
                {
                    lblMsg.Text = ex.ToString();
                }
            }
        }

        void OnUpdateCategoryCompleted(IAsyncResult ar)
        {
            try
            {
                ctx.EndSaveChanges(ar);
                lblMsg.Text = "";

            }
            catch (DataServiceRequestException ex)
            {
                lblMsg.Text = ex.ToString();
            }
        }
    }
}