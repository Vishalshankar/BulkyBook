using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;

        public CategoryController(IUnitOfWork unitOfWork, ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _db=db;
        }
       
        public IActionResult Index()
        {
            var category = _unitOfWork.Category.GetAll();
            if(category!=null)
            return View( category);

            return NotFound();
            //return View();
        }

        public IActionResult Upsert(int? id)
        {
          
            Category category = new Category();
            //for create
            if(id==null)
            {
                return View(category);
            }
            //for edit
            category = _unitOfWork.Category.Get(id.GetValueOrDefault());
            if(category==null)
            {
                return NotFound();
            }

            return View(category);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Upsert(Category category)
        {
            if(ModelState.IsValid)
            {
                if (category.Id != 0)
                {
                    _unitOfWork.Category.Update(category);
                }
                else
                {
                    _unitOfWork.Category.Add(category);
                }
            }
            
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }
        //[HttpDelete]

        public IActionResult Delete(int id)
        {
            var delObj = _unitOfWork.Category.Get(id);

            if (delObj == null)
            {
                return NotFound();
            }

            _unitOfWork.Category.Remove(delObj);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));

        }

        #region API CALLS

        [HttpGet]

        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Category.GetAll();
            return Json(new { data = allObj });
        }

        /*[HttpDelete]

        public IActionResult Delete(int id)
        {
            var delObj = _unitOfWork.Category.Get(id);

            if(delObj==null)
            {
                return Json(new { success = false, message = "error while deleting" });
            }

            _unitOfWork.Category.Remove(delObj);
            _unitOfWork.Save();
                      
             return Json(new { success = true, message = "deleted succesfully" });
            
        }*/

        #endregion
    }
}
