using SV20T1020056.DomainModels;

namespace SV20T1020056.Web.Models
{
    public class EmployeeSearchResult : BasePaginationResult
    {
        public List<Employee> Data { get; set; }
    }
}
