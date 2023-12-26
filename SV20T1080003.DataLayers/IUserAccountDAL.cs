using SV20T1080003.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1080003.DataLayers
{
    /// <summary>
    /// Định nghĩa các phép xử lý liên quan tới tài khoản
    /// </summary>
    public interface IUserAccountDAL
    {
        /// <summary>
        /// Xác thực tài khoản đăng nhập của người dùng (employee, customer)
        /// Nếu xác thực thành công thì trả về thông tin của tài khoản, ngược lại trả về null
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        UserAccount? Authorize(string userName, string password);

        /// <summary>
        /// Thay đổi mật khẩu
        /// Kiểm tra mật khẩu cũ sử dụng Authorize trên
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool ChangePassword(string userName, string password);
    }
}
