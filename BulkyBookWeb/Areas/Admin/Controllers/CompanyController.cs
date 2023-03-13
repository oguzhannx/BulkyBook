using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utilty;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }


        //UpsertGet
        public IActionResult Upsert(int? id)
        {

            Company company = new();

            if (id == null || id == 0)
            {
                //Creating Company


                //ViewBag.CategoryList = CategoryList;
                //ViewData["CoverTypeList"] = CoverTypeList;
                return View(company);

            }
            else
            {
                //updating Company

                company = _unitOfWork.Company.GetFirstOrDefault(i => i.Id == id);
                //CompanyVM.Company.ImageUrl = CompanyVM.Company.ImageUrl + ".png";
                return View(company);

            }



        }
        //UpsertPost
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company obj)
        {


            if (ModelState.IsValid)
            {




                if (obj.Id == 0)
                {
                    _unitOfWork.Company.Add(obj); //Veri tabanına kayıt
                    TempData["success"] = "Company Created successfuly";

                }
                else
                {
                    _unitOfWork.Company.Update(obj);
                    TempData["success"] = "Company Updated successfuly";

                }

                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(obj);
        }




        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var CompanyList = _unitOfWork.Company.GetAll();
            return Json(new { data = CompanyList });
        }

        //Delete
        [HttpDelete]

        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.Company.GetFirstOrDefault(c => c.Id == id);

            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting." });
            }



            _unitOfWork.Company.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Company deleted successflly";

            return Json(new { success = true, message = "Delete successful." });


        }
        #endregion
    }

}