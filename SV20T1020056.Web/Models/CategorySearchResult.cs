using SV20T1020056.DomainModels;

namespace SV20T1020056.Web.Models
{
    public class CategorySearchResult : BasePaginationResult
    {
        public List<Category> Data { get; set; }
    }
}
