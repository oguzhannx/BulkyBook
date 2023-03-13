using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utilty;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

    public class ProductController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }


        //UpsertGet
        public IActionResult Upsert(int? id)
        {

            ProductVM productVM = new()
            {
                Product = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString(),
                }),
                CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString(),
                })

            };
            if (id == null || id == 0)
            {
                //Creating product


                //ViewBag.CategoryList = CategoryList;
                //ViewData["CoverTypeList"] = CoverTypeList;
                return View(productVM);

            }
            else
            {
                //updating product

                productVM.Product = _unitOfWork.Product.GetFirstOrDefault(i => i.Id == id);
                //productVM.Product.ImageUrl = productVM.Product.ImageUrl + ".png";
                return View(productVM);

            }



        }
        //UpsertPost
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {


            if (ModelState.IsValid)
            {
                string wwwrootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwrootPath, @"images\products");
                    var extension = Path.GetExtension(file.FileName);

                    if (obj.Product.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwrootPath, obj.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }

                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    //Burada Product View Modeli altındaki Product classına ardından onun içindeki ImageURL kısmına atamamızı yaptık.
                    obj.Product.ImageUrl = @"\images\products\" + fileName + extension;

                }


                if (obj.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(obj.Product); //Veri tabanına kayıt
                }
                else
                {
                    _unitOfWork.Product.Update(obj.Product);
                }

                _unitOfWork.Save();
                TempData["success"] = "Product Created successfuly";
                return RedirectToAction("Index");
            }
            return View(obj);
        }




        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType");
            return Json(new { data = productList });

        }

        //Delete
        [HttpDelete]

        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.Product.GetFirstOrDefault(c => c.Id == id);

            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting." });
            }

            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save(); TempData["success"] = "Product deleted successflly";

            return Json(new { success = true, message = "Delete successful." });


        }
        #endregion
    }

}