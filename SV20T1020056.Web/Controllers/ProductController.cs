using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1020056.BusinessLayers;
using SV20T1020056.DomainModels;
using SV20T1020056.Web.Models;
using System.Buffers;

namespace SV20T1020056.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator},{WebUserRoles.Employee}")]
    public class ProductController : Controller
    {
        private const int PAGE_SIZE = 20;
        private const string PRODUCT_SEARCH = "product_search";
        //public IActionResult Index(int page = 1, string searchValue = "",int CategoryID=0, int SupplierID =0)
        //{
        //    int rowCount = 0;
        //    var data = ProductDataService.ListProducts(out rowCount, page, PAGE_SIZE, searchValue ?? "", CategoryID, SupplierID);
        //    var model = new Models.ProductSearchResult()
        //    {
        //        Page = page,
        //        PageSize = PAGE_SIZE,
        //        SearchValue = searchValue ?? "",
        //        RowCount= rowCount,
        //        Data = data

        //    };

        //    return View(model);
        //}
        public IActionResult Index()
        {
            PaginationSearchInput input = ApplicationContext.GetSessionData<PaginationSearchInput>(PRODUCT_SEARCH);
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
        public IActionResult Search(PaginationSearchInput input, int CategoryID = 0, int SupplierID = 0)
        {
            int rowCount = 0;
            var data = ProductDataService.ListProducts(out rowCount, input.Page, input.PageSize, input.SearchValue?? "", CategoryID, SupplierID);
            var model = new ProductSearchResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue=input.SearchValue ??"",
                RowCount = rowCount,
                Data = data
            };
            ApplicationContext.SetSessionData(PRODUCT_SEARCH, input);
            return View(model);
        }
        public IActionResult Create()
        {
            ViewBag.Title="Bổ sung mặt hàng";
            ViewBag.IsEdit=false;
            return View("Edit");
        }

        public IActionResult Edit(int id)
        {
            ViewBag.Title="Cập nhật thông tin mặt hàng";
            ViewBag.IsEdit=true;         
            Product? model = ProductDataService.GetProduct(id);         
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }
        public IActionResult Delete(int id)
        {
            ViewBag.Title="Xoá thông tin mặt hàng";
            if (Request.Method == "POST")
            {
                ProductDataService.DeleteProducts(id);
                return RedirectToAction("Index");
            }
            var model = ProductDataService.GetProduct(id);
            if (model==null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.AllowDelete = !ProductDataService.InUsedProduct(id);
            return View(model);
        }
        public IActionResult SaveCreate(Product data, IFormFile? uploadPhoto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(data.ProductName))
                    ModelState.AddModelError(nameof(data.ProductName), "Tên mặt hàng không được để trống!");
                if (data.CategoryID<=0)
                    ModelState.AddModelError(nameof(data.SupplierID), "Loại hàng không được để trống!");
                if (data.SupplierID<=0)
                    ModelState.AddModelError(nameof(data.CategoryID), "Nhà cung cấp không được để trống!");
                if (data.Price<=0||data.Price==null)
                    ModelState.AddModelError(nameof(data.Price), "Gía không được để trống!");

                if (!ModelState.IsValid)
                {
                    ViewBag.Title = data.ProductID == 0 ? "Bổ sung mặt hàng" : "Cập nhật thông tin mặt hàng";
                    ViewBag.IsEdit = data.ProductID == 0 ? false : true;
                    return View("Edit", data);
                }

                if (uploadPhoto != null)
                {
                    string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}"; //tên file sẽ lưu
                    string folder = Path.Combine(ApplicationContext.HostEnviroment.WebRootPath, @"images\products"); //đường dẫn lưu file
                    string filePath = Path.Combine(folder, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        uploadPhoto.CopyTo(stream);
                    }
                    data.Photo = fileName;
                }
                //return Json(data);
                ProductDataService.AddProduct(data);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public IActionResult SaveUpdate(Product data, IFormFile? uploadPhoto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(data.ProductName))
                    ModelState.AddModelError("ProductName", "Tên mặt hàng không được để trống!");          
                if (data.CategoryID==0)
                    ModelState.AddModelError("CategoryID", "Loại hàng không được để trống!");
                if (data.SupplierID==0)
                    ModelState.AddModelError("SupplierID", "Nhà cung cấp không được để trống!");            
                if (data.Price==0)
                    ModelState.AddModelError("Price", "Gía không được để trống!");
                if (!ModelState.IsValid)
                {
                    ViewBag.Title = data.ProductID == 0 ? "Bổ sung mặt hàng" : "Cập nhật thông tin mặt hàng";
                    ViewBag.IsEdit = data.ProductID == 0 ? false :  true;
                    return View("Edit", data);
                }
                if (uploadPhoto != null)
                {
                    string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}"; //tên file sẽ lưu
                    string folder = Path.Combine(ApplicationContext.HostEnviroment.WebRootPath, @"images\products"); //đường dẫn lưu file
                    string filePath = Path.Combine(folder, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        uploadPhoto.CopyTo(stream);
                    }
                    data.Photo = fileName;
                }
                ProductDataService.UpdateProducts(data);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public IActionResult Photo(int id, string method, int photoID = 0)
        {
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung ảnh";
                    var addModel = new ProductPhoto
                    {
                        PhotoID = 0, 
                        ProductID = id,
                    };
                    return View("photo", addModel);
                case "edit":
                    ViewBag.Title = "Thay đổi ảnh";
                    var editModel = ProductDataService.GetPhoto(photoID);
                    if (editModel == null)
                        return RedirectToAction("Edit", new { id = id });
                    return View(editModel);
                case "delete":                  
                        ProductDataService.DeletePhoto(photoID);                                        
                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Index");
            }
        }


        public IActionResult Attribute(int id, String method, int attributeId = 0)
        {
            switch (method)
            {

                case "add":
                    ViewBag.Title="Bổ thuộc tính";
                    var addModel = new ProductAttribute
                    {
                        AttributeID = 0,
                        ProductID = id,
                    };
                    return View("Attribute", addModel);
                case "edit":
                    ViewBag.Title="Thay thuộc tính";
                    var editModel = ProductDataService.GetAttribute(attributeId);
                    if (editModel == null)
                        return RedirectToAction("Edit", new { id = id });

                    return View(editModel);
                case "delete":
                    ProductDataService.DeleteAttribute(attributeId);
                    return RedirectToAction("Edit", new { id = id });
                // new { id = id }
                default:
                    return RedirectToAction("Index");

            }
        }
        [HttpPost]
        public IActionResult SavePhoto(ProductPhoto data, IFormFile? uploadPhoto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(data.Description))
                    ModelState.AddModelError("Description", "Mô tả không được để trống!");
                if (string.IsNullOrWhiteSpace(data.DisplayOrder))
                    ModelState.AddModelError("DisplayOrder", "Thứ tự không được để trống!");

                if (!ModelState.IsValid)
                {
                   
                    return View("Photo", data);
                }

                if (uploadPhoto != null)
                {
                    string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}"; //tên file sẽ lưu
                    string folder = Path.Combine(ApplicationContext.HostEnviroment.WebRootPath, @"images\products"); //đường dẫn lưu file
                    string filePath = Path.Combine(folder, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        uploadPhoto.CopyTo(stream);
                    }
                    data.Photo = fileName;
                }
                if (data.PhotoID == 0)
                {                    
                    ProductDataService.AddPhoto(data);
                }
                else
                {
                    bool result = ProductDataService.UpdatePhoto(data);
                }                
                return RedirectToAction("Edit", new { id = data.ProductID });
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public IActionResult SaveAttribute(ProductAttribute data)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(data.AttributeName))
                    ModelState.AddModelError("AttributeName", "Tên thuộc tính không được để trống!");
                if (string.IsNullOrWhiteSpace(data.AttributeValue))
                    ModelState.AddModelError("AttributeValue", "Tên giá trị không được để trống!");

                if (!ModelState.IsValid)
                {
                  
                    return View("Attribute", data);
                }

                if (data.AttributeID == 0)
                {
                    ProductDataService.AddAtribute(data);
                }
                else
                {                  
                    bool result = ProductDataService.UpdateAttribute(data);
                }
               
                return RedirectToAction("Edit",new { id = data.ProductID });
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

    }
}
