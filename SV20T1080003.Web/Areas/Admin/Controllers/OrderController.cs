using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1080003.BusinessLayer;
using SV20T1080003.BusinessLayers;
using SV20T1080003.DomainModels;
using SV20T1080003.Web.AppCodes;
using SV20T1080003.Web.Models;
using System.Drawing.Printing;

namespace SV20T1080003.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize(Roles = $"{WebUserRoles.Administrator}")]
    [Area("Admin")]
    public class OrderController : Controller
    {
        private const int PAGE_SIZE = 5;
        private const string CART = "CART";
        private const string ORDER_SEARCH = "Order_Search";
        private const string PRODUCT_SEARCH = "Product_Search";
        private const string CUSTOMERID = "CustomerId";
        private const string EMPLOYEEID = "EmployeeId";
        private const string ERROR_MESSAGE = "Error_Message";

        /// <summary>
        /// Hiển thị danh sách đơn hàng
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            var input = ApplicationContext.GetSessionData<PaginationSearchOrderInput>(ORDER_SEARCH);
            if (input == null)
            {
                input = new PaginationSearchOrderInput()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = "",
                    Status = 0
                };
            }
            return View(input);
        }

        public IActionResult Search(PaginationSearchOrderInput input)
        {
            int rowCount = 0;
            var data = OrderDataService.ListOrders(
                                            input.Page,
                                            input.PageSize,
                                            input.Status,
                                            input.SearchValue ?? "",
                                            out rowCount
                                            );
            var model = new PaginationSearchOrder()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                RowCount = rowCount,
                Data = data
            };

            ApplicationContext.SetSessionData(ORDER_SEARCH, model);

            return View(model);
        }
        /// <summary>
        /// Giao diện trang tạo đơn hàng
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            var model = GetCart();
            ViewBag.ErrorMessage = TempData[ERROR_MESSAGE] ?? "";
            ViewBag.CustomerID = TempData[CUSTOMERID] ?? "";
            ViewBag.EmployeeID = TempData[EMPLOYEEID] ?? "";
            return View(model);
        }

        /// <summary>
        /// Tìm kiếm và hiển thị thông tin mặt hàng
        /// </summary>
        /// <param name="page"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>

        [HttpPost]
        public IActionResult SearchProduct(string searchValue)
        {
            
            int rowCount = 0;
            var data = ProductDataService.ListProducts(1, PAGE_SIZE, searchValue, 0, 0, 0, 0, out rowCount);
            
            ApplicationContext.SetSessionData(PRODUCT_SEARCH, data);

            return View(data);
        }
        /// <summary>
        /// Hiển thị giỏ hàng
        /// </summary>
        /// <returns></returns>

        public IActionResult ShowCart()
        {
            var model = GetCart();
            return View(model);
        }

        /// <summary>
        /// Lấy danh sách các mặt hàng trong giỏ
        /// </summary>
        /// <returns></returns>
        private List<CartItem> GetCart()
        {
            var cart = ApplicationContext.GetSessionData<List<CartItem>>(CART);
            if (cart == null)
            {
                cart = new List<CartItem>();
                ApplicationContext.SetSessionData(CART, cart);
            }
            return cart;
        }

        /// <summary>
        /// Giao diện trang chi tiết đơn hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Details(int id = 0)
        {
            if (id < 0)
            {
                return RedirectToAction("Index");
            }
            Order order = OrderDataService.GetOrder(id);
            List<OrderDetail> orderDetails = OrderDataService.ListOrderDetails(id);

            PaginationSearchOrderDetail result = new PaginationSearchOrderDetail()
            {
                Order = order,
                OrderDetails = orderDetails
            };
            return View(result);
        }
        /// <summary>
        /// Giao diện cập nhật thông tin chi tiết đơn hàng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productId"></param>
        /// <returns></returns>        
        [HttpGet]
        public IActionResult EditDetail(int id = 0, int productId = 0)
        {
            if (id < 0)
            {
                return RedirectToAction("Index");
            }
            if (productId < 0)
            {
                return RedirectToAction("Details", new { id = productId });
            }
            OrderDetail orderDetail = OrderDataService.GetOrderDetail(id, productId);
            if (orderDetail == null)
            {
                return RedirectToAction("Index");
            }
            return View(orderDetail);
        }
        /// <summary>
        /// Cập nhật chi tiết đơn hàng (trong giỏ hàng)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <param name="salePrice"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UpdateDetail(OrderDetail data)
        {
            if (data.ProductID <= 0)
            {
                //TempData[ERROR_MESSAGE] = "Mặt hàng không tồn tại";
                return RedirectToAction("Details", new {id = data.OrderID });
            }
            // Số lượng
            if (data.Quantity < 1)
            {
                //TempData[ERROR_MESSAGE] = "Số lượng không hợp lệ";
                return RedirectToAction("Details", new {id = data.OrderID });
            }

            // Đơn giá
            if (data.SalePrice < 1)
            {
                //TempData[ERROR_MESSAGE] = "Đơn giá không hợp lệ";
                return RedirectToAction("Details", new { id = data.OrderID });
            }

            // Cập nhật chi tiết 1 đơn hàng nếu kiểm tra đúng hết
            OrderDataService.SaveOrderDetail(data.OrderID, data.ProductID, data.Quantity, data.SalePrice);
            return RedirectToAction("Details", new { id = data.OrderID });
        }
        /// <summary>
        /// Xóa 1 mặt hàng khỏi giỏ hàng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productID"></param>
        /// <returns></returns>        
        public IActionResult DeleteDetail(int id = 0, int productID = 0)
        {
            if (id < 0)
            {
                return RedirectToAction("Index");
            }
            if (productID < 0)
            {
                return RedirectToAction("Details", new { id = id });
            }

            // Xoá chi tiết 1 đơn hàng nếu kiểm tra đúng hết
            bool isDeleted = OrderDataService.DeleteOrderDetail(id, productID);
            if (!isDeleted)
            {
                //TempData[ERROR_MESSAGE] = "Không thể xoá mặt hàng này";
                return RedirectToAction("Details", new { id = id });
            }
            return RedirectToAction("Details", new { id = id });
        }
        /// <summary>
        /// Xóa đơn hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Delete(int id = 0)
        {
            //TODO: Code chức năng để xóa đơn hàng (nếu được phép xóa)

            if (id < 0)
            {
                return RedirectToAction("Index");
            }
            Order data = OrderDataService.GetOrder(id);
            if (data == null)
            {
                return RedirectToAction("Index");
            }
            // Xoá đơn hàng ở trạng thái vừa tạo, bị huỷ hoặc bị từ chối
            if (data.Status == OrderStatus.INIT
                || data.Status == OrderStatus.CANCEL
                || data.Status == OrderStatus.REJECTED)
            {
                OrderDataService.DeleteOrder(id);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Details", new { id = id });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Accept(int id = 0)
        {
            //TODO: Duyệt chấp nhận đơn hàng

            return RedirectToAction("Details", new { id = id });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Shipping(int id = 0, int shipperID = 0)
        {
            if (Request.Method == "GET")
                return View();
            else
            {
                //TODO: Chuyển đơn hàng cho người giao hàng

                return RedirectToAction("Details", new { id = id });
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Finish(int id = 0)
        {
            //TODO: Ghi nhận hoàn tất đơn hàng

            return RedirectToAction($"Details", new { id = id });
        }
        /// <summary>
        /// Hủy bỏ đơn hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Cancel(int id = 0)
        {
            //TODO: Hủy đơn hàng

            return RedirectToAction($"Details", new { id = id });
        }
        /// <summary>
        /// Từ chối đơn hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Reject(int id = 0)
        {
            //TODO: Từ chối đơn hàng

            return RedirectToAction($"Details", new { id = id });
        }

        
        /// <summary>
        /// Bổ sung thêm hàng vào giỏ hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddToCart(CartItem data)
        {
            try
            {
                var cart = GetCart();
                int index = cart.FindIndex(m => m.ProductId == data.ProductId);
                if (index < 0)
                {
                    cart.Add(data);
                }
                else
                {
                    cart[index].Price = data.Price;
                    cart[index].Quantity += data.Quantity;
                }

                ApplicationContext.SetSessionData(CART, cart);
                return Json("");
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        /// <summary>
        /// Xóa 1 mặt hàng khỏi giỏ hàng
        /// </summary>        
        /// <returns></returns>
        public ActionResult RemoveFromCart(int id)
        {
            var cart = GetCart();
            int index = cart.FindIndex(m => m.ProductId == id);
            if (index >= 0)
                cart.RemoveAt(index);
            ApplicationContext.SetSessionData(CART, cart);
            return Json("");
        }
        /// <summary>
        /// Xóa toàn bộ dữ liệu trong giỏ hàng
        /// </summary>
        /// <returns></returns>
        public ActionResult ClearCart()
        {
            var cart = GetCart();
            cart.Clear();
            ApplicationContext.SetSessionData(CART, cart);
            return Json("");
        }

        private static IEnumerable<OrderDetail> ConvertCartToOrderDetails(List<CartItem> shoppingCart)
        {
            List<OrderDetail> orderDetails = new List<OrderDetail>();

            foreach (var cartItem in shoppingCart)
            {
                OrderDetail orderDetail = new OrderDetail
                {
                    // Thiết lập các thông tin từ CartItem cho OrderDetail
                    // Ví dụ:
                    ProductID = cartItem.ProductId,
                    ProductName = cartItem.ProductName,
                    Unit = cartItem.Unit,
                    Quantity = cartItem.Quantity,
                    SalePrice = cartItem.Price
                };

                orderDetails.Add(orderDetail);
            }

            return orderDetails;
        }
        /// <summary>
        /// Khởi tạo đơn hàng và chuyển đến trang Details sau khi khởi tạo xong để tiếp tục quá trình xử lý đơn hàng
        /// </summary>        
        /// <returns></returns>
        [HttpPost]
        public ActionResult Init(int customerID = 0, int employeeID = 0)
        {
            List<CartItem> shoppingCart = GetCart();
            TempData[CUSTOMERID] = customerID;
            TempData[EMPLOYEEID] = employeeID;
            if (shoppingCart == null || shoppingCart.Count == 0)
            {
                TempData[ERROR_MESSAGE] = "Không thể tạo đơn hàng với giỏ hàng trống";
                return RedirectToAction("Create");
            }

            if (customerID == 0 || employeeID == 0)
            {
                TempData[ERROR_MESSAGE] = "Vui lòng chọn khách hàng và nhân viên phụ trách";
                return RedirectToAction("Create");
            }

            int orderID = OrderDataService.InitOrder(customerID, employeeID, DateTime.Now, ConvertCartToOrderDetails(shoppingCart));

            HttpContext.Session.Remove(CART); //Xóa giỏ hàng 

            return RedirectToAction("Details", new {id = orderID});
        }
    }
}
