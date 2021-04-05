using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using projemm3.BusinessLayer;
using projemm3.Entities;
using projemm3.Models;

namespace projemm3.Controllers
{
    public class CategoryController : Controller
    {

        CategoryManagercs CategoryManagercs = new CategoryManagercs();
        // GET: Category
        public ActionResult Index()
        {
            return View(CategoryManagercs.List()); 

        }

        // GET: Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = CategoryManagercs.Find(x=>x.Id ==id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Category/Create
        public ActionResult Create()
        {
            return View();
        } //ekle

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Category category)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                CategoryManagercs.Insert(category);
                CacheHelper.RemoveCategoriesFromCache();
                return RedirectToAction("Index");
            }

            return View(category);
        }

       
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = CategoryManagercs.Find(x => x.Id == id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        } //düzenle

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Category category)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");
            if (ModelState.IsValid)
            {
                Category cat = CategoryManagercs.Find(x => x.Id == category.Id);
                cat.Title = category.Title;
                cat.Description = category.Description;
                CategoryManagercs.Update(cat);
                CacheHelper.RemoveCategoriesFromCache(); //cache helper metodu ile cache temizledik.
                return RedirectToAction("Index");
            }
            return View(category);
        }

        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = CategoryManagercs.Find(x => x.Id == id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = CategoryManagercs.Find(x => x.Id == id);
            CategoryManagercs.Delete(category);
            CacheHelper.RemoveCategoriesFromCache();
            return RedirectToAction("Index");
        }

    }
}
