using projemm3.BusinessLayer;
using projemm3.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace projemm3.Models
{
    public class CacheHelper
    {
       
        public static List<Category>GetCategoriesFromCache()
        {
            var result = WebCache.Get("category-cache");
            if(result==null)
            {
                CategoryManagercs categoryManager = new CategoryManagercs();
                result = categoryManager.List();
                WebCache.Set("category-cache", categoryManager.List(),20,true);
            }
            return result;
          
        }
        public static void RemoveCategoriesFromCache()
        {
            Remove("category-cache");
        }
        public static void Remove(string key)
        {
            WebCache.Remove(key);
        }
            
    }
}