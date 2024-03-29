using Project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Project.DataAccess.Repository.IRepository;
using Project.Models.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Project.Utility;

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

        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity=(ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = userId;

            ShoppingCart cardFromDb = _unitOfWork.ShoppingCart.Get(u => u.ApplicationUserId == userId && 
            u.ProductId == shoppingCart.ProductId);

            if(cardFromDb != null ) {
                //ShoppingCart exists
                cardFromDb.Count += shoppingCart.Count;
                _unitOfWork.ShoppingCart.Update(cardFromDb);
                _unitOfWork.Save();
            }
            else
            {
                
                _unitOfWork.ShoppingCart.Add(shoppingCart);
                _unitOfWork.Save();
                HttpContext.Session.SetInt32(StaticDetails.SessionCart,
                _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId).Count());
            }

            TempData["success"] = "Cart updated successfully";
            return RedirectToAction(nameof(Index));
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