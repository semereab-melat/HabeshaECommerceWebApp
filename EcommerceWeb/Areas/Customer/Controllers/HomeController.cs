using DukanHabeshaDataAccess.Repositories.IRepositories;
using DukanHabeshaModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DukanHabeshaWeb.Areas.Customer.Controllers
{
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


        public IActionResult Cloths()
        {
            IEnumerable<Product> clothslist = _unitOfWork.Product.GetAll().Where(cloths => cloths.Category.Name == "Habesha Cloths");
            return View(clothslist);


            //this is  something i need to try at the repositry class in productrepositroy i guess
            /*{
                if (repositories.Keys.Contains(typeof(T)) == true)
                    return repositories[typeof(T)] as IRepository<T>;

                IRepository<T> repo = new GenericRepository<T>(context);
                repositories.Add(typeof(T), repo);
                return repo;
            }*/
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