using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace projemm3.Entities.ValueObjects
{
    public class RegisterViewModel
    {
        [DisplayName("Kullanıcı Adı"), Required(ErrorMessage = "{0} Alanı Boş Geçilemez"),StringLength(25,ErrorMessage ="{0} maksimum {1} karakter olmalı")]
        public string Username { get; set; }
        [DisplayName("E-Posta"), Required(ErrorMessage = "{0} Alanı Boş Geçilemez"), StringLength(70, ErrorMessage = "{0} maksimum {1} karakter olmalı"),EmailAddress(ErrorMessage ="Lütfen {0} alanı için geçerli formatta bir e-posta giriniz ")]
        public string EMail { get; set; }
        [DisplayName("Şifre"), Required(ErrorMessage = "{0} Alanı Boş Geçilemez"), DataType(DataType.Password), StringLength(25, ErrorMessage = "{0} maksimum {1} karakter olmalı")]
        public string Password { get; set; }
        [DisplayName("Şifre Tekrar"), Required(ErrorMessage = "{0} Alanı Boş Geçilemez"), DataType(DataType.Password), StringLength(25, ErrorMessage = "{0} maksimum {1} karakter olmalı"),Compare("Password", ErrorMessage = "{0} ile {1} uyuşmuyor.")]
        public string RePassword { get; set; }

    }
}