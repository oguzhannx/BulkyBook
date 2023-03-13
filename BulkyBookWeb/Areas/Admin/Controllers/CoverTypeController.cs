using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utilty;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<CoverType> obCoverTypeList = _unitOfWork.CoverType.GetAll();
            return View(obCoverTypeList);
        }

        //GET                                                                                                                                                   
        public IActionResult Create()
        {

            return View();
        }
        //GET
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType obj)
        {
            //if (obj.Name == obj.Name.ToString())
            //{
            //    ModelState.AddModelError("Name", "The Display Order Cannot Exactly Match the Name");
            //}
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "CoverType created successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //UpdateGEt
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();

            }
            //var CoverTypeFromDb= _db.Categories.Find(id);
            var CoverTypeFromDbFirst = _unitOfWork.CoverType.GetFirstOrDefault(c => c.Id == id);
            // var CoverTypeFromDbSingle = _db.Categories.SingleOrDefault(c => c.Id == id);
            if (CoverTypeFromDbFirst == null)
            {
                return NotFound();
            }
            return View(CoverTypeFromDbFirst);
        }
        //UpdatePOST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoverType obj)
        {
            //if (obj.Name == obj.Name.ToString())
            //{
            //    ModelState.AddModelError("Name", "The Display Order Cannot Exactly Match the Name");
            //}
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "CoverType updated successfully";

                return RedirectToAction("Index");
            }
            return View(obj);
        }


        //Delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();

            }
            //var CoverTypeFromDb= _db.Categories.Find(id);
            var CoverTypeFromDbFirst = _unitOfWork.CoverType.GetFirstOrDefault(c => c.Id == id);
            // var CoverTypeFromDbSingle = _db.Categories.SingleOrDefault(c => c.Id == id);
            if (CoverTypeFromDbFirst == null)
            {
                return NotFound();
            }
            return View(CoverTypeFromDbFirst);
        }
        //Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _unitOfWork.CoverType.GetFirstOrDefault(c => c.Id == id);

            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.CoverType.Remove(obj);
            _unitOfWork.Save(); TempData["success"] = "CoverType deleted successflly";

            return RedirectToAction("Index");


        }

    }
}
