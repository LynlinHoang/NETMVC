namespace SV20T1020056.Web.Models
{
    public class OrderSearchInput : PaginationSearchInput
    {
        public int Status { get; set; } = 0;
        public string DateRange { get; set; } = "";
        public DateTime? FromTime
        {
            get
            {
                if (string.IsNullOrWhiteSpace(DateRange))
                    return null;
                string[] times = DateRange.Split('-');
                if (times.Length == 2)
                {
                    DateTime? value = Converter.ToDateTime(times[0].Trim());
                    return value;
                }
                return null;
            }
        }
        public DateTime? ToTime
        {
            get
            {
                if (string.IsNullOrWhiteSpace(DateRange))
                    return null;
                string[] times = DateRange.Split('-');
                if (times.Length == 2)
                {
                    DateTime? value = Converter.ToDateTime(times[1].Trim());
                    if (value.HasValue)              
                    value = value.Value.AddMilliseconds(86399998); //86399999
                    return value;
                }
                return null;
            }
        }
    }
}
