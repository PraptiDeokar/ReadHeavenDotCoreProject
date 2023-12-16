using Project.Models;
using Microsoft.AspNetCore.Mvc;
using Project.DataAccess.Data;
using Project.DataAccess.Repository.IRepository;
using Project.Models.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project.Models.ViewModels;
using Project.DataAccess.Repository;
using NuGet.Protocol.Plugins;
using Microsoft.AspNetCore.Authorization;
using Project.Utility;

namespace TheReadHaven.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {

            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();

            return View(objProductList);
        }
        public IActionResult Upsert(int? id)
        {

            // ViewBag.CategoryList = CategoryList;
            // ViewData["CategoryList"] = CategoryList;
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.ID.ToString(),
                }),
                Product = new Product()

            };
            if (id == null || id == 0)
            {
                //insert
                return View(productVM);
            }
            else
            {
                // update
                productVM.Product = _unitOfWork.Product.Get(u => u.ID == id);
                return View(productVM);
            }

        }
        [HttpPost]

        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {

            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"Images\product");

                    //updating img
                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        //delete old img
                        var oldImgPath =
                            Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImgPath))
                        {
                            System.IO.File.Delete(oldImgPath);
                        }

                    }


                    using (var filestream = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    productVM.Product.ImageUrl = @"\Images\product\" + filename;
                }

                if (productVM.Product.ID == 0)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                    TempData["Success"] = "Product Created Successfully";
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                    TempData["Success"] = "Product Updated Successfully";
                }


                // _unitOfWork.Product.Add(productVM.Product);

                _unitOfWork.Save();
                return RedirectToAction("Index", "Product");
            }
            else
            {
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u =>
                new SelectListItem
                {
                    Text = u.Name,
                    Value = u.ID.ToString(),
                });

                return View(productVM);

            }
        }


        //public IActionResult Edit(int? id)
        //{
        //    if (id == 0 || id == null)
        //    {
        //        return NotFound();
        //    }

        //    Product? productFromDB = _unitOfWork.Product.Get(u => u.ID == id);
        //    // Category? categoryFromDB1 = _db.categories.FirstOrDefault(u => u.ID == id);
        //    //Category? categoryFromDB2 = _db.categories.Where(u=>u.ID==id).FirstOrDefault();  


        //    if (productFromDB == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(productFromDB);

        //}
        //[HttpPost]

        //public IActionResult Edit(Product obj)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Product.Update(obj);
        //        _unitOfWork.Save();
        //        TempData["Success"] = "Product Updated Successfully";
        //        return RedirectToAction("Index", "Product");
        //    }

        //    return View();
        //}

        //public IActionResult delete(int? id)
        //{
        //    if (id == 0 || id == null)
        //    {
        //        return NotFound();
        //    }

        //    Product? productFromDB = _unitOfWork.Product.Get(u => u.ID == id);
        //    //Category? categoryFromDB1 = _db.categories.FirstOrDefault(u => u.ID == id);
        //    //Category? categoryFromDB2 = _db.categories.Where(u => u.ID == id).FirstOrDefault();


        //    if (productFromDB == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(productFromDB);

        //}

        //[HttpPost, ActionName("Delete")]

        //public IActionResult DeletePost(int? id)
        //{
        //    Product? obj = _unitOfWork.Product.Get(u => u.ID == id);
        //    if (obj == null)
        //    {
        //        return NotFound();
        //    }
        //    _unitOfWork.Product.Remove(obj);
        //    _unitOfWork.Save();
        //    TempData["Success"] = "Product Deleted Successfully";
        //    return RedirectToAction("Index", "Product");
        //}



        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProductList });

        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var ProductToBeDeleted = _unitOfWork.Product.Get(u => u.ID == id);

            if (ProductToBeDeleted == null)
            {
                return Json(new { success = false , message = "Error while deleting"});
            }
            var oldImgPath =
                           Path.Combine(_webHostEnvironment.WebRootPath,
                           ProductToBeDeleted.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImgPath))
            {
                System.IO.File.Delete(oldImgPath);
            }
            _unitOfWork.Product.Remove(ProductToBeDeleted); 
            _unitOfWork.Save();
            return Json(new { success = true, message = "Deleted Successfully" });

        }
        #endregion
    }
}
