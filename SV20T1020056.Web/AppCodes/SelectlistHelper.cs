using Microsoft.AspNetCore.Mvc.Rendering;
using SV20T1020056.BusinessLayers;

namespace SV20T1020056.Web
{
    public static class SelectlistHelper
    {
            public static List<SelectListItem> Provinces()
            {
            List<SelectListItem> list= new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                    Value = "",
                    Text = "--Chon tỉnh/thành--"
            });
            foreach (var item in CommonDataService.ListOfProvinces())
            {
                list.Add(new SelectListItem()
                {
                    Value= item.ProvinceName,
                    Text= item.ProvinceName
                });
            }
            return list;
            }
    }
}
