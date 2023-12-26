using SV20T1080003.DomainModels;

namespace SV20T1080003.Web.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class PaginationSearchOrderDetail
    {
        /// <summary>
        /// Lấy ra dữ liệu của đơn hàng
        /// </summary>
        public Order Order { get; set; }

        /// <summary>
        /// Lấy ra thông tin chi tiết của đơn hàng
        /// </summary>
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
