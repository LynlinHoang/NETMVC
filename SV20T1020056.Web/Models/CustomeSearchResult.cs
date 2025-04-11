using SV20T1020056.DomainModels;

namespace SV20T1020056.Web.Models
{
    public class CustomeSearchResult : BasePaginationResult
    {
        public List<Customer> Data { get; set; }
    }
    
}
