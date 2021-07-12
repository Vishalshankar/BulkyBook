using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
       
        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
          
        }
       
        public IActionResult Index()
        {
            var coverType = _unitOfWork.SP_Call.List<CoverType>(SD.Proc_CoverType_GetAll, null);
            if(coverType!=null)
            return View( coverType);

            return NotFound();
            //return View();
        }

        public IActionResult Upsert(int? id)
        {
          
            CoverType coverType = new CoverType();
            //for create
            if(id==null)
            {
                return View(coverType);
            }
            //for edit
            var parameter = new DynamicParameters();
            parameter.Add("@Id", id);

            coverType = _unitOfWork.SP_Call.OneRecord<CoverType>(SD.Proc_CoverType_Get, parameter);
            if (coverType==null)
            {
                return NotFound();
            }

            return View(coverType);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Upsert(CoverType coverType)
        {
            if(ModelState.IsValid)
            {
                var parameter = new DynamicParameters();
                parameter.Add("@Name",coverType.Name);
                if (coverType.Id != 0)
                {
                    parameter.Add("@Id", coverType.Id);
                    _unitOfWork.SP_Call.Execute<CoverType>(SD.Proc_CoverType_Update, parameter);
                }
                else
                {                    
                    _unitOfWork.SP_Call.Execute<CoverType>(SD.Proc_CoverType_Create, parameter);
                }
            }
            
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }
        //[HttpDelete]

        public IActionResult Delete(int id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@Id", id);

            var delObj = _unitOfWork.SP_Call.OneRecord<CoverType>(SD.Proc_CoverType_Get, parameter);

            if (delObj == null)
            {
                return NotFound();
            }

            _unitOfWork.SP_Call.Execute<CoverType>(SD.Proc_CoverType_Delete, parameter);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));

        }

        #region API CALLS

        [HttpGet]

        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.CoverType.GetAll();
            return Json(new { data = allObj });
        }

        /*[HttpDelete]

        public IActionResult Delete(int id)
        {
            var delObj = _unitOfWork.CoverType.Get(id);

            if(delObj==null)
            {
                return Json(new { success = false, message = "error while deleting" });
            }

            _unitOfWork.CoverType.Remove(delObj);
            _unitOfWork.Save();
                      
             return Json(new { success = true, message = "deleted succesfully" });
            
        }*/

        #endregion
    }
}
