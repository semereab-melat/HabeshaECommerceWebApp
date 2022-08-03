using DukanHabeshaDataAccess.Repositories.IRepositories;
using DukanHabeshaModels;
using Microsoft.AspNetCore.Mvc;

namespace DukanHabeshaWeb.Areas.Customer.Controllers
{
    public class ListItemsErrorController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public ListItemsErrorController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

       /* public IActionResult Cloths()
        {
            IEnumerable<Product> clothslist = _unitOfWork.Product.GetAll().Where(cloths => cloths.Category.Name == "Habesha Cloths");
            return View(clothslist);
        }*/
    }
}
