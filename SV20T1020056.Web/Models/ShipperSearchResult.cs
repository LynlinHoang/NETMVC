using SV20T1020056.DomainModels;

namespace SV20T1020056.Web.Models
{
    public class ShipperSearchResult : BasePaginationResult
    {
        public List<Shipper> Data { get; set; }
    }
}
