using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace WCF.QueuedService.Contract
{
    [DataContract]
    public class OrderItem
    {
        #region Private Fields
        private Guid _productID;
        private string _productName;
        private decimal _unitPrice;
        private int _quantity;
        #endregion

        #region Constructors
        public OrderItem()
        { }

        public OrderItem(Guid productID, string productName, decimal unitPrice, int quantity)
        {
            this._productID = productID;
            this._productName = productName;
            this._unitPrice = unitPrice;
            this._quantity = quantity;
        }
        #endregion

        #region Public Properties
        [DataMember]
        public Guid ProductID
        {
            get { return _productID; }
            set { _productID = value; }
        }

        [DataMember]
        public string ProductName
        {
            get { return _productName; }
            set { _productName = value; }
        }

        [DataMember]
        public decimal UnitPrice
        {
            get { return _unitPrice; }
            set { _unitPrice = value; }
        }

        [DataMember]
        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }
        #endregion
    }

}
