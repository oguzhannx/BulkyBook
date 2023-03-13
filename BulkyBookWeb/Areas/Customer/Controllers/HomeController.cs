using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utilty;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BulkyBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType");
            return View(productList);

        }

        //get
        public IActionResult Details(int productId)
        {
            ShoppingCart cartObj = new()
            {
                Count = 1,
                ProductId = productId,
                Product = _unitOfWork.Product.GetFirstOrDefault(i => i.Id == productId, includeProperties: "Category,CoverType")
            };
            //var product = _unitOfWork.Product.GetFirstOrDefault(i => i.Id == id, includeProperties: "Category,CoverType");
            return View(cartObj);
        }

        //httpPost
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart.ApplicationUserId = claim.Value;

            ShoppingCart shoppingCartFromDb = _unitOfWork.ShoppingCart.GetFirstOrDefault(
                i => i.ApplicationUserId == claim.Value && i.ProductId == shoppingCart.ProductId);

            if (shoppingCartFromDb == null)
            {
                _unitOfWork.ShoppingCart.Add(shoppingCart);
                _unitOfWork.Save();

                HttpContext.Session.SetInt32(SD.SessionCart,
                    _unitOfWork.ShoppingCart.GetAll(u=> u.ApplicationUserId == claim.Value).ToList().Count);
            }
            else
            {
                _unitOfWork.ShoppingCart.IncrementCount(shoppingCartFromDb, shoppingCart.Count);
                _unitOfWork.Save();


            }


            //var product = _unitOfWork.Product.GetFirstOrDefault(i => i.Id == id, includeProperties: "Category,CoverType");
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