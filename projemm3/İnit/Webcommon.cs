using projem.common;
using projemm3.Entities;
using projemm3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projemm3.İnit
{
    public class Webcommon: Icommon1
    {
        public string GetUsername()
        {
            ProjemUser user = CurrentSessio.User;
            if (user != null)
                return user.Username;
            else
                return "system";
        }
    }
}