using SV20T1080003.DomainModels;

namespace SV20T1080003.Web.Models
{
    public class PaginationSearchSupplier : PaginationSearchBaseResult
    {
        public IList<Supplier> Data { get; set; }
    }
}
