using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace WCF.QueuedService.Contract
{
    [DataContract]
    [KnownType(typeof(OrderItem))]
    public class Order
    {
        #region Private Fields
        private Guid _orderNo;
        private DateTime _orderDate;
        private Guid _supplierID;
        private string _supplierName;
        private IList<OrderItem> _orderItems;
        #endregion

        #region Constructors
        public Order()
        {
            this._orderItems = new List<OrderItem>();
        }

        public Order(Guid orderNo, DateTime orderDate, Guid supplierID, string supplierName)
        {
            this._orderNo = orderNo;
            this._orderDate = orderDate;
            this._supplierID = supplierID;
            this._supplierName = supplierName;

            this._orderItems = new List<OrderItem>();
        }

        #endregion

        #region Public Properties
        [DataMember]
        public Guid OrderNo
        {
            get { return _orderNo; }
            set { _orderNo = value; }
        }

        [DataMember]
        public DateTime OrderDate
        {
            get { return _orderDate; }
            set { _orderDate = value; }
        }

        [DataMember]
        public Guid SupplierID
        {
            get { return _supplierID; }
            set { _supplierID = value; }
        }

        [DataMember]
        public string SupplierName
        {
            get { return _supplierName; }
            set { _supplierName = value; }
        }

        [DataMember]
        public IList<OrderItem> OrderItems
        {
            get { return _orderItems; }
            set { _orderItems = value; }
        }

        #endregion

        #region Public Methods
        public override string ToString()
        {
            string description = string.Format("General Informaion:\n\tOrder No.\t: {0}\n\tOrder Date\t: {1}\n\tSupplier No.\t: {2}\n\tSupplier Name\t: {3}", this._orderNo, this._orderDate.ToString("yyyy/MM/dd"), this._supplierID, this._supplierName);
            StringBuilder productList = new StringBuilder();
            productList.AppendLine("\nProducts:");

            int index = 0;
            foreach (OrderItem item in this._orderItems)
            {
                productList.AppendLine(string.Format("\n{4}. \tNo.\t\t: {0}\n\tName\t\t: {1}\n\tPrice\t\t: {2}\n\tQuantity\t: {3}", item.ProductID, item.ProductName, item.UnitPrice, item.Quantity, ++index));
            }

            return description + productList.ToString();
        }
        #endregion
    }

}
