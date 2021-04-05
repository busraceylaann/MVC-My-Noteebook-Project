using projemm3.BusinessLayer;
using projemm3.Entities;
using projemm3.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace projemm3.Controllers
{
    public class CommentController : Controller
    {
        private NoteManager noteManager = new NoteManager();
        private CommentManager commentManager = new CommentManager();
        public ActionResult ShowNoteComment(int? id)
        {
            if(id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Note note = noteManager.Find(x => x.Id == id); //databaseden notu cektik
            Note note = noteManager.ListQueryable().Include("Comments").FirstOrDefault(x => x.Id == id);


            if (note==null)
            {
                return HttpNotFound();
            }
            return PartialView("_PartialComments",note.Comments);//bu kısımda notun commentlerini çektik
        }
        [HttpPost]
        public ActionResult Edit(int? id,string text)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = commentManager.Find(x => x.Id == id);

            if(comment ==null)
            {
                return new HttpNotFoundResult();
            }
            comment.Text = text;
            if( commentManager.Update(comment) >0)
            {
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false}, JsonRequestBehavior.AllowGet);
        }

       public ActionResult Delete(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = commentManager.Find(x => x.Id == id);

            if (comment == null)
            {
                return new HttpNotFoundResult();
            }
           
            if (commentManager.Delete(comment) > 0)
            {
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false }, JsonRequestBehavior.AllowGet); 
        }

        [HttpPost]
        public ActionResult Create(Comment comment, int? noteid)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                if (noteid == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                Note note = noteManager.Find(x => x.Id == noteid);

                if (note == null)
                {
                    return new HttpNotFoundResult();
                }

                comment.Note = note;
                comment.Owner = CurrentSessio.User;

                if (commentManager.Insert(comment) > 0)
                {
                    return Json(new { result = true }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }

       
       
    }
    
}