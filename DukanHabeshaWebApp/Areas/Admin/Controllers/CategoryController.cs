
using DukanHabeshaDataAccess.Repository.IRepository;
using DukanHabeshaModels;
using DukanHabeshaUtility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DukanHabeshaWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {

        //private readonly ApplicationDbContext _db;
       // private readonly ICategoryRepository _db;
        private readonly IUnitOfWork _unitwork;

        public CategoryController(IUnitOfWork unitwork)
        {
            _unitwork = unitwork;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> objCatagoriesList = _unitwork.Category.GetAll();
            return View(objCatagoriesList);
        }

        //Get
        public IActionResult Create()
        {
            return View();
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The Name can't exactly match the Display Order");
            }

            if (ModelState.IsValid)
            {
               _unitwork.Category.Add(obj);
               _unitwork.Save();
                TempData["success"] = "Category Created Successfully!";
                return RedirectToAction("Index");
            }
            return View(obj);

        }


        //Get
        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            //Find() find an element based on primary key
           // var catagoriesFromDb = _db.Catagories.Find(id);

            //these two give similiar output except FirstOrDefault gives first element and if not found doesnt give an error
            //SingleOrDefault: send an error if more than one element found
            var catagoriesFromFirst = _unitwork.Category.GetFirstOrDefault(category => category.Id == id);
            //var catagoreisFromSingle = _db.Catagories.SingleOrDefault(category => category.Id == id);

            if (catagoriesFromFirst == null)
            {
                return NotFound();
            }

            return View(catagoriesFromFirst);
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The Name can't exactly match the Display Order");
            }

            if (ModelState.IsValid)
            {
                _unitwork.Category.Update(obj);
                _unitwork.Save();
                TempData["success"] = "Category Edited Succesfully !";
                return RedirectToAction("Index");
            }
            return View(obj);

        }


        //Get
        public IActionResult Delete(int? id)
        {
            if(id == null || id == 0)
            { 
                return NotFound();
            }   
            var categoriesFromDb = _unitwork.Category.GetFirstOrDefault(category => category.Id == id);
            if (categoriesFromDb == null)
            {
                return NotFound();
            }
            return View(categoriesFromDb);
            
        }

        //Post
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var categoriesFromDb = _unitwork.Category.GetFirstOrDefault(category => category.Id == id);
            if (categoriesFromDb == null || id == 0)
            {
                return NotFound();
            }
            
                _unitwork.Category.Remove(categoriesFromDb);
                _unitwork.Save();
               TempData["success"] = "Category Deleted Succesfully  !";
            return RedirectToAction("Index");

        }
    }

}