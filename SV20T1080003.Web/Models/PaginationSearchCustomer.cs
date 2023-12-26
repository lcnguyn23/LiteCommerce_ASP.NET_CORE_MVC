using SV20T1080003.DomainModels;

namespace SV20T1080003.Web.Models
{
    public class PaginationSearchCustomer : PaginationSearchBaseResult
    {
        public IList<Customer> Data { get; set; }
    }
}
