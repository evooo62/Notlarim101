using System;
using Notlarim101.BusinessLayer.Abstract;
using Notlarim101.Common.Helper;
using Notlarim101.DataAccessLayer.EntityFramework;
using Notlarim101.Entity;
using Notlarim101.Entity.Messages;
using Notlarim101.Entity.ValueObject;

namespace Notlarim101.BusinessLayer
{
    public class NotlarimUserManager:ManagerBase<NotlarimUser>
    {
        //Kullanici username kontrolu yapmaliyim
        //kullnici email kontrolu yapmaliyim
        //Kayit islemini gerceklestirmeliyim
        //Activasyon e-postasi gonderimi


        BusinessLayerResult<NotlarimUser> res = new BusinessLayerResult<NotlarimUser>();


        public BusinessLayerResult<NotlarimUser> RegisterUser(RegisterViewModel data)
        {
            NotlarimUser user = Find(s => s.Username == data.Username || s.Email == data.Email);

            

            if (user!=null)
            {
                if (user.Username==data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExist, "Kullanici adi kayitli");
                }

                if (user.Email==data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailalreadyExist, "Email kayitli");
                }
                //throw new Exception("Kayitli kullanici yada e-posta adresi");
            }
            else
            {
                int dbResult = Insert(new NotlarimUser()
                {
                    Name = data.Name,
                    Surname = data.Surname,
                    Username = data.Username,
                    Email = data.Email,
                    Password = data.Password,
                    ProfileImageFileName = "User1.jpeg",
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = false,
                    IsAdmin = false,
                    //repository e tasindi
                    //ModifiedOn = DateTime.Now,
                    //CreatedOn = DateTime.Now,
                    //ModifiedUsername = "system"
                }); 
                if (dbResult>0)
                {
                    res.Result = Find(s => s.Email == data.Email && s.Username == data.Username);
                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activeteUri = $"{siteUri}/Home/UserActivate/{res.Result.ActivateGuid}";
                    string body = $"Merhaba{res.Result.Username};<br><br> Hesabınıza aktifleştirmek için <a href='{activeteUri}' target='_blank'>Tıklayın <a/>";

                    MainHelper.SendMail(body, res.Result.Email, "Notlarim101 hesap aktifleştirildi");
                    //activasyon mail i atilacak
                    //lr.Result.ActivateGuid;
                }
            }

            return res;
        }

        public BusinessLayerResult<NotlarimUser> LoginUser(LoginViewModel data)
        {
            //Giris kontrolu
            //Hesap aktif edilmismi kontrolu
            
            
            res.Result = Find(s => s.Username == data.Username && s.Password == data.Password);
            if (res.Result!=null)
            {
                if (!res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserIsNotActive, "Kullanici adi aktiflestirilmemis!!!");
                    res.AddError(ErrorMessageCode.CheckYourEmail, "Lutfen Mailinizi kontrol edin...");
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.UsernameOrPasswordWrong, "kullanici adi yada sifre uyusmuyor.");
            }

            return res;
        }
        public BusinessLayerResult<NotlarimUser> ActvateUser(Guid id)
        {
            
            if (res.Result!=null)
            {
                if (res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserAlreadyActive, "Bu hesap daha önce aktif edilmiştir!!!");
                        return res;
                }
                res.Result.IsActive = true;
                Update(res.Result);
            }
            else
            {
                res.AddError(ErrorMessageCode.ActivateIdDoesNotExist, "Fuck Off Bitches");
            }
            return res;
        }

        public BusinessLayerResult<NotlarimUser> GetUserById(int id)
        {
            BusinessLayerResult<NotlarimUser> res = new BusinessLayerResult<NotlarimUser>();
            res.Result =Find(s => s.Id == id);
            if (res.Result==null)
            {
                res.AddError(ErrorMessageCode.UserNotFound, "Kullanıcı Bulunamadı");
            }
            return res;
        }

        public BusinessLayerResult<NotlarimUser> UpdateProfile(NotlarimUser data)
        {
            NotlarimUser user = Find(s => s.Id != data.Id && (s.Username == data.Username || s.Email == data.Email));
            
            if (user!=null && user.Id!=data.Id)
            {
                if (user.Username==data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExist,"Bu kullanıcı adı daha önce kaydedilmiştir");
                }
                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailalreadyExist, "Bu email daha önce kaydedilmiştir");
                }
                return res;
            }
            res.Result = Find(s => s.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.Username = data.Username;
            if (!string.IsNullOrEmpty(data.ProfileImageFileName))
            {
                res.Result.ProfileImageFileName = data.ProfileImageFileName;
            }
            if (Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.ProfileCouldNotUpdate, "Profil güncellenmedi.");
            }
            return res;
        }

        public BusinessLayerResult<NotlarimUser> RemoveUserById(int ıd)
        {

            NotlarimUser user = Find(s => s.Id==ıd);
           
            if (user!=null)
            {
                if (Delete(user)==0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotFind, "Kullanıcı silinemedi");
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.UserCouldNotFind, "Kullanıcı bulunamadı");
            }
            return res;
        }
    }
}
