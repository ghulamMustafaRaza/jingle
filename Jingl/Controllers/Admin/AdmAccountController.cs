using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookieManager;
using Jingl.General.Model.Admin.UserManagement;
using Jingl.Service.Interface;
using Jingl.Service.Manager;
using Jingl.Web.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Jingl.General.Utility;
using Jingl.General.Model.Admin.Master;
using Jingl.General.Model.Admin.ViewModel;

namespace Jingl.Web.Controllers.CRM
{
    public class AdmAccountController : Controller
    {
     
        private readonly IUserManagementManager IUserManagementManager;
        private readonly IMasterManager IMasterManager;
        private readonly ICookie _cookie;
        private readonly HelperController HelperController;

        public AdmAccountController(IConfiguration _config,ICookie cookie)
        {
            this.IMasterManager = new MasterManager(_config);
            this.IUserManagementManager = new UserManagementManager(_config);
            this._cookie = cookie;
            this.HelperController = new HelperController(_config, cookie);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(UserModel model)
        {

            var Password = Encryptor.Encrypt(model.Password);
            model.Password = Password;
            var checkdata = IUserManagementManager.CheckValidUser(model);

            if(checkdata != null )
            {
                //CookieOptions options = new CookieOptions();
                //options.Expires = DateTime.Now.AddDays(1);
                //Response.Cookies.Append("Role_ID", checkdata != null ? checkdata.RoleId.ToString():"", options);
                //Response.Cookies.Append("UserName", model.UserName, options);

                string FirstName = checkdata.FirstName != null ? checkdata.FirstName : "";
                string LastName = checkdata.LastName != null ? checkdata.LastName : "";
                string UserName = FirstName + " " + LastName;
                HelperController.SetCookie("UserId", checkdata != null ? checkdata.Id.ToString() : "");
                HelperController.SetCookie("UserName", UserName);
                HelperController.SetCookie("Role_ID", checkdata != null ? checkdata.RoleId.ToString() : "");

                return Json(new { User = "Valid",RoleId = checkdata.RoleId });
            }
            else
            {
                return Json(new { User = "NotValid", RoleId = checkdata != null ? checkdata.RoleId.ToString() : "" });
            }

           // return RedirectToAction("Dashboard", "CRMHome");



        }

        public IActionResult LogOut()
        {
            return View();
        }

    }
}