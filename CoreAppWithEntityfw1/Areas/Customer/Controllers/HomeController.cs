using Project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Project.DataAccess.Repository.IRepository;
using Project.Models.Models;

namespace TheReadHaven.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(ILogger<HomeController> logger,IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;

        }

        public IActionResult Index()
        {
            IEnumerable<Product> ProductList=_unitOfWork.Product.GetAll(includeProperties:"Category");
            return View(ProductList);
        }
        public IActionResult Details(int Productid)
        {
            ShoppingCart cart = new()
            {
                Product = _unitOfWork.Product.Get(u => u.ID == Productid, includeProperties: "Category"),
                Count = 1,
                ProductId = Productid
            };
        return View(cart);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}