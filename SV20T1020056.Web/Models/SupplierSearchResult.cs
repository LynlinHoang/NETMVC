using SV20T1020056.DomainModels;

namespace SV20T1020056.Web.Models
{
    public class SupplierSearchResult : BasePaginationResult
    {
        public List<Supplier> Data { get; set; }
    }
}
