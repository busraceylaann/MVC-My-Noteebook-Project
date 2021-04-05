using projemm3.BusinessLayer;
using projemm3.BusinessLayer.Results;
using projemm3.Entities;
using projemm3.Entities.Messages;
using projemm3.Entities.ValueObjects;
using projemm3.Models;
using projemm3.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace projemm3.Controllers
{
    public class HomeController : Controller
    {
        private NoteManager nm = new NoteManager();
        private CategoryManagercs cm = new CategoryManagercs();
        private GaleriManager gm = new GaleriManager();
        public ActionResult Index()
        {//Category contrellar uzerinden gelen view talebi
            //if(TempData["mm"]!=null)
            //{
            //    return View(TempData["mm"] as List<Note>); //temp data içindeki notları al kullan.tempdata homecontoller arası geçiş
            //}
           

            return View(nm.ListQueryable().Where(x=>x.IsDraft==false).OrderByDescending(x => x.ModifiedOn).ToList());

            //return View(nm.GetAllNoteQueryable().OrderByDescending(x => x.ModifiedOn).ToList()); 
        }

       
        public ActionResult ByCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
            Category cat = cm.Find(x => x.Id == id.Value);

            if (cat == null)
            {
                return HttpNotFound();
                //return RedirectToAction("Index","Home"); buda olur
            }
            return View("Index", cat.Notes.Where(x => x.IsDraft == false).OrderByDescending(x => x.ModifiedOn).ToList());
        }

        public ActionResult MostLiked()
        {
            NoteManager nm = new NoteManager();
            return View("Index", nm.ListQueryable().OrderByDescending(x => x.LikeCount).ToList());
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult ShowProfile()
        {

           
            projem3UserManager eum = new projem3UserManager();
            BusinesLayerResult<ProjemUser> res = eum.GetUserById(CurrentSessio.User.Id);

            if(res.Errors.Count>0)
            {
                ErorViewModel errornotifyobj = new ErorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };
                return View("Error", errornotifyobj);
            }

            return View(res.Result);
        }
        
        public ActionResult EditProfile()
        {
            
            projem3UserManager eum = new projem3UserManager();
            BusinesLayerResult<ProjemUser> res = eum.GetUserById(CurrentSessio.User.Id);

            if (res.Errors.Count > 0)
            {
                ErorViewModel errornotifyobj = new ErorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };
                return View("Error", errornotifyobj);
            }

            return View(res.Result);
        }

        [HttpPost]
        public ActionResult EditProfile(ProjemUser model, HttpPostedFileBase ProfileImage)
        {
            ModelState.Remove("ModifiedUsername");
            if (ModelState.IsValid)

            {
                if (ProfileImage != null &&
                   (ProfileImage.ContentType == "image/jpeg" ||
                   ProfileImage.ContentType == "image/jpg" ||
                   ProfileImage.ContentType == "image/png"))
                {
                    string filename = $"user_{model.Id}.{ProfileImage.ContentType.Split('/')[1]}";

                    ProfileImage.SaveAs(Server.MapPath($"~/images/{filename}"));
                    model.ProfilImageFileName = filename;
                }
                projem3UserManager eum = new projem3UserManager();
                BusinesLayerResult<ProjemUser> res = eum.UpdateProfile(model);
                if (res.Errors.Count > 0)
                {
                    ErorViewModel errorNotifyObj = new ErorViewModel()
                    {
                        Items = res.Errors,
                        Title = "Profil Güncellenemedi.",
                        RedirectingUrl = "/Home/EditProfile"
                    };
                    return View("Error", errorNotifyObj);
                }

               CurrentSessio.Set<ProjemUser>("login", res.Result) ; // Profil güncellendiği için session güncellendi.
                return RedirectToAction("ShowProfile");
            }
            return View(model);
               
            
        }
        public ActionResult TestNotify()
        {
            ErorViewModel model = new ErorViewModel()
            {
                Header = "Yönlendirme...",
                Title = "Ok Test",
                RedirectingTimeout = 3000,
                Items=new List<ErorMessageobj>() { new ErorMessageobj() { Message= "Test başarılı 1" },
                    new ErorMessageobj() { Message= "Test başarılı 2" } }
            };
            return View("Error",model);
        }

        public ActionResult DeleteProfile()
        {
           
            projem3UserManager eum = new projem3UserManager();
            BusinesLayerResult<ProjemUser> res =eum.RemoveUserById(CurrentSessio.User.Id);

            if (res.Errors.Count > 0)
            {
                ErorViewModel errorNotifyObj = new ErorViewModel()
                {
                    Items = res.Errors,
                    Title = "Profil Silinemedi.",
                    RedirectingUrl = "/Home/ShowProfile"
                };

                return View("Error", errorNotifyObj);
            }

            Session.Clear();

            return RedirectToAction("Index");
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                projem3UserManager pum = new projem3UserManager();
                BusinesLayerResult<ProjemUser> res = pum.LoginUser(model);

                if (res.Errors.Count > 0)
                {
                    if (res.Errors.Find(x => x.Code == ErorMessages.UserIsnotActive) != null)
                    {
                        ViewBag.SetLink = "http://Home/Activate/1234-4567-78980";
                    }
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));

                   
                    

                    return View(model);
                }
                CurrentSessio.Set<ProjemUser>("login", res.Result);
                return RedirectToAction("Index");
            }
           
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                
                projem3UserManager pum = new projem3UserManager();
               BusinesLayerResult<ProjemUser> res= pum.RegisterUser(model);

               if(res.Errors.Count>0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }


               
                OkViewModel notifyobj = new OkViewModel()
                {
                    Title = "Kayıt Başarılı",
                    RedirectingUrl="/Home/Login",
                };
                notifyobj.Items.Add("Lütfen e-posta adresinize gönderdiğimiz aktivasyon linkine tıklayarak hesabınızı aktivite ediniz.Hesabınızı aktivite etmeden not ekleyemez ve beğenme yapamazsınız.");
                return View("Ok",notifyobj);
            }
            return View(model);
        }
       
        public ActionResult UserActivate(Guid id)
        { 
            projem3UserManager pum = new projem3UserManager();
            BusinesLayerResult<ProjemUser> res = pum.ActivateUser(id);
            if (res.Errors.Count > 0)
            {
                ErorViewModel errornotifyobj = new ErorViewModel()
                {
                    Title = "Geçersiz İşlem",
                    Items = res.Errors
                };
                return View("Error",errornotifyobj);
            }
            OkViewModel oktifyobj = new OkViewModel()
            {
                Title="Hesap Aktifleştirildi",
                RedirectingUrl="/Home/Login",
               
            };
            oktifyobj.Items.Add("Hesabınız aktifleştirildi.Artık not paylaşabilir ve beğenme yapabilirsiniz.");
            return View("Ok",oktifyobj);
        }





    }
}