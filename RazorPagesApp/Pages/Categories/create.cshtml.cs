using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesApp.Data;
using RazorPagesApp.Models;

namespace RazorPagesApp.Pages.Categories
{
    [BindProperties]
    public class createModel : PageModel
    {
        private readonly ApplicationDBContext _db;
        public Category Category { get; set; }

        public createModel(ApplicationDBContext db)
        {
            _db = db;
        }

        public void OnGet()
        {

        }
        public IActionResult OnPost()
        {
            _db.categories.Add(Category);
            _db.SaveChanges();
            TempData["Success"] = "Category created Successfully";
            return RedirectToPage("Index");
        }

    }
}
