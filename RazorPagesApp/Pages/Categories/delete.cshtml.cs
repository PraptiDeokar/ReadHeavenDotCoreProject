using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesApp.Data;
using RazorPagesApp.Models;

namespace RazorPagesApp.Pages.Categories
{
    public class deleteModel : PageModel
    {

        private readonly ApplicationDBContext _db;

        [BindProperty]
        public Category? Category { get; set; }

        public deleteModel(ApplicationDBContext db)
        {
            _db = db;
        }


        public void OnGet(int? id)
        {
            if (id != 0 || id != null)
            {
                Category = _db.categories.Find(id);
            }

   }

        public IActionResult OnPost(int? id)
        {
            Category? obj = _db.categories.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.categories.Remove(obj);
            _db.SaveChanges();
           TempData["Success"] = "Category Deleted Successfully";
            return RedirectToPage("Index");
         
        }


    }
}
