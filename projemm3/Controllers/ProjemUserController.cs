using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using projemm3.BusinessLayer;
using projemm3.BusinessLayer.Results;
using projemm3.Entities;


namespace projemm3.Controllers
{
    public class ProjemUserController : Controller
    {
        private projem3UserManager Projem3UserManager = new projem3UserManager();

       
        public ActionResult Index()
        {
            return View(Projem3UserManager.List());
        }

        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjemUser projemUser = Projem3UserManager.Find(x => x.Id == id.Value);
            if (projemUser == null)
            {
                return HttpNotFound();
            }
            return View(projemUser);
        }
        
        public ActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( ProjemUser projemUser)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");
            if (ModelState.IsValid)
            {  BusinesLayerResult<ProjemUser> res= Projem3UserManager.Insert(projemUser);
                if(res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                }

                return RedirectToAction("Index");
            }

            return View(projemUser);
        }

        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjemUser projemUser = Projem3UserManager.Find((x => x.Id == id.Value));
            if (projemUser == null)
            {
                return HttpNotFound();
            }
            return View(projemUser);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( ProjemUser projemUser)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");
            if (ModelState.IsValid)
            {
                BusinesLayerResult<ProjemUser> res = Projem3UserManager.Update(projemUser);
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                }
                return RedirectToAction("Index");
            }
            return View(projemUser);
        }

        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjemUser projemUser = Projem3UserManager.Find((x => x.Id == id.Value));
            if (projemUser == null)
            {
                return HttpNotFound();
            }
            return View(projemUser);
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProjemUser projemUser = Projem3UserManager.Find((x => x.Id == id));
            Projem3UserManager.Delete(projemUser);
            return RedirectToAction("Index");
        }

        
    }
}
