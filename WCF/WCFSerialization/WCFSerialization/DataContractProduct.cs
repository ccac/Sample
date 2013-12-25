using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace WCFSerialization
{
    [DataContract(Name="Product",Namespace="http://WCFSerialization/Samples/Product")]
    public class DataContractProduct
    {
        private Guid _productID;
        private string _productName;
        private string _producingArea;
        private double _unitPrice;

        #region Constructors
        public DataContractProduct()
        {
            Console.WriteLine("The constructor of DataContractProduct has been invocated!");
        }

        public DataContractProduct(Guid id, string name, string producingArea, double price)
        {
            this._productID = id;
            this._productName = name;
            this._producingArea = producingArea;
            this._unitPrice = price;
        }

        #endregion


        #region Properties
        [DataMember(Name="id",Order=0)]
        public Guid ProductID
        {
            get { return _productID; }
            set { _productID = value; }
        }

        [DataMember(Name = "name", Order = 1)]
        public string ProductName
        {
            get { return _productName; }
            set { _productName = value; }
        }

        [DataMember(Name = "area", Order = 2)]
        public string ProducingArea
        {
            get { return _producingArea; }
            set { _producingArea = value; }
        }

        [DataMember(Name = "price", Order = 3)]
        public double UnitPrice
        {
            get { return _unitPrice; }
            set { _unitPrice = value; }
        }

        #endregion


    }
}
