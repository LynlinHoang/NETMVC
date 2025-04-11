using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.IdentityModel.Tokens;
using SV20T1020056.BusinessLayers;
using SV20T1020056.DomainModels;
using SV20T1020056.Web.Models;
using System.Drawing.Printing;

namespace SV20T1020056.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator},{WebUserRoles.Employee}")]

    public class OrderController : Controller
    {
        private const int ORDER_PAGE_SIZE = 20;
        private const string ORDER_SEARCH = "order_search";
        private const string SHOPPING_CART = "shopping_cart";
        private const int PRODUCT_PAGE_SIZE = 20;
        private const string PRODUCT_SEARCH = "product_search_for_sale";
        public IActionResult Index()
        {
            OrderSearchInput? input = ApplicationContext.GetSessionData<OrderSearchInput>(ORDER_SEARCH);
            if (input == null)
            {
                input = new OrderSearchInput()
                {
                    Page=1,
                    PageSize=ORDER_PAGE_SIZE,
                    SearchValue="",
                    Status=0,
                    DateRange=string.Format("{0:dd/MM/yyyy} - {1:dd/MM/yyyy}", DateTime.Today.AddMonths(-1), DateTime.Today.AddDays(1))
                };
            }
            return View(input);
        }
        public IActionResult Search(OrderSearchInput input)
        {
            int rowCount = 0;
            var data = OrderDataService.ListOrders(out rowCount, input.Page, input.PageSize, input.Status, input.FromTime, input.ToTime, input.SearchValue ?? "");
            var model = new OrderSearchResult()
            {
                Page= input.Page,
                PageSize=input.PageSize,
                SearchValue=input.SearchValue??"",
                Status=input.Status,
                TimeRange=input.DateRange??"",
                RowCount=rowCount,
                Data=data
            };
            ApplicationContext.SetSessionData(ORDER_SEARCH, input);

            return View(model);
        }
        public IActionResult Create()
        {
            ProductSeachInput input = ApplicationContext.GetSessionData<ProductSeachInput>(PRODUCT_SEARCH);
            if (input == null)
            {
                input = new ProductSeachInput()
                {
                    Page=1,
                    PageSize=PRODUCT_PAGE_SIZE,
                    SearchValue=""
                };
            }
            return View(input);
        }

        public IActionResult SearchProduct(ProductSeachInput input)
        {
            int rowCount = 0;
            var data = ProductDataService.ListProducts(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "");
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
        public IActionResult Details(int id = 0)
        {
            var order = OrderDataService.GetOrder(id);
            if(order== null)
            {
                return RedirectToAction("Index");

            }
            var details = OrderDataService.ListOrderDetails(id);
            var model = new OrderDetailModel
            {
                Order = order,
                Details = details
            };
            return View(model);
        }
        public IActionResult Accept(int id=0)
        {
            bool result = OrderDataService.AcceptOrder(id);
            if (!result)
                TempData["Message"]= "Không thể duyệt đơn hàng này";

            return RedirectToAction("Details", new {id});
        }
       
        public IActionResult Cancel(int id = 0)
        {
            bool result = OrderDataService.CancelOrder(id);
            if (!result)
                TempData["Message"]= "Không thể thực hiện thao tác hủy đối với đơn hàng này";

            return RedirectToAction("Details", new { id });
        }
        public IActionResult Reject(int id = 0)
        {
            bool result = OrderDataService.RejectOrder(id);
            if (!result)
                TempData["Message"]= "Không thể thực hiện thao tác từ chối đối với đơn hàng này";

            return RedirectToAction("Details", new { id });
        }
        public IActionResult Finish(int id = 0)
        {
            bool result = OrderDataService.FinishOrder(id);
            if (!result)
                TempData["Message"]= "Không thể ghi nhận trạng thái kết thúc cho đơn hàng này";

            return RedirectToAction("Details", new { id });
        }

       
        public IActionResult Delete(int id)
        {
            bool result = OrderDataService.DeleteOrder(id);
            if (!result)
            {
                TempData["Message"]= "Không thể xóa đơn hàng này";
            }
            return RedirectToAction("Details");
        }
        public IActionResult DeleteDetail(int id = 0, int productId = 0)
        {
            bool result = OrderDataService.DeleteOrderDetail(id,productId);
            if (!result)
            {
                TempData["Message"]= "Không thể xóa mặt hàng ra khỏi đơn hàng";
            }
            return RedirectToAction("Details", new {id});
        } 
        [HttpGet]
        public IActionResult EditDetail(int id = 0, int productId = 0)
        {
            var model= OrderDataService.GetOrderDetail(id, productId);
            return View(model);
        }
        [HttpPost]
        public IActionResult UpdateDetail(int orderID, int productID, int quantity, decimal salePrice)
        {
            if (quantity<=0)
            {
                return Json("Số lượng bán không hợp lệ");
            }
            if (salePrice<0)
            {
                return Json("Gía bán không hợp lệ");
            }
            bool result = OrderDataService.SaveOrderDetail(orderID,productID,quantity,salePrice);
            if (!result)
            {
                return Json("Không được phép thay đổi thông tin của đơn hàng này");
            }

            return Json("");
        }
        [HttpGet]
        public IActionResult Shipping(int id = 0)
        {
            ViewBag.OrderID=id;

            return View();
        }
        [HttpPost]
        public IActionResult Shipping(int id = 0,int shipperID=0)
        {
            if (shipperID<=0) return Json("Vui lòng chọn người giao hàng");
            bool result= OrderDataService.ShipOrder(id, shipperID);
            if (!result)
            {
                return Json("Đơn hàng không cho phép chuyển cho người giao hàng");

            }
            return Json("");
        }

      
        private List<OrderDetail> GetShoppingCart()
        {
            var shoppingCart = ApplicationContext.GetSessionData<List<OrderDetail>>(SHOPPING_CART);
            if (shoppingCart == null)
            {
                shoppingCart = new List<OrderDetail>();
                ApplicationContext.SetSessionData(SHOPPING_CART, shoppingCart);

            }
            return shoppingCart;
        }

        public IActionResult ShowShoppingCart()
        {
            var model= GetShoppingCart();
            return View(model);
        }
        public IActionResult RemoveFromCart(int id = 0)
        {
            var shoppingCart = GetShoppingCart();
            int index = shoppingCart.FindIndex(m => m.ProductID==id);
            if (index>=0)
                shoppingCart.RemoveAt(index);
            ApplicationContext.SetSessionData(SHOPPING_CART, shoppingCart);

            return Json("");
        }
        public IActionResult AddToCart(OrderDetail data)
        {
            if (data.SalePrice<=0|| data.Quantity<=0)
                return Json("Giá bán và số lượng không hợp lệ");

            var shoppingCart = GetShoppingCart();
            var existsProduct = shoppingCart.FirstOrDefault(m => m.ProductID==data.ProductID);
            if (existsProduct == null)
            {
                shoppingCart.Add(data);

            }
            else
            {
                existsProduct.Quantity += data.Quantity;
                existsProduct.SalePrice= data.SalePrice;
            }
            ApplicationContext.SetSessionData(SHOPPING_CART, shoppingCart);
            return Json("");
        }
        public IActionResult ClearCart()
        {
            var shoppingCart = GetShoppingCart();
            shoppingCart.Clear();
            ApplicationContext.SetSessionData(SHOPPING_CART, shoppingCart);
            return Json("");
        }
        public IActionResult Init(int customerID= 0, string deliveryProvince="", string deliveryAddress = "")
        {
            var shoppingCart = GetShoppingCart();
            if (shoppingCart.Count==0)
                return Json("Giỏ hàng trống, không thể lập đơn hàng");
            if(customerID<=0||string.IsNullOrWhiteSpace(deliveryProvince)||string.IsNullOrWhiteSpace(deliveryAddress))
                return Json("Vui lòng nhập đầy đủ thông tin");
            int employeeID = Convert.ToInt32(User.GetUserData()?.UserId);
            int orderID = OrderDataService.InitOrder(employeeID, customerID, deliveryProvince, deliveryAddress, shoppingCart);
            ClearCart();
            return Json(orderID);
        }
        [HttpGet]
        public IActionResult EditProvince(int id = 0)
        {
            var model = OrderDataService.GetOrder(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult EditProvince(int id = 0, string DeliveryProvince = "", string DeliveryAddress = "")
        {
            if (DeliveryProvince.IsNullOrEmpty())
                return Json("Vui lòng chọn tỉnh/thành");
            bool result = OrderDataService.EditProvince(id, DeliveryProvince, DeliveryAddress);
            if (!result)
                return Json("Đơn hàng không cho phép chuyển tỉnh/thành");
            return Json("");
        }
    }
}
