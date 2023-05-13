using System;
using Pos_System.API.Enums;
using Pos_System.API.Payload.Response.Orders;

namespace Pos_System.API.Payload.Response.Stores
{
	public class GetStoreEndDayReport
	{
        public Guid StoreId { get; set; }
        public List<CategoryReport> CategoryReports { get; set; } = new List<CategoryReport>();
        public double TotalAmount { get; set; }
        public double TotalProductDiscount { get; set; }
        public double TotalPromotionDiscount { get; set; }
        public double TotalDiscount { get; set; }
        public double VatAmount { get; set; }
        public double FinalAmount { get; set; }
        public double ProductCosAmount { get; set; }
        public double TotalRevenue { get; set; }
        public double InStoreAmount { get; set; }
        public double DeliAmount { get; set; }
        public double TakeAwayAmount { get; set; }
        public int TotalProduct { get; set; }
        public int TotalOrder { get; set; }
        public int TotalOrderInStore { get; set; }
        public int TotalOrderTakeAway { get; set; }
        public int TotalOrderDeli { get; set; }
        public double AverageBill { get; set; }
        public List<int> TimeLine { get; set; } = new List<int>();
        public List<int> TotalOrderTimeLine { get; set; } = new List<int>();
        public List<double> TotalAmountTimeLine { get; set; } = new List<double>();
    }

    public class CategoryReport
    {

        public CategoryReport(Guid id, string name, int totalProduct, double totalAmount, double totalDiscount, List<ProductReport> productReports)
        {
            Id = id;
            Name = name;
            TotalProduct = totalProduct;
            TotalAmount = totalAmount;
            TotalDiscount = totalDiscount;
            ProductReports = productReports;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public int TotalProduct { get; set; }
        public double TotalAmount { get; set; }
        public double TotalDiscount { get; set; }
        public List<ProductReport> ProductReports { get; set; } = new List<ProductReport>();
    }

    public class ProductReport
    {
        public ProductReport(Guid id, string name, int quantity, double totalAmount, double totalDiscount)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
            TotalAmount = totalAmount;
            TotalDiscount = totalDiscount;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public double TotalAmount { get; set; }
        public double TotalDiscount { get; set; }
    }
}

