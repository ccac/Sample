using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WCFSerialization
{
    public class XMLOrder
    {
        private Guid _orderID;
        private DateTime _orderDate;
        private XMLProduct _product;
        private int _quantity;

        #region Constructors
        public XMLOrder()
        {
            this._orderID = new Guid();
            this._orderDate = DateTime.MinValue;
            this._quantity = int.MinValue;

            Console.WriteLine("The constructor of XMLOrder has been invocated!");
        }

        public XMLOrder(Guid id, DateTime date, XMLProduct product, int quantity)
        {
            this._orderID = id;
            this._orderDate = date;
            this._product = product;
            this._quantity = quantity;
        }
        #endregion

        #region Properties
        public Guid OrderID
        {
            get { return _orderID; }
            set { _orderID = value; }
        }

        public DateTime OrderDate
        {
            get { return _orderDate; }
            set { _orderDate = value; }
        }

        public XMLProduct Product
        {
            get { return _product; }
            set { _product = value; }
        }

        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }
        #endregion

        public override string ToString()
        {
            return string.Format("ID: {0}\nDate:{1}\nProduct:\n\tID:{2}\n\tName:{3}\n\tProducing Area:{4}\n\tPrice:{5}\nQuantity:{6}",
                this._orderID,this._orderDate,this._product.ProductID,this._product.ProductName,this._product.ProducingArea,this._product.UnitPrice,this._quantity);
        }
    }

}
