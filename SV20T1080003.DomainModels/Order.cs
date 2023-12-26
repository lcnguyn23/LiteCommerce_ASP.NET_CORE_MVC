using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1080003.DomainModels
{
    public class Order
    {
        /// <summary>
        /// Mã đơn hàng
        /// </summary>
        public int OrderID { get; set; }
        /// <summary>
        /// Thời điểm đặt hàng
        /// </summary>
        public DateTime OrderTime { get; set; }
        /// <summary>
        /// Thời điểm chấp nhận đơn hàng
        /// </summary>
        public DateTime? AcceptTime { get; set; } = null;
        /// <summary>
        /// Thời điểm bắt đầu giao hàng
        /// </summary>
        public DateTime? ShippedTime { get; set; }
        /// <summary>
        /// Thời điểm kết thúc quá trình xử lý đối với đơn hàng (là thời điểm khách nhận hàng nếu đơn hàng
        /// được giao, hoặc thời điểm thực hiện việc hủy bỏ/từ chối đơn hàng nếu đơn hàng bị hủy bỏ hoặc từ chối)
        /// </summary>
        public DateTime? FinishedTime { get; set; }
        /// <summary>
        /// Mã trạng thái đơn hàng
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Mã khách hàng đặt mua hàng
        /// </summary>
        public int? CustomerID { get; set; }
        /// <summary>
        /// Tên khách hàng
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// Tên giao dịch của khách hàng
        /// </summary>
        public string CustomerContactName { get; set; }
        /// <summary>
        /// Địa chỉ của khách hàng
        /// </summary>
        public string CustomerAddress { get; set; }
        /// <summary>
        /// Email của khách hàng
        /// </summary>
        public string CustomerEmail { get; set; }


        /// <summary>
        /// Mã của nhân viên phụ trách đơn hàng
        /// </summary>
        public int? EmployeeID { get; set; }
        /// <summary>
        /// Họ tên của nhân viên phụ trách đơn hàng
        /// </summary>
        public string EmployeeFullName { get; set; }

        /// <summary>
        /// Mã người giao hàng
        /// </summary>
        public int? ShipperID { get; set; } = null;
        /// <summary>
        /// Tên người giao hàng
        /// </summary>
        public string ShipperName { get; set; }
        /// <summary>
        /// Điện thoại của người giao hàng
        /// </summary>
        public string ShipperPhone { get; set; }

        /// <summary>
        /// Mô tả trạng thái đơn hàng dựa trên mã trạng thái
        /// </summary>
        public string StatusDescription
        {
            get
            {
                switch (Status)
                {
                    case OrderStatus.INIT:
                        return "Đơn hàng mới (chờ duyệt)";
                    case OrderStatus.ACCEPTED:
                        return "Đơn đã chấp nhận (chờ chuyển hàng)";
                    case OrderStatus.SHIPPING:
                        return "Đơn hàng đang được giao";
                    case OrderStatus.FINISHED:
                        return "Đơn hàng đã hoàn tất";
                    case OrderStatus.CANCEL:
                        return "Đơn hàng đã bị hủy";
                    case OrderStatus.REJECTED:
                        return "Đơn hàng bị từ chối";
                    default:
                        return "";
                }
            }
        }
    }
}
