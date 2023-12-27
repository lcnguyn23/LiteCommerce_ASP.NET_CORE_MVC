using SV20T1080003.DataLayers;
using SV20T1080003.DataLayers.SQLServer;
using SV20T1080003.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1080003.BusinessLayers
{
    public class OrderDataService
    {
        private static readonly IOrderDAL orderDB;
        /// <summary>
        /// 
        /// </summary>
        static OrderDataService()
        {
            string connectionString = "server=TANLOC;user id=sa;password=12345;database=LiteCommerceDB;TrustServerCertificate=true";
            orderDB = new DataLayers.SQLServer.OrderDAL(connectionString);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Order> ListOrders(string searchValue = "")
        {
            return orderDB.List(1, 0, searchValue, 0).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="status"></param>
        /// <param name="searchValue"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public static List<Order> ListOrders(int page, int pageSize, int status, string searchValue, out int rowCount)
        {
            rowCount = orderDB.Count(status, searchValue);
            return orderDB.List(page, pageSize, searchValue, status).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public static Order GetOrder(int orderID)
        {
            return orderDB.GetById(orderID);
        }
        
        public static int InitOrder(int customerID, int employeeID, DateTime orderTime, IEnumerable<OrderDetail> details)
        {
            Order data = new Order()
            {
                CustomerID = customerID,
                EmployeeID = employeeID,
                OrderTime = orderTime,
                AcceptTime = null,
                ShipperID = null,
                ShippedTime = null,
                FinishedTime = null,
                Status = OrderStatus.INIT
            };
            return orderDB.Add(data, details);
        }
        /// <summary>
        /// Hủy bỏ đơn hàng
        /// </summary>
        /// <param name="orderID">Mã đơn hàng cần hủy</param>
        /// <returns></returns>
        public static bool CancelOrder(int orderID)
        {
            Order data = orderDB.GetById(orderID);
            if (data == null)
                return false;

            if (data.Status == OrderStatus.FINISHED
                    || data.Status == OrderStatus.REJECTED)
            {
                return false;
            }

            data.Status = OrderStatus.CANCEL;
            data.FinishedTime = DateTime.Now;
            return orderDB.Update(data);
        }
        /// <summary>
        /// Từ chối đơn hàng
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public static bool RejectOrder(int orderID)
        {
            Order data = orderDB.GetById(orderID);
            if (data == null)
                return false;

            if (data.Status == OrderStatus.ACCEPTED
                    || data.Status == OrderStatus.SHIPPING
                    || data.Status == OrderStatus.FINISHED
                    || data.Status == OrderStatus.CANCEL)
            {
                return false;
            }

            data.Status = OrderStatus.REJECTED;
            data.FinishedTime = DateTime.Now;
            return orderDB.Update(data);
        }
        /// <summary>
        /// Chấp nhận đơn hàng
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public static bool AcceptOrder(int orderID)
        {
            Order data = orderDB.GetById(orderID);
            if (data == null)
                return false;

            if (data.Status == OrderStatus.INIT)
            {
                data.Status = OrderStatus.ACCEPTED;
                data.AcceptTime = DateTime.Now;
                return orderDB.Update(data);
            }
            return false;
        }
        /// <summary>
        /// Xác nhận đã chuyển hàng
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="shipperID"></param>
        /// <returns></returns>
        public static bool ShipOrder(int orderID, int shipperID)
        {
            Order data = orderDB.GetById(orderID);
            if (data == null)
                return false;

            if (data.Status == OrderStatus.ACCEPTED)
            {
                data.Status = OrderStatus.SHIPPING;
                data.ShipperID = shipperID;
                data.ShippedTime = DateTime.Now;
                return orderDB.Update(data);
            }
            return false;
        }
        /// <summary>
        /// Ghi nhận kết thúc quá trình xử lý đơn hàng thành công
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public static bool FinishOrder(int orderID)
        {
            Order data = orderDB.GetById(orderID);
            if (data == null)
                return false;

            if (data.Status == OrderStatus.SHIPPING)
            {
                data.Status = OrderStatus.FINISHED;
                data.FinishedTime = DateTime.Now;
                return orderDB.Update(data);
            }
            return false;
        }
        /// <summary>
        /// Xóa đơn hàng và toàn bộ chi tiết của đơn hàng
        /// (chỉ cho phép xóa đơn hàng đang ở một trong số các trạng thái: vừa khởi tạo, bị hủy hoặc bị từ chối)
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public static bool DeleteOrder(int orderID)
        {
            var data = orderDB.GetById(orderID);
            if (data == null)
                return false;

            if (data.Status == OrderStatus.INIT || data.Status == OrderStatus.CANCEL || data.Status == OrderStatus.REJECTED)
                return orderDB.Delete(orderID);

            return false;
        }
        /// <summary>
        /// Lấy danh sách chi tiết của đơn hàng
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public static List<OrderDetail> ListOrderDetails(int orderID)
        {
            return orderDB.ListOrderDetails(orderID).ToList();
        }
        /// <summary>
        /// Lấy 1 chi tiết của đơn hàng
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="productID"></param>
        /// <returns></returns>
        public static OrderDetail GetOrderDetail(int orderID, int productID)
        {
            return orderDB.GetOrderDetail(orderID, productID);
        }
        
        
        public static int SaveOrderDetail(int orderID, int productID, int quantity, decimal salePrice)
        {
            return orderDB.SaveOrderDetail(orderID, productID, quantity, salePrice);
        }
        /// <summary>
        /// Xóa 1 chi tiết trong đơn hàng
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="productID"></param>
        /// <returns></returns>
        public static bool DeleteOrderDetail(int orderID, int productID)
        {
            return orderDB.DeleteOrderDetail(orderID, productID);
        }

    }
}

