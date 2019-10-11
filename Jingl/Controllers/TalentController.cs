using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookieManager;
using Jingl.General.Model.Admin.Master;
using Jingl.General.Model.Admin.Transaction;
using Jingl.General.Model.Admin.UserManagement;
using Jingl.General.Model.Admin.ViewModel;
using Jingl.General.Model.User.ViewModel;
using Jingl.Service.Interface;
using Jingl.Service.Manager;
using Jingl.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Jingl.Web.Controllers
{
    public class TalentController : Controller
    {
        private readonly IMasterManager IMasterManager;
        private readonly ITransactionManager ITransactionManager;
        private readonly IUserManagementManager IUserManagementManager;
        private readonly ICookie _cookie;
        private readonly HelperController HelperController;
   

        public TalentController(IConfiguration config, ICookie cookie)
        {
            this.IUserManagementManager = new UserManagementManager(config);
            this.IMasterManager = new MasterManager(config);
            this.ITransactionManager = new TransactionManager(config);
            this.HelperController = new HelperController(config, cookie);
        }

        public IActionResult Index()
        {
            return View();
        }
      

        public ActionResult RegisterTalent(string UserId)
        {
            var model = new TalentRegModel();
            try
            {
                var UserData = HelperController.GetUserData();
                model.UserId = Convert.ToInt32(UserId);
                if(UserData != null)
                {
                    model.TalentNm = UserData.Name;
                    model.Email = UserData.Email;
                }
                var categorymodel = IMasterManager.GetCategoryByType("Talent");
                List<TalentCategoryViewModel> ListCategory = new List<TalentCategoryViewModel>();
                foreach (var i in categorymodel)
                {
                    TalentCategoryViewModel data = new TalentCategoryViewModel();
                    data.CategoryId = i.Id;
                    data.CategoryNm = i.CategoryNm;
                    ListCategory.Add(data);
                }
                model.TalentCategory = ListCategory;


            }
            catch (Exception ex)
            {

                HelperController.InsertLog(Convert.ToInt32(UserId), "NoPostRegisterTalent", ex.Message);
                throw ex;
            }

            return View(model);
        }

        
        public ActionResult Video(string FileId,string TalentId)
        {
            var model = new TalentVideoModel();
            try
            {
                int? TempCategoryId = null;
                var VideoList = ITransactionManager.GetAllVideoByCategory(TempCategoryId);
                if(VideoList != null)
                {
                    model = VideoList.Where(x => x.FileId == Convert.ToInt32(FileId)).FirstOrDefault();
                    if(model != null)
                    {
                        model.TalentNm = model.TalentNm;
                        model.ProjectNm = model.ProjectNm;
                    }
                    else
                    {
                        return RedirectToAction("NotFound","NoAccess");
                    }
                    
                }


                //var talentmodel = new TalentModel();
                //talentmodel.Id = Convert.ToInt32(TalentId);
                //var talent = IMasterManager.GetTalent(talentmodel);
                




            }
            catch (Exception ex)
            {

                HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "Video", ex.Message);
                throw ex;
            }
            return View(model);
        }

       

        




        [HttpPost]
        public ActionResult RegisterTalent(TalentRegModel model,string[] TalentCategory)
        {
            try
            {
                //TalentRegModel Tmodel = new TalentRegModel();
                //Tmodel.UserId = model.UserId;
                //Tmode

                model.CreatedBy = model.UserId.ToString();
                foreach (var i in TalentCategory)
                {
                    model.CategoryId = int.Parse(i);
                }
                var data=  ITransactionManager.CreateTalentRegistration(model);
              
                //var gettalentdata = IMasterManager.CreateTalent(model);

                //if (data != null)
                //{
                //    IMasterManager.DeleteTalentCategoryById(data.id);
                //    foreach (var i in TalentCategory)
                //    {
                //        var categoryModel = new TalentCategoryViewModel();
                //        categoryModel.TalentId = data.id;
                //        categoryModel.CategoryId = int.Parse(i);
                //        var temp = IMasterManager.CreateTalentCategory(categoryModel);
                //    }
                //}

                UserModel UserModel = new UserModel();
                UserModel.Id = model.UserId.Value;
                UserModel = IUserManagementManager.GetUser(UserModel);
                var getlastdata = ITransactionManager.GetAllTalentRegistration().LastOrDefault();

                HelperController.EmailApprovalTalent(UserModel.Email,"1",model.TalentNm, getlastdata.RegNum,"").Wait();

                return Json("OK");
            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "RegisterTalent", ex.Message);

                return Json("Error");
               
            }
        }

        public IActionResult UpdateLevel()
        {
            return View();

        }

        public IActionResult BestTalent()
        {
            var data = new List<TalentModel>();
            try
            {
                data = IMasterManager.GetAllTalent().Where(x => x.Status == 3).OrderByDescending(x=>x.CompletedBook).Take(5).ToList();
            }
            catch (Exception ex)
            {

                HelperController.InsertLog(0, "BestTalent", ex.Message);
                throw ex;
            }

            return View(data);
        }

        public IActionResult GetBestTalentData()
        {
            var data = new List<TalentModel>();
            var model = new DashboardViewModel();
            try
            {
                var itemdata = new List<Itemdata>();

                data = IMasterManager.GetAllTalent().Where(x => x.Status == 3).OrderByDescending(x => x.CompletedBook).Take(5).ToList();
                foreach (var i in data)
                {
                    itemdata.Add(new Itemdata { label = i.TalentNm, value = i.CompletedBook });
                }

                model.items = itemdata.ToList();

            }
            catch (Exception ex)
            {

                HelperController.InsertLog(0, "GetBestTalentData", ex.Message);
                throw ex;
            }

            return Json(model);
        }



        public IActionResult OurTalent(string CategoryId)
        {
            var data = new TalentViewModel();
            ViewBag.CategoryId = 0;
            try
            {
                data = IMasterManager.GetAllTalentCategory();
                data.ListTalentViewModel = IMasterManager.GetAllTalentByCategory(Convert.ToInt32(CategoryId)).ToList();
                if (!String.IsNullOrEmpty(CategoryId)) ViewBag.CategoryId = CategoryId;
            }
            catch (Exception ex)
            {

                HelperController.InsertLog(0, "OurTalent", ex.Message);
                throw ex;
            }

            return View(data);
        }

        [HttpPost]
        public IActionResult OurTalentcontent(string CategoryId)
        {
            
            var data = new TalentViewModel();
            try
            {
                data = IMasterManager.GetAllTalentCategory();
                var Ourtalent = IMasterManager.GetAllTalentByCategory(Convert.ToInt32(CategoryId)).OrderByDescending(x => x.IsPriority).ToList();
                data.ListTalentViewModel = Ourtalent;
            }
            catch (Exception ex)
            {

                HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "OurTalentContent", ex.Message);
                throw ex;
            }

            return PartialView("~/Views/Talent/OurTalentcontent.cshtml", data); ;

            //int? temp = null;
            //if (CategoryId != null)
            //{
            //    temp = Convert.ToInt32(CategoryId);
            //}
            //TalentViewModel model = new TalentViewModel();
            //model = IMasterManager.GetAllTalentCategory();
            //var Ourtalent = IMasterManager.GetAllTalentByCategory(temp).ToList();
            //model.ListTalentViewModel = Ourtalent;
            //return PartialView("~/Views/Explore/ExploreContent.cshtml", model);
        }

        public IActionResult TalentDetail(string TalentId)
        {
            var data = new TalentModel();
            var wishlist = new WishlistModel();
            try
            {
                
                data.Id = Convert.ToInt32(TalentId);
                data = IMasterManager.GetTalent(data);
                data.ListTalentVideo = ITransactionManager.GetTalentVideos(data.Id);
                data.TalentBookList = ITransactionManager.GetBookingByTalentId(data.Id);
                data.TalentCategory = IMasterManager.GetTalentCategoryData(data.Id);
                data.CategoryName = HelperController.CategoryTalentData(data.Id);

                ViewBag.Email = "";
                ViewBag.Code = "";
                ViewBag.FirstName = "";
                ViewBag.PhoneNo = "";
                ViewBag.IsVerified = "";

                wishlist.UserId = Convert.ToInt32(HelperController.GetCookie("UserId"));
                wishlist.TalentId = data.Id;

                data.WishlistId = ITransactionManager.GetWishlistIdByUserTalent(wishlist);

                if(Convert.ToInt32(HelperController.GetCookie("UserId")) > 0)
                {
                    UserModel user = new UserModel();
                    user.Id = Convert.ToInt32(HelperController.GetCookie("UserId"));
                    user = IUserManagementManager.GetUser(user);
                    data.Booker = user;
                    ViewBag.Email = user.Email;
                    ViewBag.Code = user.VerificationCode;
                    ViewBag.FirstName = user.FirstName;
                    ViewBag.IsVerified = user.IsVerified;
                    ViewBag.PhoneNo = user.PhoneNumber;
                }

                if (data.ListTalentVideo.Count() > 0)
                {
                    ViewBag.Link = data.ListTalentVideo.Where(x => x.BookCategory == 0).FirstOrDefault().Link;
                }
                else
                {
                    ViewBag.Link = "";
                }
            }
            catch (Exception ex)
            {

                HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "TalentDetail", ex.Message);
                throw ex;
            }
            return View(data);
        }

        public IActionResult TalentDetailOld(string TalentId)
        {
            var data = new TalentModel();
            try
            {
                data.Id = Convert.ToInt32(TalentId);
                data = IMasterManager.GetTalent(data);
                data.ListTalentVideo = ITransactionManager.GetTalentVideos(data.Id);
               
            }
            catch (Exception ex)
            {

                HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "TalentDetail", ex.Message);
                throw ex;
            }
            return View(data);
        }

        public IActionResult TalentVideo(string TalentId)
        {
            IList<TalentVideoModel> VideoList = new List<TalentVideoModel>();
            try
            {

                VideoList = ITransactionManager.GetTalentVideos(Convert.ToInt32(TalentId));
            }
            catch (Exception ex)
            {

                HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "TalentVideo", ex.Message);
                throw ex;
            }

            return View(VideoList);
        }

        public IActionResult TalentWorkspace(string TalentId, int? Status)
        {
            IList<BookModel> data = new List<BookModel>();

            try
            {
                var Id = Convert.ToInt32(TalentId);
                if (Status != null)
                {
                    if (Status > 3 && Status < 5)
                    {
                        data = ITransactionManager.GetBookingByTalentId(Id).Where(book => book.Status < 5 && book.Status > 1).ToList();
                    }
                    else
                    {
                        data = ITransactionManager.GetBookingByTalentId(Id).Where(book => book.Status == Status && book.Status > 1).ToList();
                    }
                }
                else
                {
                    data = ITransactionManager.GetBookingByTalentId(Id);
                }
            }
            catch (Exception ex)
            {

                HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "TalentWorkspace", ex.Message);
                throw ex;
            }
            return View(data);
        }

        public IActionResult Level()
        {
            return View();
        }



    }
}