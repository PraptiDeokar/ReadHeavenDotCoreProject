using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesApp.Data;
using RazorPagesApp.Models;

namespace RazorPagesApp.Pages.Categories
{
    public class editModel : PageModel
    {
        private readonly ApplicationDBContext _db;
        
        [BindProperty]
        public Category? Category { get; set; }

        public editModel(ApplicationDBContext db)
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

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                _db.categories.Update(Category);
                _db.SaveChanges();
                TempData["Success"] = "Category Updated Successfully";
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}
