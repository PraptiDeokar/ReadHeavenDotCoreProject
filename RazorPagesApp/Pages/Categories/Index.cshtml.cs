using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesApp.Data;
using RazorPagesApp.Models;

namespace RazorPagesApp.Pages.Categories
{
    public class IndexModel : PageModel
    {

        private readonly ApplicationDBContext _db;
        public List<Category> CategoryList { get; set; }

        public IndexModel(ApplicationDBContext db)
        {
                _db = db;
        }
        public void OnGet()
        {
            CategoryList=_db.categories.ToList();
        }
    }
}
