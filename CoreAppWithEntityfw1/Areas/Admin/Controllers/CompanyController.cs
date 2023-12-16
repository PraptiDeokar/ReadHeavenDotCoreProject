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
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
       
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
           
        }
        public IActionResult Index()
        {

            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();

            return View(objCompanyList);
        }
        public IActionResult Upsert(int? id)
        {

            
            if (id == null || id == 0)
            {
                //insert
                return View(new Company());
            }
            else
            {
                // update
                Company companyobj = _unitOfWork.Company.Get(u => u.ID == id);
                return View(companyobj);
            }

        }
        [HttpPost]

        public IActionResult Upsert(Company companyobj)
        {

            if (ModelState.IsValid)
            {
                
                if (companyobj.ID == 0)
                {
                    _unitOfWork.Company.Add(companyobj);
                    TempData["Success"] = "Company Created Successfully";
                }
                else
                {
                    _unitOfWork.Company.Update(companyobj);
                    TempData["Success"] = "Company Updated Successfully";
                }


                // _unitOfWork.Company.Add(CompanyVM.Company);

                _unitOfWork.Save();
                return RedirectToAction("Index", "Company");
            }
            else
            {
                
                return View(companyobj);

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
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            return Json(new { data = objCompanyList });

        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var CompanyToBeDeleted = _unitOfWork.Company.Get(u => u.ID == id);

            if (CompanyToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.Company.Remove(CompanyToBeDeleted);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Deleted Successfully" });

        }
        #endregion
    }
}
