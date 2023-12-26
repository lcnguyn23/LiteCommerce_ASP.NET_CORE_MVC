using SV20T1080003.DomainModels;

namespace SV20T1080003.Web.Models
{
    public class PaginationSearchOrder : PaginationSearchBaseResult
    {
        public List<Order> Data { get; set; }
    }
}
