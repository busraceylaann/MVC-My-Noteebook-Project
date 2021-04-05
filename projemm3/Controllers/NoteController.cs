using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using projemm3.BusinessLayer;
using projemm3.Entities;
using projemm3.Models;

namespace projemm3.Controllers
{
    public class NoteController : Controller
    {

       private NoteManager notemanager = new NoteManager();
        private CategoryManagercs categoryManagercs = new CategoryManagercs();
        private LikedManager likedManager = new LikedManager();
        
        public ActionResult Index()
        {

            var notes = notemanager.ListQueryable().Include("Category").Include("Owner").Where(x => x.Owner.Id == CurrentSessio.User.Id).OrderByDescending(x => x.ModifiedOn);
            return View(notes.ToList());
        }

        public ActionResult MyLikedNotes()
        {
           
            var notes = likedManager.ListQueryable().Include("LikedUser").Include("Note").Where(x => x.LikedUser.Id == CurrentSessio.User.Id).
                Select(x => x.Note).Include("Category").Include("Owner")
                .OrderByDescending(x => x.ModifiedOn);
        

            return View("Index", notes.ToList());
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = notemanager.Find(x=>x.Id==id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }
        
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title");
            return View();
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Note note,HttpPostedFileBase resim)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {

                if (resim != null)
                {
                    WebImage img = new WebImage(resim.InputStream);
                    FileInfo resiminfo = new FileInfo(resim.FileName);
                    string newresim = Guid.NewGuid().ToString() + resiminfo.Extension;
                    img.Resize(320, 150);
                    img.Save("~/Resim/" + newresim);
                    note.Notresim = "~/Resim/" + newresim;

                }
                note.Owner = CurrentSessio.User;
                notemanager.Insert(note);
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title", note.CategoryId);
            return View(note);
        }

        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = notemanager.Find(x => x.Id == id);
            if (note == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title", note.CategoryId);
            return View(note);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Note note)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");
            if (ModelState.IsValid)
            {
                Note db_note= notemanager.Find(x => x.Id == note.Id);
                db_note.IsDraft = note.IsDraft;
                db_note.CategoryId = note.CategoryId;
                db_note.Text = note.Text;
                db_note.Title = note.Title;
                notemanager.Update(db_note);
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title", note.CategoryId);
            return View(note);
        }

        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = notemanager.Find(x => x.Id == id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        // POST: Note/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Note note = notemanager.Find(x => x.Id == id);
            notemanager.Delete(note);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult GetLiked(int[] ids)
        {
            if (CurrentSessio.User != null)
            {
                int userid = CurrentSessio.User.Id;
                List<int> likedNoteIds = new List<int>();
                //likenoteid tipin de liste oluşturdum

                if (ids != null)
                {
                    likedNoteIds = likedManager.List(
                        x => x.LikedUser.Id == userid && ids.Contains(x.Note.Id)).Select(  //belirli bir nesne mevcutmu
                        x => x.Note.Id).ToList();
                }
                else
                {
                    likedNoteIds = likedManager.List(
                        x => x.LikedUser.Id == userid).Select(
                        x => x.Note.Id).ToList();
                }

                return Json(new { result = likedNoteIds });
            }
            else
            {
                return Json(new { result = new List<int>() });
            }
        }
        [HttpPost]
        public ActionResult SetLikeState(int noteid,bool liked)
        {
            int res = 0;

            if (CurrentSessio.User == null)
                return Json(new { hasError = true, errorMessage = "Beğenme işlemi için giriş yapmalısınız.", result = 0 });

            Liked like =
                likedManager.Find(x => x.Note.Id == noteid && x.LikedUser.Id == CurrentSessio.User.Id);

            Note note = notemanager.Find(x => x.Id == noteid);

            if (like != null && liked == false)
            {
                res = likedManager.Delete(like);
            }
            else if (like == null && liked == true)
            {
                res = likedManager.Insert(new Liked()
                {
                    LikedUser = CurrentSessio.User,
                    Note = note
                });
            }

            if (res > 0)
            {
                if (liked)
                {
                    note.LikeCount++;
                }
                else
                {
                    note.LikeCount--;
                }

                res = notemanager.Update(note);

                return Json(new { hasError = false, errorMessage = string.Empty, result = note.LikeCount });
            }

            return Json(new { hasError = true, errorMessage = "Beğenme işlemi gerçekleştirilemedi.", result = note.LikeCount });
        }
        public ActionResult GetNoteText(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = notemanager.Find(x => x.Id == id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return PartialView("_PartialNoteText",note);
        }
    }
}
