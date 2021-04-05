using projem.common.helpers;
using projemm3.BusinessLayer.Abstract;
using projemm3.BusinessLayer.Results;
using projemm3.DataAccessLayer.EntityFramework;
using projemm3.Entities;
using projemm3.Entities.Messages;
using projemm3.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace projemm3.BusinessLayer
{
   public  class projem3UserManager :ManagerBase<ProjemUser>
    {
      

        public BusinesLayerResult<ProjemUser> RegisterUser(RegisterViewModel data)
        {
           ProjemUser user= Find(x => x.Username == data.Username || x.Email==data.EMail);
            BusinesLayerResult<ProjemUser> layer_result= new BusinesLayerResult<ProjemUser>();
            
           
            if (user!=null)
            {
              if(user.Username==data.Username)
                {
                    layer_result.AddError(ErorMessages.UsernameAlreadyExists,"Kullanıcı adı kayıtlı");

                }
                if (user.Email == data.EMail)
                {
                    layer_result.AddError(ErorMessages.EmailAlreadyExists,"E-posta adresi kayıtlı");

                }
            }
            else
            {
               int dbResult=base.Insert(new ProjemUser()
                {
                    Username = data.Username,
                    Email = data.EMail,
                    ProfilImageFileName= "people-profile-icon_24877-40756.jpg",
                    Password = data.Password,
                    ActivateGuid=Guid.NewGuid(),
                  
                   IsActive = true,
                   IsAdmin = false



               });

                if(dbResult>0)
                {
                    layer_result.Result = Find(x => x.Username == data.Username && x.Email == data.EMail);


                    string siteUrl = Confighelpercs.Get<string>("SiteRootUrl");
                    string activateUrl= $"{siteUrl}/Home/UserActivate/{layer_result.Result.ActivateGuid}";
                    string body= $" Merhaba {layer_result.Result.Username} ;<br><br> Hesabınızı aktivite etmek için <a href='{activateUrl}' target='_blank'>tıklayınız</a>";
                    MailHelper.SendMail(body,layer_result.Result.Email,"Benim Notlarım Hesap Aktifleştirme");
                }
            }
            return layer_result;

        }

        public BusinesLayerResult<ProjemUser> GetUserById(int id)
        {
            BusinesLayerResult<ProjemUser> res = new BusinesLayerResult<ProjemUser>();
            res.Result =Find(x => x.Id == id);
            if(res.Result==null)
            {
                res.AddError(ErorMessages.UserNotFound,"Kullanıcı Bulunamadı");
            }
            return res;
        }

        public BusinesLayerResult<ProjemUser> LoginUser(LoginViewModel data)
        {
            BusinesLayerResult<ProjemUser> res = new BusinesLayerResult<ProjemUser>();
            res.Result = Find(x => x.Username == data.Username && x.Password == data.Password);
           
           

            if(res.Result!= null)
            {
                if(!res.Result.IsActive)
                {
                    res.AddError(ErorMessages.UserIsnotActive, "Kullanıcı Aktifleştirilmemiştir");
                    res.AddError(ErorMessages.CheckyourEmail,"Lütfen E-posta Adresinizi Kontrol ediniz");
                }
               

            }
            else
            {
                res.AddError(ErorMessages.UsernameorPasswrong,"Kullanıcı Adı Yada Şifre uyuşmuyor");
            }
            return res;
        }

        public BusinesLayerResult<ProjemUser> ActivateUser(Guid activateid)
        {
            BusinesLayerResult<ProjemUser> res = new BusinesLayerResult<ProjemUser>();
            res.Result = Find(x => x.ActivateGuid == activateid);

            if(res.Result != null)
            {
                if(res.Result.IsActive)
                {
                    res.AddError(ErorMessages.UserAlreadyActive, "Kullanıcı zaten aktif edilmiştir");
                    return res;
                }

                res.Result.IsActive = true;
                Update(res.Result);
            }
            else
            {
                res.AddError(ErorMessages.ActivateIdDoesNotExist, "Aktifleştirilecek Kullanıcı Bulunamadı");
            }
            return res;
        }

        public  BusinesLayerResult<ProjemUser> UpdateProfile(ProjemUser data)
        {
           ProjemUser db_user= Find(x => x.Id != data.Id && (x.Username == data.Username || x.Email == data.Email));
            BusinesLayerResult<ProjemUser> res = new BusinesLayerResult<ProjemUser>();

            if (db_user != null && db_user.Id != data.Id)
            {
                if (db_user.Username == data.Username)
                {
                    res.AddError(ErorMessages.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                }

                if (db_user.Email == data.Email)
                {
                    res.AddError(ErorMessages.EmailAlreadyExists, "E-posta adresi kayıtlı.");
                }

                return res;
            }

            res.Result = Find (x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.Username = data.Username;

            if (string.IsNullOrEmpty(data.ProfilImageFileName) == false)
            {
                res.Result.ProfilImageFileName = data.ProfilImageFileName;
            }

            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErorMessages.ProfileCouldNotUpdated, "Profil güncellenemedi.");
            }

            return res;
        }

        public BusinesLayerResult<ProjemUser> RemoveUserById(int id)
        {
            BusinesLayerResult<ProjemUser> res = new BusinesLayerResult<ProjemUser>();
            ProjemUser user = Find(x => x.Id == id);

            if (user != null)
            {
                if (Delete(user) == 0)
                {
                    res.AddError(ErorMessages.UserCouldNotRemove, "Kullanıcı silinemedi.");
                    return res;
                }
            }
            else
            {
                res.AddError(ErorMessages.UserCouldNotFind, "Kullanıcı bulunamadı.");
            }

            return res;
        }

        public new BusinesLayerResult<ProjemUser> Insert(ProjemUser data) //diğer insert metodunu gizledik
        {
            ProjemUser user = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinesLayerResult<ProjemUser> layer_result = new BusinesLayerResult<ProjemUser>();

            layer_result.Result = data;
            if (user != null)
            {
                if (user.Username == data.Username)
                {
                    layer_result.AddError(ErorMessages.UsernameAlreadyExists, "Kullanıcı adı kayıtlı");

                }
                if (user.Email == data.Email)
                {
                    layer_result.AddError(ErorMessages.EmailAlreadyExists, "E-posta adresi kayıtlı");

                }
            }
            else
            {
                layer_result.Result.ProfilImageFileName = "people-profile-icon_24877-40756.jpg";
                layer_result.Result.ActivateGuid = Guid.NewGuid();

               if(base.Insert(layer_result.Result)==0)
                {
                    layer_result.AddError(ErorMessages.UserCouldNotInserted, "Kullanıcı Eklenemedi" +
                        "");
                }
             
                
            }
            return layer_result;
        }

        public new BusinesLayerResult<ProjemUser> Update(ProjemUser data)
        {
            ProjemUser db_user = Find(x => x.Id != data.Id && (x.Username == data.Username || x.Email == data.Email));
            BusinesLayerResult<ProjemUser> res = new BusinesLayerResult<ProjemUser>();
            res.Result = data;
            if (db_user != null && db_user.Id != data.Id)
            {
                if (db_user.Username == data.Username)
                {
                    res.AddError(ErorMessages.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                }

                if (db_user.Email == data.Email)
                {
                    res.AddError(ErorMessages.EmailAlreadyExists, "E-posta adresi kayıtlı.");
                }

                return res;
            }

            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.Username = data.Username;
             res.Result.IsAdmin = data.IsAdmin;
            res.Result.IsActive = data.IsActive;



            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErorMessages.UserCouldNotUpdated, "Kullanıcı güncellenemedi.");
            }

            return res;

        }
    }
}

