using Project.Models;
using Microsoft.AspNetCore.Mvc;
using Project.DataAccess.Data;
using Project.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Project.Utility;

namespace TheReadHaven.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =StaticDetails.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();

            return View(objCategoryList);
        }
        public IActionResult Create()
        {

            return View();

        }
        [HttpPost]

        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.Display_Order.ToString())
            {
                ModelState.AddModelError("name", "Name & Display order can not be same");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                TempData["Success"] = "Category Created Successfully";
                _unitOfWork.Save();
                return RedirectToAction("Index", "Category");
            }

            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }

            Category? categoryFromDB = _unitOfWork.Category.Get(u => u.ID == id);
            // Category? categoryFromDB1 = _db.categories.FirstOrDefault(u => u.ID == id);
            //Category? categoryFromDB2 = _db.categories.Where(u=>u.ID==id).FirstOrDefault();  


            if (categoryFromDB == null)
            {
                return NotFound();
            }

            return View(categoryFromDB);

        }
        [HttpPost]

        public IActionResult Edit(Category obj)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Category Updated Successfully";
                return RedirectToAction("Index", "Category");
            }

            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }

            Category? categoryFromDB = _unitOfWork.Category.Get(u => u.ID == id);
            //Category? categoryFromDB1 = _db.categories.FirstOrDefault(u => u.ID == id);
            //Category? categoryFromDB2 = _db.categories.Where(u => u.ID == id).FirstOrDefault();


            if (categoryFromDB == null)
            {
                return NotFound();
            }

            return View(categoryFromDB);

        }
        [HttpPost, ActionName("Delete")]

        public IActionResult DeletePost(int? id)
        {
            Category? obj = _unitOfWork.Category.Get(u => u.ID == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();
            TempData["Success"] = "Category Deleted Successfully";
            return RedirectToAction("Index", "Category");
        }

    }
}
