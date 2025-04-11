using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1020056.BusinessLayers;
using SV20T1020056.DomainModels;
using SV20T1020056.Web.Models;

namespace SV20T1020056.Web.Controllers
{
    [Authorize(Roles =$"{WebUserRoles.Administrator},{WebUserRoles.Employee}")]
    public class CategoryController : Controller
    {
        private const int PAGE_SIZE = 20;
        private const string CATEGOGY_SEARCH = "Category_search";
        public IActionResult Index()
        {
            PaginationSearchInput input = ApplicationContext.GetSessionData<PaginationSearchInput>(CATEGOGY_SEARCH);
            if (input == null)
            {
                input = new PaginationSearchInput()
                {
                    Page=1,
                    PageSize=PAGE_SIZE,
                    SearchValue=""
                };
            }
            return View(input);
        }
        public IActionResult Search(PaginationSearchInput input)
        {
            int rowCount = 0;
            var data = CommonDataService.ListOfCategory(out rowCount, input.Page, input.PageSize, input.SearchValue?? "");
            var model = new CategorySearchResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue=input.SearchValue ??"",
                RowCount = rowCount,
                Data = data
            };
            ApplicationContext.SetSessionData(CATEGOGY_SEARCH, input);
            return View(model);
        }
        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung loại hàng";
            Category model = new Category()
            {
                CategoryID = 0
            };
            
            return View("Edit", model);
        }
        public IActionResult Edit(int id)
        {
            ViewBag.Title = "Cập nhập thông tin loại hàng";
            Category? model = CommonDataService.GetCategory(id);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }
        [HttpPost]
        public IActionResult Save(Category data, IFormFile? uploadPhoto)
        {
            try
            {
                if (uploadPhoto != null)
                {
                    string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}"; //tên file sẽ lưu
                    string folder = Path.Combine(ApplicationContext.HostEnviroment.WebRootPath, @"images\category"); //đường dẫn lưu file
                    string filePath = Path.Combine(folder, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        uploadPhoto.CopyTo(stream);
                    }
                    data.Photo = fileName;
                }
                if (string.IsNullOrWhiteSpace(data.CategoryName))
                    ModelState.AddModelError("CategoryName", "Tên loại hàng không được để trống!");
                if (string.IsNullOrWhiteSpace(data.Description))
                    ModelState.AddModelError("Description", "Mô tả không được để trống!");
                if (!ModelState.IsValid)
                {
                    ViewBag.Title= data.CategoryID==0 ? "Bổ xung loại hàng" : "Cập nhập loại hàng";
                    return View("Edit", data);
                }
                if (data.CategoryID == 0)

                {

                    int id = CommonDataService.AddCategory(data);
                    if (id<=0)
                    {
                        ModelState.AddModelError(nameof(data.CategoryName), "Tên loại hàng bị trùng");
                        return View("Edit", data);
                    }
                }
                else
                {
                    
                    bool result = CommonDataService.UpdateCategory(data);
                    if (!result)
                    {
                        ModelState.AddModelError(nameof(data.CategoryName), "Tên loại hàng bị trùng với tên loại hàng trước");
                        return View("Edit", data);
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "Không thể lưu dữ liệu. Vui lòng thử lại sau vài phút");
                return View("Edit", data);
            }
        }
        public IActionResult Delete(int id)
        {
            if (Request.Method == "POST")
            {
                CommonDataService.DeleteCategory(id);
                return RedirectToAction("Index");
            }
            var model = CommonDataService.GetCategory(id);
            if (model==null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.AllowDelete = !CommonDataService.IsUsedCategory(id);
            return View(model);
        }

    }
}
