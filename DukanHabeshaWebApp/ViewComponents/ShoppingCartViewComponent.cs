
using DukanHabeshaDataAccess.Repository.IRepository;
using DukanHabeshaUtility;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DukanHabeshaWebApp.ViewComponents
{
    public class ShoppingCartViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
               //if the session is already set we can use GetInt AND The Return from all these if-else statment is integer. so our view shoul use @model int
                if (HttpContext.Session.GetInt32(SD.SessionCart) != null)
                {
                    return View(HttpContext.Session.GetInt32(SD.SessionCart));
                }
                else
                {
                    //in this case we are setting the session from database AND The Return from all these if-else statment is integer. so our view shoul use @model int
                    HttpContext.Session.SetInt32(SD.SessionCart,
                        _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value).ToList().Count);
                    return View(HttpContext.Session.GetInt32(SD.SessionCart));
                }
            }
            else
            {
                //AND The Return from all these if-else statment is integer. so our view shoul use @model int
                HttpContext.Session.Clear();
                return View(0);
            }
        }
    }
}
