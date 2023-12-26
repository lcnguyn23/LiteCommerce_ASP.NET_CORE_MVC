using SV20T1080003.DomainModels;

namespace SV20T1080003.Web.Models
{
    public class PaginationSearchCategory : PaginationSearchBaseResult
    {
        public IList<Category> Data { get; set; }
    }
}
