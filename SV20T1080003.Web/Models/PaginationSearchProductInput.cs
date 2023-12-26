namespace SV20T1080003.Web.Models
{
    public class PaginationSearchProductInput : PaginationSearchInput
    {
        public int CategoryId { get; set; } = 0;
        public int SupplierId { get; set; } = 0;
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
    }
}
