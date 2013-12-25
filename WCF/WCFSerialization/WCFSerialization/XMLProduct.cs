using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WCFSerialization
{
    public class XMLProduct
    {
        #region Private Fields
        private Guid _productID;
        private string _productName;
        private string _producingArea;
        private double _unitPrice;
        #endregion

        #region Constructors
        public XMLProduct()
        {
            Console.WriteLine("The constructor of XMLProduct has been invocated!");
        }

        public XMLProduct(Guid id, string name, string producingArea, double price)
        {
            this._productID = id;
            this._productName = name;
            this._producingArea = producingArea;
            this._unitPrice = price;
        }

        #endregion

        #region Properties
        public Guid ProductID
        {
            get { return _productID; }
            set { _productID = value; }
        }

        public string ProductName
        {
            get { return _productName; }
            set { _productName = value; }
        }

        internal string ProducingArea
        {
            get { return _producingArea; }
            set { _producingArea = value; }
        }

        public double UnitPrice
        {
            get { return _unitPrice; }
            set { _unitPrice = value; }
        }

       #endregion

    }

}
