using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1080003.DataLayers
{
    public interface ICommonDAL<T> where T : class //generic
    {
        /// <summary>
        /// Tìm kiếm và lấy danh sách dữ liệu dưới dạng phân trang
        /// </summary>
        /// <param name="page">Trang cần hiển thị</param>  
        /// <param name="pageSize">Số dòng trên mỗi trang(0 nếu không tiến hành phân trang)</param>
        /// <param name="searchValue">Giá trị tìm kiếm(chuỗi rỗng nếu lấy toàn bộ dữ liệu)</param>
        /// <returns></returns>
        IList<T> List(int page = 1, int pageSize = 0, string searchValue = "");
        /// <summary>
        /// Đếm số dòng dữ liệu thỏa điều kiện tìm kiếm
        /// </summary>
        /// <param name="serchValue">Giá trị tìm kiếm(chuỗi rỗng nếu lấy toàn bộ dữ liệu)</param>
        /// <return></return>
        int Count(string searchValue = "");
        /// <summary>
        /// Bổ sung thêm dữ liệu vào database. Hàm trả về ID của dữ liệu được bổ sung (nếu trả về 0 tức là lỗi)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(T data);
        /// <summary>
        /// Cập nhật dữ liệu
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(T data);
        /// <summary>
        /// Xóa dữ liệu
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Delete(int id);
        /// <summary>
        /// Lấy bản ghi dữ liệu dựa vào mã
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T? Get(int id);
        /// <summary>
        /// KIểm tra xem dữ liệu có mã id hiện có đang được sử dụng bởi các dữ liệu khác hay không
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool InUsed(int id);
    }
}
