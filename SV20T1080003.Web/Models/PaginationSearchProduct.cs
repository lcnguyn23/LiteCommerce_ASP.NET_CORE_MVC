using SV20T1080003.DomainModels;

namespace SV20T1080003.Web.Models
{
    public class PaginationSearchProduct : PaginationSearchBaseResult
    {
        public IList<Product> Data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int CategoryId { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int SupplierId { get; set; }
    }
}
