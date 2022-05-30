using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.Utility
{
    public class Helper
    {
        public const string Admin = "Admin";
        public static string Patron = "Patron";
     

        public static List<SelectListItem> GetRolesForDropDown()
        {
            /* if (isAdmin)
             {
                 return new List<SelectListItem>
                 {
                     new SelectListItem
                     {
                         Text = Helper.Admin,
                         Value = Helper.Admin
                     }
                 };
             }
             else
             {
                 return new List<SelectListItem>
                 {
                     new SelectListItem{Value=Helper.SuperVisor,Text=Helper.SuperVisor},
                     new SelectListItem{Value=Helper.Regular,Text=Helper.Regular},
                     new SelectListItem{Value=Helper.Manager,Text=Helper.Manager},
                 };
             }*/
            return new List<SelectListItem>
                {
                     
                    new SelectListItem{Value=Helper.Admin,Text=Helper.Admin},
                    new SelectListItem{Value=Helper.Patron,Text=Helper.Patron},
                };
        }
    }

}
