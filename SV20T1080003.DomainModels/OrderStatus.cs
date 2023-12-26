using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1080003.DomainModels
{
    public class OrderStatus
    {
        /// <summary>
        /// Đơn hàng vừa được khởi tạo (đang chờ duyệt)
        /// </summary>
        public const int INIT = 1;
        /// <summary>
        /// Đơn hàng đã được duyệt (đang chờ chuyển hàng)
        /// </summary>
        public const int ACCEPTED = 2;
        /// <summary>
        /// Đơn hàng đang được chuyển hàng
        /// </summary>
        public const int SHIPPING = 3;
        /// <summary>
        /// Đơn hàng đã hoàn tất
        /// </summary>
        public const int FINISHED = 4;
        /// <summary>
        /// Đơn hàng bị hủy
        /// </summary>
        public const int CANCEL = -1;
        /// <summary>
        /// Đơn hàng bị từ chối
        /// </summary>
        public const int REJECTED = -2;
    }
}
