
using DukanHabeshaDataAccess.Repository.IRepository;
using DukanHabeshaModels;
using DukanHabeshaModels.ViewModels;
using DukanHabeshaUtility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Security.Claims;

namespace DukanHabeshaWebApp.Areas.Customer.Controllers
{

    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public ShoppingCartVM shopingCartVM { get; set; }

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            //these two lines of code helps to get the user and his claims
            //we can get the user Id through these two lines of code
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            shopingCartVM = new ShoppingCartVM()
            {
                ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value,
                includeProperties:"Product"),
                OrderHeader = new ()
            };

            foreach (var cart in shopingCartVM.ListCart)
            {

                if (cart.Count > 0)
                {
                    cart.Price = ((double)(cart.Count * cart.Product.Price));
                    shopingCartVM.OrderHeader.OrderTotal += (cart.Price);
                }
                
            }

            return View(shopingCartVM);
        }


        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            shopingCartVM = new ShoppingCartVM()
            {
                ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value,
                includeProperties: "Product"),
                OrderHeader = new()
            };
            shopingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(
                u => u.Id == claim.Value);

            shopingCartVM.OrderHeader.Name = shopingCartVM.OrderHeader.ApplicationUser.Name;
            shopingCartVM.OrderHeader.PhoneNumber = shopingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            shopingCartVM.OrderHeader.StreetAddress = shopingCartVM.OrderHeader.ApplicationUser.StreetAddress;
            shopingCartVM.OrderHeader.City = shopingCartVM.OrderHeader.ApplicationUser.City;
            shopingCartVM.OrderHeader.State = shopingCartVM.OrderHeader.ApplicationUser.State;
            shopingCartVM.OrderHeader.PostalCode = shopingCartVM.OrderHeader.ApplicationUser.PostalCode;



            foreach (var cart in shopingCartVM.ListCart)
            {
                /*cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price,
                    cart.Product.Price50, cart.Product.Price100);*/
                if(cart.Count >0)
                {
                    cart.Price = ((double)(cart.Count * cart.Product.Price));
                    shopingCartVM.OrderHeader.OrderTotal += (cart.Price);
                }
               
            }
            return View(shopingCartVM);


        }




        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public IActionResult SummaryPOST()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            shopingCartVM.ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value,
                includeProperties: "Product");


            shopingCartVM.OrderHeader.OrderDate = System.DateTime.Now;
            shopingCartVM.OrderHeader.ApplicationUserId = claim.Value;


            foreach (var cart in shopingCartVM.ListCart)
            {
                /*cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price,
                    cart.Product.Price50, cart.Product.Price100);*/
                if(cart.Count > 0)
                {
                    cart.Price = ((double)(cart.Count * cart.Product.Price));
                    shopingCartVM.OrderHeader.OrderTotal += (cart.Price);
                }
                
                
            }
            ApplicationUser applicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value);

            if (applicationUser != null)
            {
                shopingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
                shopingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
            }
            /* else
             {
                 shopingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
                 shopingCartVM.OrderHeader.OrderStatus = SD.StatusApproved;
             }*/

            _unitOfWork.OrderHeader.Add(shopingCartVM.OrderHeader);
            _unitOfWork.Save();
            foreach (var cart in shopingCartVM.ListCart)
            {
                OrderDetail orderDetail = new()
                {
                    ProductId = cart.ProductId,
                    OrderId = shopingCartVM.OrderHeader.Id,
                    Price = (double)cart.Product.Price,
                    Count = cart.Count
                };
                _unitOfWork.OrderDetail.Add(orderDetail);
                _unitOfWork.Save();
            }


            /* if (applicationUser.CompanyId.GetValueOrDefault() == 0)
             {*/
                 //stripe settings 
                 var domain = "https://localhost:44355/";
                 var options = new SessionCreateOptions
                 {
                     PaymentMethodTypes = new List<string>
                 {
                   "card",
                 },
                     LineItems = new List<SessionLineItemOptions>(),
                     Mode = "payment",
                     SuccessUrl = domain+$"Customer/Cart/OrderConfirmation?id={shopingCartVM.OrderHeader.Id}",
                     CancelUrl = domain+$"Customer/Cart/index",
                 };

                 foreach (var item in shopingCartVM.ListCart)
                 {

                     var sessionLineItem = new SessionLineItemOptions
                     {
                         PriceData = new SessionLineItemPriceDataOptions
                         {
                             UnitAmount = (long)(item.Product.Price * 100),//20.00 -> 2000
                             Currency = "gbp",
                             ProductData = new SessionLineItemPriceDataProductDataOptions
                             {
                                 Name = item.Product.Name
                             },

                         },
                         Quantity = item.Count,
                     };
                     options.LineItems.Add(sessionLineItem);

                 }

                 var service = new SessionService();
                 Session session = service.Create(options);
                 _unitOfWork.OrderHeader.UpdateStripePaymentID(shopingCartVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
                 _unitOfWork.Save();
                 Response.Headers.Add("Location", session.Url);
                 return new StatusCodeResult(303);
      

        }

        public IActionResult OrderConfirmation(int id)
        //, includeProperties:"ApplicationUser"
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == id);
            
                var service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);
                //check the stripe status
                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.OrderHeader.Updatestatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
                    _unitOfWork.Save();
                }
            
           // _emailSender.SendEmailAsync(orderHeader.ApplicationUser.Email, "New Order - Bulky Book", "<p>New Order Created</p>");
            List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId ==
            orderHeader.ApplicationUserId).ToList();
            HttpContext.Session.Clear();
            _unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
            _unitOfWork.Save();
            return View(id);

        }


        public IActionResult Plus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);
            _unitOfWork.ShoppingCart.IncrementCount(cart, 1);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);

            if (cart.Count <=1)
            {
                _unitOfWork.ShoppingCart.Remove(cart);
                var count = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count - 1;
                HttpContext.Session.SetInt32(SD.SessionCart, count);
            }
            else
            {
                _unitOfWork.ShoppingCart.DecrementCount(cart, 1);

            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);
            _unitOfWork.ShoppingCart.Remove(cart);
            _unitOfWork.Save();

            var count = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count;
            HttpContext.Session.SetInt32(SD.SessionCart, count);

            return RedirectToAction(nameof(Index));
        }



        /*private double GetPriceBasedOnQuantity(double quantity, double price)
        {
            if (quantity >= 0)
            {
                return price;
            }
            
        }*/
    }
}
