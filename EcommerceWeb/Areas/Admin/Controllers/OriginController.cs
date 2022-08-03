
using DukanHabeshaDataAccess.Repositories.IRepositories;
using DukanHabeshaModels;
using Microsoft.AspNetCore.Mvc;

namespace DukanHabeshaWeb.Areas.Admin.Controllers;
// [Area("Admin")]
public class OriginController : Controller
{
    private readonly IUnitOfWork _unitwork;

    public OriginController(IUnitOfWork unitwork)
    {
        _unitwork = unitwork;
    }


    public IActionResult Index()
    {
        IEnumerable<Origin> objOriginCountry = _unitwork.Origin.GetAll();
        return View(objOriginCountry);
    }


    //Get
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Origin obj)
    {
        if (ModelState.IsValid)
        {
            _unitwork.Origin.Add(obj);
            _unitwork.Save();
            TempData["success"] = "Origin Country Created Successfully!";
            return RedirectToAction("Index");
        }
        return View(obj);
    }

    //Get
    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        var originCountryFromList = _unitwork.Origin.GetFirstOrDefault(u => u.Id == id);
        if (originCountryFromList == null)
        {
            return NotFound();
        }
        return View(originCountryFromList);
    }

    //Post
    [HttpPost]
    [ValidateAntiForgeryToken]

    public IActionResult Edit(Origin obj)
    {
        if (ModelState.IsValid)
        {
            _unitwork.Origin.Update(obj);
            _unitwork.Save();
            TempData["success"] = "Origin Country Updated Successfully!";
            return RedirectToAction("Index");
        }
        return View(obj);
    }

    //Get
    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var deletedObjFromList = _unitwork.Origin.GetFirstOrDefault(u => u.Id == id);

        if (deletedObjFromList == null)
        {
            return NotFound();
        }
        return View(deletedObjFromList);
    }

    //Post
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]

    public IActionResult DeletePost(int? id)
    {
        var deletedObjFromList = _unitwork.Origin.GetFirstOrDefault(u => u.Id == id);
        if (deletedObjFromList == null)
        {
            return NotFound();
        }
        _unitwork.Origin.Remove(deletedObjFromList);
        _unitwork.Save();
        TempData["success"] = "Origin Country Deleted successfully!";
        return RedirectToAction("Index");
    }
}



