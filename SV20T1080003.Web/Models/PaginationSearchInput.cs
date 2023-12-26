namespace SV20T1080003.Web.Models
{
    public class PaginationSearchInput
    {
        /// <summary>
        /// Thông tin đầu vào
        /// </summary>
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SearchValue { get; set; } = "";
    }
}
