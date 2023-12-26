using SV20T1080003.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1080003.DataLayers
{
    public interface IOrderDAL
    {
        IList<Order> List(int page = 1, int pageSize = 0, string searchValue = "", int status = 0);
        int Add(Order order);
        bool Update(Order order);
        Order? GetById(int id);
        bool Delete(int orderID);
        int Count(int status, string searchValue);
        OrderStatus? GetOrderStatus(int orderStatusID);
        IList<OrderStatus> ListOrderStatus();
        IList<OrderDetail> ListOrderDetails(int orderID);
        OrderDetail GetOrderDetail(int orderID, int productID);
        int SaveOrderDetail(int orderID, int productID, int quantity, decimal salePrice);
        bool DeleteOrderDetail(int orderID, int productID);
        bool DeleteOrderDetailByProductID(int productID);
    }
}
