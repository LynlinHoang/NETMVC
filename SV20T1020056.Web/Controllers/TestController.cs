using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace SV20T1020056.Web.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Create()
        {
            var model = new Models.Person()
            {
                   Name = "Hoàng Thị Lin",
                   BirtDate = new DateTime(1990,10,25),
                   Salary=10.2m
            };
                return View(model);
        }
        public IActionResult Sava(Models.Person model)
        {
            return Json(model);
        }
        private DateTime? StringToDateTime(string s, string formats="d/M/yyyy; d-M-yyyy; d.M.yyy")
        {
            try
            {
                return DateTime.ParseExact(s, formats.Split(';'), CultureInfo.InvariantCulture);
            }
            catch
            {
                return null;
            }
        }
    }
}
