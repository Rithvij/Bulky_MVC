using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();

            return View(objCompanyList);
        }

        [HttpPost]
        public IActionResult Upsert(Company CompanyObj)
        {


            if (ModelState.IsValid)
            {
                
                
                if (CompanyObj.Id == 0)
                {
                    _unitOfWork.Company.Add(CompanyObj);
                }
                else
                {
                    _unitOfWork.Company.Update(CompanyObj);
                }

                _unitOfWork.Save();
                TempData["success"] = "Company Created Successfully";
                return RedirectToAction("Index");
            }
            else
            {
                
                return View(CompanyObj);
            }
        }

        public IActionResult Upsert(int? id)
        {
            //CompanyVM CompanyVM = new()
            //{
            //    CategoryList = _unitOfWork.Category.GetAll()
            //    .Select(u => new SelectListItem
            //    {
            //        Text = u.Name,
            //        Value = u.Id.ToString()
            //    }),
            //    Company = new Company()
            //};
            if (id == null || id == 0)
            {
                return View(new Company());
            }
            else
            {
                Company companyObj = _unitOfWork.Company.Get(u => u.Id == id);
                return View(companyObj);
            }

        }

        //[HttpPost]
        //public IActionResult Edit(Company obj)
        //{
        //    //if (obj.Name == obj.DisplayOrder.ToString())
        //    //{
        //    //    ModelState.AddModelError("name", "The Display Order cannot exactly match the name.");
        //    //}

        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Company.Update(obj);
        //        _unitOfWork.Save();
        //        TempData["success"] = "Company Updated Successfully";
        //        return RedirectToAction("Index");
        //    }
        //    return View();
        //}

        //public IActionResult Edit(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    Company? CompanyFromDb = _unitOfWork.Company.Get(u => u.Id == id);
        //    //Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u => u.Id == id);
        //    //Category? categoryFromDb2 = _db.Categories.Where(u => u.Id==id).FirstOrDefault();
        //    if (CompanyFromDb == null) return NotFound();

        //    return View(CompanyFromDb);
        //}

        //[HttpPost, ActionName("Delete")]
        //public IActionResult DeletePost(int? id)
        //{
        //    Company? obj = _unitOfWork.Company.Get(u => u.Id == id);
        //    if (obj == null)
        //    {
        //        return NotFound();
        //    }
        //    _unitOfWork.Company.Remove(obj);
        //    _unitOfWork.Save();
        //    TempData["success"] = "Company Deleted Successfully";
        //    return RedirectToAction("Index");

        //}

        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    Company? CompanyFromDb = _unitOfWork.Company.Get(u => u.Id == id);
        //    //Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u => u.Id == id);
        //    //Category? categoryFromDb2 = _db.Categories.Where(u => u.Id==id).FirstOrDefault();
        //    if (CompanyFromDb == null) return NotFound();

        //    return View(CompanyFromDb);
        //}

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();

            return Json(new { data = objCompanyList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var CompanyToBeDeleted = _unitOfWork.Company.Get(u=>u.Id == id);
            if(CompanyToBeDeleted==null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            
            _unitOfWork.Company.Remove(CompanyToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success=true, message="Delete Successful" });
        }
        #endregion

    }
}
