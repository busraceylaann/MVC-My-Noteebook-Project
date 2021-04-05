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
using projemm3.DataAccessLayer.EntityFramework;
using projemm3.Entities;

namespace projemm3.Controllers
{
    public class GaleriController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: Galeri
        public ActionResult Index()
        {
            return View(db.Galeris.ToList());
        }
        public ActionResult Slider()
        {

            return View(db.Galeris.ToList());
        }
        // GET: Galeri/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Galeri galeri = db.Galeris.Find(id);
            if (galeri == null)
            {
                return HttpNotFound();
            }
            return View(galeri);
        }

        // GET: Galeri/Create
        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Galeri galeri, HttpPostedFileBase resim)
        {
            if (ModelState.IsValid)
            {
                if (resim != null)
                {
                    WebImage img = new WebImage(resim.InputStream);
                    FileInfo resiminfo = new FileInfo(resim.FileName);
                    string newresim = Guid.NewGuid().ToString() + resiminfo.Extension;
                    img.Save("~/Galeri/" + newresim);
                    galeri.ResimUrl = "~/Galeri/" + newresim;

                }

                db.Galeris.Add(galeri);//galeri tablosuna gelen galeri modelini ekle
                db.SaveChanges();
                return RedirectToAction("/Index");//ekleme işlemi başarılı ise mevcut controllerın ındex sayfasına dön
            }
            return View(galeri);            
        }

        // GET: Galeri/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                ViewBag.Uyari = "Güncelleştirilecek resim bulunamadı";//eğer id si yoksa uyarı vericek
            }
            var resim = db.Galeris.Find(id);
            if (resim == null)
            {
                return HttpNotFound();
            }
            return View(resim);//eğer resim null değilse gelen resim değerini döndürür
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, Galeri galeri, HttpPostedFileBase resim)
        {
            if (ModelState.IsValid)
            {
                var r = db.Galeris.Where(x => x.ResimId == id).SingleOrDefault();//bize gelen resmin id sini bulur
                if (resim != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(r.ResimUrl)))//daha önce yüklenmiş olan bir logo resmi varsa silmeliyiz
                    {
                        System.IO.File.Delete(Server.MapPath(r.ResimUrl));
                    }
                    WebImage img = new WebImage(resim.InputStream);//image nesnesi oluşturduk
                    FileInfo imginfo = new FileInfo(resim.FileName);//gelen image nesnesinin ismini aldık

                    string resimname = Guid.NewGuid().ToString() + imginfo.Extension;
                    img.Resize(200, 100);//logonun boyutları
                    img.Save("~/Galeri/" + resimname);

                    r.ResimUrl = "/Galeri/" + resimname;//r dan gelen url nin neye eşitliyceğimizi yazdık
                }               
                db.SaveChanges();
                return RedirectToAction("/Index");
            }
            return View(galeri);
        }

        // GET: Galeri/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var r = db.Galeris.Find(id);//galeriden seçtiğimiz resmin id sini bul
            if (r == null)
            {
                return HttpNotFound();
            }
            db.Galeris.Remove(r);//bulduğun id yi kaldır
            db.SaveChanges();
            return RedirectToAction("/Index");
        }       
    }
}
