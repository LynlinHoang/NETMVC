using SV20T1020056.DomainModels;

namespace SV20T1020056.Web.Models
{
    public class ProductSearchResult : BasePaginationResult
    {
        public List<Product> Data { get; set; }
    }
}
