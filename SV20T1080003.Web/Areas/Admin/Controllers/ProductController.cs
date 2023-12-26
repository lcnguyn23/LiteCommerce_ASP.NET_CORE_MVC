using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1080003.BusinessLayer;
using SV20T1080003.BusinessLayers;
using SV20T1080003.DataLayers.SQLServer;
using SV20T1080003.DomainModels;
using SV20T1080003.Web.AppCodes;
using SV20T1080003.Web.Models;
using System.Drawing.Printing;
using System.Reflection;

namespace SV20T1080003.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize(Roles = $"{WebUserRoles.Administrator}")]
    [Area("Admin")]
    public class ProductController : Controller
    {
        private const int PAGE_SIZE = 3;
        private const string PRODUCT_SEARCH = "Product_Search";
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //public IActionResult Index(int page = 1, string searchValue = "", int categoryId = 0, int supplierId = 0, decimal minPrice = 0, decimal maxPrice = 0)
        //{
        //    int rowCount = 0;
        //    var data = ProductDataService.ListProducts(page, PAGE_SIZE, searchValue ?? "", categoryId, supplierId, minPrice, maxPrice, out rowCount);
        //    var model = new PaginationSearchProduct()
        //    {
        //        Page = page,
        //        PageSize = PAGE_SIZE,
        //        SearchValue = searchValue ?? "",
        //        CategoryId = categoryId,
        //        SupplierId = supplierId,
        //        RowCount = rowCount,
        //        Data = data
        //    };

        //    string? errorMessage = Convert.ToString(TempData["ErrorMessage"]);
        //    ViewBag.ErrorMessage = errorMessage;

        //    return View(model);
        //}

        public IActionResult Index()
        {
            var input = ApplicationContext.GetSessionData<PaginationSearchProductInput>(PRODUCT_SEARCH);
            if (input == null)
            {
                input = new PaginationSearchProductInput()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = "",
                    CategoryId = 0,
                    SupplierId = 0,
                };
            }
            return View(input);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public IActionResult Search(PaginationSearchProductInput input)
        {
            int rowCount = 0;
            var data = ProductDataService.ListProducts(
                                            input.Page,
                                            input.PageSize,
                                            input.SearchValue ?? "",
                                            input.CategoryId,
                                            input.SupplierId,
                                            input.MinPrice,
                                            input.MaxPrice,
                                            out rowCount
                                            );
            var model = new PaginationSearchProduct()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                RowCount = rowCount,
                Data = data
            };

            ApplicationContext.SetSessionData(PRODUCT_SEARCH, model);

            return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            var data = new Product()
            {
                ProductId = 0
            };
            var model = new ProductDetail()
            {
                Product = data
            }; 
            ViewBag.Title = "Bổ sung mặt hàng";
            return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Edit(int id = 0)
        {
            var model = ProductDataService.GetProduct(id);
            if (model == null)
                return RedirectToAction("Index");
            var dataModel = new ProductDetail()
            {
                Product = model,
                ListPhotos = ProductDataService.ListPhotos(id),
                ListAttributes = ProductDataService.ListAttributes(id),
            };
            ViewBag.Title = "Cập nhật mặt hàng";
            return View("Create", dataModel);
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="uploadPhoto"></param>
        /// <returns></returns>
        public IActionResult Save(ProductDetail data, IFormFile? uploadPhoto)
        {
            ViewBag.Title = data.Product.ProductId == 0 ? "Bổ sung mặt hàng" : "Cập nhật mặt hàng";

            if (string.IsNullOrWhiteSpace(data.Product.ProductName))
                ModelState.AddModelError(nameof(data.Product.ProductName), "Tên mặt hàng không được rỗng");
            if (string.IsNullOrWhiteSpace(data.Product.CategoryId.ToString()))
                ModelState.AddModelError(nameof(data.Product.CategoryId), "Vui lòng chọn loại hàng");
            if (string.IsNullOrWhiteSpace(data.Product.SupplierId.ToString()))
                ModelState.AddModelError(nameof(data.Product.SupplierId), "Vui lòng chọn tỉnh thành");
            if (string.IsNullOrWhiteSpace(data.Product.Unit))
                ModelState.AddModelError(nameof(data.Product.Unit), "Đơn vị tính không được rỗng");
            if (string.IsNullOrWhiteSpace(data.Product.Price.ToString()))
                ModelState.AddModelError(nameof(data.Product.Price), "Giá hàng không được rỗng");

            //Xử lý với ảnh
            //Upload ảnh lên (nếu có), sau khi upload xong thì mới lấy tên file ảnh vừa upload
            //để gán cho trường Photo của Employee
            if (uploadPhoto != null)
            {
                string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}";
                string filePath = System.IO.Path.Combine(ApplicationContext.HostEnviroment.WebRootPath, @"images\products", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadPhoto.CopyTo(stream);
                }
                data.Product.Photo = fileName;
            }


            if (!ModelState.IsValid)
            {
                return View("Create", data);
            }

            if (data.Product.ProductId == 0)
            {
                int productId = ProductDataService.AddProduct(data.Product);
                if (productId > 0)
                {
                    return RedirectToAction("Index");
                }
                ViewBag.ErrorMessage = "Không bổ sung được dữ liệu";
                return View("Create", data);
            }
            else
            {
                bool success = ProductDataService.UpdateProduct(data.Product);
                if (success)
                {
                    return RedirectToAction("Index");
                }
                ViewBag.ErrorMessage = "Không cập nhật được dữ liệu";
                return View("Create", data);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                bool success = ProductDataService.DeleteProduct(id);
                if (!success)
                    TempData["ErrorMessage"] = "Không thể xóa mặt hàng này";
                return RedirectToAction("Index");
            }
            var model = ProductDataService.GetProduct(id);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="method"></param>
        /// <param name="photoId"></param>
        /// <returns></returns>
        public IActionResult Photo(int id = 0, string method = "add", long photoId = 0)
        {
            if (id < 0)
            {
                return RedirectToAction("Index");
            }
            ProductPhoto data = null;
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung ảnh";
                    data = new ProductPhoto()
                    {
                        PhotoId = 0,
                        ProductId = id
                    };
                    return View(data);
                case "edit":
                    ViewBag.Title = "Thay đổi ảnh";
                    if (photoId < 0)
                    {
                        return RedirectToAction("Index");
                    }
                    data = ProductDataService.GetPhoto(photoId);
                    if (data == null)
                    {
                        return RedirectToAction("index");
                    }
                    return View(data);
                case "delete":
                    ProductDataService.DeletePhoto(photoId);
                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Index");
            }
        }

        public IActionResult SavePhoto(ProductPhoto data, IFormFile? uploadPhoto)
        {
            
            if (string.IsNullOrWhiteSpace(data.DisplayOrder.ToString()))
                ModelState.AddModelError(nameof(data.DisplayOrder), "Vui lòng chọn thứ tự hiển thị");

            if (data.DisplayOrder < 1)
                ModelState.AddModelError("DisplayOrder", "Thứ tự hiển thị phải là một số tự nhiên dương");

            List<ProductPhoto> productPhotos = ProductDataService.ListPhotos(data.ProductId);
            bool isUsedDisplayOrder = false;
            foreach (ProductPhoto item in productPhotos)
            {
                if (item.DisplayOrder == data.DisplayOrder && data.PhotoId != item.PhotoId)
                {
                    isUsedDisplayOrder = true;
                    break;
                }
            }
            if (isUsedDisplayOrder)
            {
                ModelState.AddModelError("DisplayOrder",
                    $"Thứ tự hiển thị {data.DisplayOrder} của hình ảnh đã được sử dụng trước đó");
            }

            data.Description = data.Description ?? "";
            data.IsHidden = Convert.ToBoolean(data.IsHidden.ToString());

            //Xử lý với ảnh
            //Upload ảnh lên (nếu có), sau khi upload xong thì mới lấy tên file ảnh vừa upload
            //để gán cho trường Photo của Employee
            if (uploadPhoto != null)
            {
                string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}";
                string filePath = System.IO.Path.Combine(ApplicationContext.HostEnviroment.WebRootPath, @"images\productphotos", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadPhoto.CopyTo(stream);
                }
                data.Photo = fileName;
            }


            if (!ModelState.IsValid)
            {
                ViewBag.Title = data.PhotoId == 0 ? "Bổ sung ảnh" : "Thay đổi ảnh";
                return View("Photo", data);
            }

            // thực hiện thêm hoặc cập nhật
            if (data.PhotoId == 0)
            {
                ProductDataService.AddPhoto(data);
            }
            else
            {
                ProductDataService.UpdatePhoto(data);
            }
            return RedirectToAction("Edit", new { id = data.ProductId });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="method"></param>
        /// <param name="attributeId"></param>
        /// <returns></returns>
        public IActionResult Attribute(int id = 0, string method = "add", int attributeId = 0)
        {
            if (id < 0)
            {
                return RedirectToAction("Index");
            }
            ProductAttribute data = null;
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung thuộc tính";
                    data = new ProductAttribute()
                    {
                        AttributeId = 0,
                        ProductId = id,
                    };
                    return View(data);
                case "edit":
                    ViewBag.Title = "Thay đổi thuộc tính";
                    if (attributeId < 0)
                    {
                        return RedirectToAction("Index");
                    }
                    data = ProductDataService.GetAttribute(attributeId);
                    if (data == null)
                    {
                        return RedirectToAction("Index");
                    }
                    return View(data);
                case "delete":
                    ProductDataService.DeleteAttribute(attributeId);
                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ActionResult SaveAttribute(ProductAttribute data)
        {
            // kiểm tra dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(data.AttributeName))
            {
                ModelState.AddModelError(nameof(data.AttributeName), "Tên thuộc tính không được để trống");
            }
            if (string.IsNullOrWhiteSpace(data.AttributeValue))
            {
                ModelState.AddModelError(nameof(data.AttributeValue), "Giá trị thuộc tính không được để trống");
            }

            if (string.IsNullOrWhiteSpace(data.DisplayOrder.ToString()))
            {
                ModelState.AddModelError("DisplayOrder", "Thứ tự hiển thị thuộc tính không được để trống");
            }
            else if (data.DisplayOrder < 1)
            {
                ModelState.AddModelError("DisplayOrder", "Thứ tự hiển thị thuộc tính phải > 0");
            }
            List<ProductAttribute> productAttributes = ProductDataService.ListAttributes(data.ProductId);
            bool isUsedDisplayOrder = false;
            foreach (ProductAttribute item in productAttributes)
            {
                if (item.DisplayOrder == data.DisplayOrder && data.AttributeId != item.AttributeId)
                {
                    isUsedDisplayOrder = true;
                    break;
                }
            }
            if (isUsedDisplayOrder)
            {
                ModelState.AddModelError("DisplayOrder",
                        $"Thứ tự hiển thị {data.DisplayOrder} của thuộc tính đã được sử dụng trước đó");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Title = data.AttributeId == 0 ? "Bổ sung thuộc tính" : "Thay đổi thuộc tính";
                return View("Attribute", data);
            }

            // thực hiện thêm hoặc cập nhật
            if (data.AttributeId == 0)
            {
                ProductDataService.AddAttribute(data);
            }
            else
            {
                ProductDataService.UpdateAttribute(data);
            }
            return RedirectToAction("Edit", new { id = data.ProductId });
        }
    }
}
