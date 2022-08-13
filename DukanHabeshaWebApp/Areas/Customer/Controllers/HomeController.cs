
using DukanHabeshaDataAccess.Repository.IRepository;
using DukanHabeshaModels;
using DukanHabeshaUtility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace DukanHabeshaWebApp.Areas.Customer.Controllers
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
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category,origin");
            return View(productList);
        }

        public IActionResult Details(int productId)
        {
            ShoppingCart cartObj = new()
            {
                Count = 1,
                /* ProductI = productId,*/
                Product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == productId, includeProperties: "Category,origin")
            };
            return View(cartObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize] //only a user with log in credential can access this action

        
        //since we have a hidden ProductId property in Details action of httpGet, we need to include 
        //it in the post action method of Details
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            //User always found in Controller as a default
            //userID is can get from ClaimsIdentity which is inside User Default Implimentation
            
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart.ApplicationUserId = claim.Value;

           ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.GetFirstOrDefault(
                u=> u.ApplicationUserId==claim.Value && u.ProductId==shoppingCart.ProductId);

            


            if (cartFromDb == null)
            {
                _unitOfWork.ShoppingCart.Add(shoppingCart);
                _unitOfWork.Save();
                //adding session so that i can track number of items added to the cart
                HttpContext.Session.SetInt32(SD.SessionCart,
                _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value).ToList().Count);

            }
           
            else
            {
                _unitOfWork.ShoppingCart.IncrementCount(cartFromDb , shoppingCart.Count);
                _unitOfWork.Save();

            }                   

            return RedirectToAction(nameof(Index));
        }




        public IActionResult GetClothsCategory()
        {

            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category,origin");        
            productList = productList.Where(u => u.Category.Name.ToUpper().Contains("CLOTHS"));
            return View(productList);

        }

        public IActionResult GetJewleryCategory()
        {

            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category,origin");
            productList = productList.Where(u => u.Category.Name.ToUpper().Contains("JEWLERY"));
            return View(productList);

        }

        public IActionResult GetHouseDecorationCategory()
        {

            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category,origin");
            productList = productList.Where(u => u.Category.Name.ToUpper().Contains("HOUSE DECORATION"));
            return View(productList);

        }

        public IActionResult GetFoodStuffCategory()
        {

            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category,origin");
            productList = productList.Where(u => u.Category.Name.ToUpper().Contains("FOOD STUFF"));
            return View(productList);

        }

        public IActionResult GetAntiqueCategory()
        {

            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category,origin");
            productList = productList.Where(u => u.Category.Name.Contains("Antique"));
            return View(productList);

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