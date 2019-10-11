using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Jingl.Models;
using Jingl.Services;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Jingl.Service.Interface;
using CookieManager;
using Jingl.Web.Helper;
using Jingl.Service.Manager;
using Microsoft.Extensions.Configuration;
using Jingl.General.Model.User.ViewModel;
using Jingl.General.Enum;
using Jingl.General.Model.Admin.Master;
using WebPush;
using Jingl.General.Model.User.Notification;
using Newtonsoft.Json;
using Jingl.General.Model.Admin.Transaction;
using Jingl.General.Model.Admin.UserManagement;
using Jingl.General.Utility;

namespace Jingl.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMasterManager IMasterManager;
        private readonly ICookie _cookie;
        private readonly HelperController HelperController;
        private readonly GmailAuthServices GAuth;
        private readonly ITransactionManager ITransactionManager;
        private readonly IUserManagementManager IUserManagementManager;

        public HomeController(IConfiguration config, ICookie cookie)
        {
            this.ITransactionManager = new TransactionManager(config);
            this.GAuth = new GmailAuthServices();
            this.IMasterManager = new MasterManager(config);
            this.HelperController = new HelperController(config, cookie);
            this.IUserManagementManager = new UserManagementManager(config);
        }

        public async Task<IActionResult> Index(string message = "")
        {
            //await HelperController.NotificationEmail("dendidul@gmail.com", "3",EmailTargetType.User,118,"ORD-0002");

            var model = new HomeViewModel();
            try
            {
                UserModel user = new UserModel();
                List<CategoryModel> listCat = new List<CategoryModel>();
                List<TalentVideoModel> listTv = new List<TalentVideoModel>();
                List<TalentCategoryViewModel> listTc = new List<TalentCategoryViewModel>();
                var adVideo = new TalentVideoModel();
                string defUser = "";
                string defPass = "";
                listTv = ITransactionManager.GetTalentVideos(35).ToList();
                listCat = IMasterManager.GetCategoryByType("Talent").ToList();
                listTc = IMasterManager.GetTalentCategoryAllData().ToList();
                adVideo = listTv.Where(x => x.BookCategory == 0).FirstOrDefault();
                if (adVideo != null)
                {
                    ViewBag.Link = adVideo.Link;
                }
                else
                {
                    ViewBag.Link = "";
                }
                var talentModel = new TalentViewModel();
                int userId = Convert.ToInt32(HelperController.GetCookie("UserId"));
                user.Id = userId;
                talentModel = IMasterManager.GetAllTalentCategory();
                user = IUserManagementManager.GetUser(user);

                if (user != null)
                {
                    if (user.UserName == user.DefaultUsername)
                    {
                        defUser = "Username";
                    }
                    if (!string.IsNullOrEmpty(user.Password) && !string.IsNullOrEmpty(user.DefaultPassword))
                    {
                        if (Encryptor.Decrypt(user.Password) == Encryptor.Decrypt(user.DefaultPassword))
                        {
                            defPass = "Password";
                        }
                    }
                    if (!string.IsNullOrEmpty(defPass) || !string.IsNullOrEmpty(defUser))
                    {
                        message = string.Format("Hi {0}.\\nSilahkan ubah {1} {2} mu di Menu Profle/Setting/Edit Profile", user.UserName, defUser, defPass);
                    }
                }
                //talentModel.ListTalentModel = IMasterManager.GetAllTalent().Where(x=>x.UserId != userId).ToList();

                int talentSequence = 0;
                IList<TalentModel> TalentList = new List<TalentModel>();
                //foreach (var i in talentModel.ListTalentModel.Where(x => x.UserId != userId && x.Status == (int)Registration.Completed && x.RoleId == 3 && x.Id != 35).ToList())
                foreach (var i in talentModel.ListTalentModel.Where(x => x.Status == (int)Registration.Completed && x.RoleId == 3 && x.Id != 35).ToList())
                {
                    TalentModel talent = new TalentModel();
                    talent = i;
                    talent.CategoryName = HelperController.CategoryTalentData(i.Id);
                    talent.sequence = talentSequence;
                    TalentList.Add(talent);
                    talentSequence++;
                }

              
                
                talentModel.ListTalentModel = TalentList;

                var listTalentVideo = ITransactionManager.GetAllVideo();
                var listTalentVideoForMostWatch = ITransactionManager.GetAllVideo();

                IList<TalentVideoModel> ListVideo = new List<TalentVideoModel>();
                IList<TalentVideoModel> ListMostWatchVideo = new List<TalentVideoModel>();

                int videoSequence = 1;
                int videoTrending = 1;

                foreach (var i in listTalentVideo.ToList())
                {
                    TalentVideoModel tvm = new TalentVideoModel();
                    tvm = i;
                    tvm.Sequence = videoSequence;
                    ListVideo.Add(tvm);
                    videoSequence++;

                }

                foreach (var j in listTalentVideoForMostWatch.OrderByDescending(x => x.ViewsCount).ToList())
                {
                    TalentVideoModel tvm = new TalentVideoModel();
                    tvm = j;
                    tvm.Sequence = videoTrending;
                    ListMostWatchVideo.Add(tvm);
                    videoTrending++;

                }
                var getBannerData = IMasterManager.GetAllBanner().Where(x => x.BannerCategory == "HomeScr").OrderBy(x => x.Sequence).ToList();
                //talentModel.ListVideo = ListVideo;
                model.ListBanner = getBannerData;
                model.ListTalentModel = TalentList;
                ListVideo = ListVideo.OrderBy(x => x.Sequence).ToList();
                model.ListBestVideo = ListVideo.OrderBy(x => x.Sequence).Take(4).ToList();
                model.ListMostWatchVideo = ListMostWatchVideo.Take(4).ToList();
                model.ListAllVideo = ListVideo;
                model.ListTalentCategoryModel = listTc;
                ViewBag.Categories = listCat.Where(x=>x.Id != 25);
                ViewBag.Message = message;

            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "Index", ex.Message);

                throw ex;
            }

            return View(model);
        }

        public async Task<IActionResult> IndexOld2()
        {
            //await HelperController.NotificationEmail("dendidul@gmail.com", "3",EmailTargetType.User,118,"ORD-0002");

            var model = new HomeViewModel();
            try
            {
                var talentModel = new TalentViewModel();
                int userId = Convert.ToInt32(HelperController.GetCookie("UserId"));
                talentModel = IMasterManager.GetAllTalentCategory();
                //talentModel.ListTalentModel = IMasterManager.GetAllTalent().Where(x=>x.UserId != userId).ToList();

                int talentSequence = 0;
                IList<TalentModel> TalentList = new List<TalentModel>();
                //foreach (var i in talentModel.ListTalentModel.Where(x => x.UserId != userId && x.Status == (int)Registration.Completed && x.RoleId == 3 && x.Id != 35).ToList())
                foreach (var i in talentModel.ListTalentModel.Where(x => x.Status == (int)Registration.Completed && x.RoleId == 3 && x.Id != 35).ToList())
                {
                    TalentModel talent = new TalentModel();
                    talent = i;
                    talent.CategoryName = HelperController.CategoryTalentData(i.Id);
                    talent.sequence = talentSequence;
                    TalentList.Add(talent);
                    talentSequence++;
                }

                talentModel.ListTalentModel = TalentList;

                var listTalentVideo = ITransactionManager.GetAllVideo();
                var listTalentVideoForMostWatch = ITransactionManager.GetAllVideo();

                IList<TalentVideoModel> ListVideo = new List<TalentVideoModel>();
                IList<TalentVideoModel> ListMostWatchVideo = new List<TalentVideoModel>();

                int videoSequence = 1;
                int videoTrending = 1;

                foreach (var i in listTalentVideo.ToList())
                {
                    TalentVideoModel tvm = new TalentVideoModel();
                    tvm = i;
                    tvm.Sequence = videoSequence;
                    ListVideo.Add(tvm);
                    videoSequence++;

                }

                foreach (var j in listTalentVideoForMostWatch.OrderByDescending(x => x.ViewsCount).ToList())
                {
                    TalentVideoModel tvm = new TalentVideoModel();
                    tvm = j;
                    tvm.Sequence = videoTrending;
                    ListMostWatchVideo.Add(tvm);
                    videoTrending++;

                }
                var getBannerData = IMasterManager.GetAllBanner().Where(x => x.BannerCategory == "HomeScr").OrderBy(x => x.Sequence).ToList();
                //talentModel.ListVideo = ListVideo;
                model.ListBanner = getBannerData;
                model.ListTalentModel = TalentList;
                ListVideo = ListVideo.OrderBy(x => x.Sequence).ToList();
                model.ListBestVideo = ListVideo.OrderBy(x => x.Sequence).Take(4).ToList();
                model.ListMostWatchVideo = ListMostWatchVideo.Take(4).ToList();

            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "Index", ex.Message);

                throw ex;
            }

            return View(model);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult welcome()
        {

            var RoleId = HelperController.GetCookie("Role_ID");
            if (RoleId != "")
            {
                if (RoleId == "2" || RoleId == "3")
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("DashBoardDataStudio", "AdmHome");

                }


            }
            else
            {
                return View();
            }



        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult SignIn()
        {
            return View();
        }
        public IActionResult Carapesan()
        {
            return View();
        }
        public IActionResult LoginUser()
        {
            return View();
        }

        public async Task<IActionResult> SignInGmail(string returnUrl)
        {
            try
            {
                var authenticateResult = await HttpContext.AuthenticateAsync("External");
                var authenticateResul1 = await HttpContext.AuthenticateAsync("Application");
                //  var authenticateResult = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
                var principal = authenticateResult.Principal;
                var providerKey = authenticateResult.Principal.Claims.FirstOrDefault();

                if (!authenticateResult.Succeeded)
                    return BadRequest(); // TODO: Handle this better.

                var claimsIdentity = new ClaimsIdentity("Application");

                claimsIdentity.AddClaim(authenticateResult.Principal.FindFirst(ClaimTypes.NameIdentifier));
                claimsIdentity.AddClaim(authenticateResult.Principal.FindFirst(ClaimTypes.Email));

                await HttpContext.SignInAsync(
                    "Application",
                    new ClaimsPrincipal(claimsIdentity));

                return LocalRedirect(returnUrl);
            }
            catch (Exception ex)
            {

                HelperController.InsertLog(0, "SignInGmail", ex.Message);
                return BadRequest();
            }
        }

        public IActionResult SamsungPrize()
        {
            return View();
        }

        public IActionResult TipsVideo()
        {
            return View();
        }

        public IActionResult BenefitLevel()
        {
            return View();
        }
    }
}
