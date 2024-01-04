using Microsoft.AspNetCore.Identity;
using Project.DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Project.Utility;
using Project.Models;

namespace Project.DataAccess.DBInitializer
{
    public class DBInitializer : IDBInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDBContext _db;

        public DBInitializer(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDBContext db)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;   
        }
        

        public void Initialize()
        {
            //migrations if they are not applied
            //try
            //{

            //    if (_db.Database.GetPendingMigrations().Count() > 0)
            //    {
            //        _db.Database.Migrate();
            //    }
            //}
            //catch (Exception ex)
            //{

            //}



            //create roles if they are created
            if (!_roleManager.RoleExistsAsync(StaticDetails.Role_Customer).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Customer)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Employee)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Company)).GetAwaiter().GetResult();

                //if roles are not created,then we will create admin user as well*/
                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "admin@readheaven.com",
                    Email = "admin@readheaven.com",
                    Name = "Prapti",
                    PhoneNumber = "4322323232",
                    StreetAddress = "Sector 21",
                    State = "MH",
                    PostalCode = "420221",
                    CIty = "Mumbai"
                }, "Admin123*").GetAwaiter().GetResult();
               
               ApplicationUser user=_db.ApplicationUsers.FirstOrDefault(u=>u.Email== "admin@readheaven.com");
                _userManager.AddToRoleAsync(user,StaticDetails.Role_Admin).GetAwaiter().GetResult() ;


            }

            return;

        }




       

          



    }
}
