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
using Stripe;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Identity;

namespace TheReadHaven.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.Role_Admin)]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDBContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(ApplicationDBContext db,UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;

        }
        public IActionResult Index()
        {

            return View();
        }

        public IActionResult RoleManagement(string userId)
        {
            string RoleID = _db.UserRoles.FirstOrDefault(u => u.UserId == userId).RoleId;
           
            RoleManagement roleManagement = new RoleManagement()
            {
                ApplicationUser = _db.ApplicationUsers.Include(u => u.Company).FirstOrDefault(u => u.Id == userId),
                RoleList = _db.Roles.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Name
                }),
                CompanyList = _db.Company.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.ID.ToString()
                }),

            };

            roleManagement.ApplicationUser.Role = _db.Roles.FirstOrDefault(u => u.Id == RoleID).Name;
            return View(roleManagement);
        }

        [HttpPost]

        public IActionResult RoleManagement(RoleManagement roleManagement)
        {
            string RoleID = _db.UserRoles.FirstOrDefault(u => u.UserId == roleManagement.ApplicationUser.Id).RoleId;
            string oldRole = _db.Roles.FirstOrDefault(u => u.Id == RoleID).Name;
            if (!(roleManagement.ApplicationUser.Role == oldRole))
            {
                ApplicationUser applicationUser= _db.ApplicationUsers.FirstOrDefault(u=>u.Id==roleManagement.ApplicationUser.Id);
              if(roleManagement.ApplicationUser.Role == StaticDetails.Role_Company) {
                applicationUser.CompanyId=roleManagement.ApplicationUser.CompanyId;
                
                }
                if (oldRole == StaticDetails.Role_Company)
                {
                    applicationUser.CompanyId = null;
                }
            _db.SaveChanges();
                _userManager.RemoveFromRoleAsync(applicationUser, oldRole).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(applicationUser,roleManagement.ApplicationUser.Role).GetAwaiter().GetResult();
            }
            return RedirectToAction("Index");
        }


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<ApplicationUser> objUserList = _db.ApplicationUsers.Include(u => u.Company).ToList();

            var userRoles = _db.UserRoles.ToList();
            var roles=_db.Roles.ToList();

            foreach (var user in objUserList)
            {
                var roleId = userRoles.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;

                if (user.Company == null)
                {
                    user.Company = new Company()
                    {
                        Name = ""
                };
            }
        }

    

            return Json(new { data = objUserList });

        }


        [HttpPost]
        public IActionResult LockUnlock([FromBody]string id)
        {
            var objFromDb=_db.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if (objFromDb==null)
            {
                return Json(new { success = false, message = "Error while Locking/Unlocking" });
            }

            if(objFromDb.LockoutEnd!=null && objFromDb.LockoutEnd>DateAndTime.Now) {
            objFromDb.LockoutEnd = DateAndTime.Now;
            }
            else
            {
                objFromDb.LockoutEnd = DateAndTime.Now.AddYears(1000);
            }
            _db.SaveChanges();
            return Json(new { success = true, message = "changes saved successfully.." });
                }
        #endregion
    }
}
