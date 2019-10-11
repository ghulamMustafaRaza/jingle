using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookieManager;
using Jingl.General.Model.Admin.Transaction;
using Jingl.Service.Interface;
using Jingl.Service.Manager;
using Jingl.Web.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace Jingl.Web.Controllers.CRM
{
    public class AdmBookController : AdmMenuController
    {
        // GET: TrxCRM
        private readonly IMasterManager IMasterManager;
        private readonly IUserManagementManager IUserManagementManager;
        private readonly ITransactionManager ITransactionManager;
        private readonly HelperController HelperController;
        private readonly FilesController FilesController;

        public AdmBookController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this.FilesController = new FilesController(config, cookie);
            this.IUserManagementManager = new UserManagementManager(config);
            this.IMasterManager = new MasterManager(config);
            this.ITransactionManager = new TransactionManager(config);
            this.HelperController = new HelperController(config, cookie);
        }


        public ActionResult Index()
        {

            var Admmodel = ITransactionManager.AdmGetAllBook();
            return View(Admmodel);
        }

        // GET: TrxCRM/Details/5
        public IActionResult Details(string id)
        {
            var model = new BookModel();
            try
            {
                model.Id = Convert.ToInt32(id);
                model = ITransactionManager.GetDataBook(model);
                ViewBag.ListPaymentMethod = new SelectList(HelperController.PaymentMethodList, "value", "text", model.PayMethod);
                ViewBag.ListTalent = new SelectList(IMasterManager.GetAllTalent(), "Id", "TalentNm", model.TalentId);
                ViewBag.ListUser = new SelectList(IUserManagementManager.GetAllUser(), "Id", "UserName", model.BookedBy);
                ViewBag.ListTransactionStatus = new SelectList(HelperController.TransactionStatusList, "value", "text", model.Status);
                ViewBag.BookCategory = new SelectList(IMasterManager.GetCategoryByType("Book"), "Id", "CategoryNm", model.BookCategory);
                //   var AdmBookdata = ITransactionManager.AdmGetAllBook().Where(x => x.Id == model.Id).FirstOrDefault();


            }
            catch (Exception)
            {

                //HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("TalentId")), "Details", ex.Message);
                throw;
            }

            return View(model);
        }

        // GET: TrxCRM/Create
        public ActionResult Create()
        {
            ViewBag.ListPaymentMethod = new SelectList(HelperController.PaymentMethodList, "value", "text", 0);
            ViewBag.ListTalent = new SelectList(IMasterManager.GetAllTalent(), "Id", "TalentNm", 0);
            ViewBag.ListTransactionStatus = new SelectList(HelperController.TransactionStatusList, "value", "text", 0);
            ViewBag.ListUser = new SelectList(IUserManagementManager.GetAllUser(), "Id", "UserName", 0);
            ViewBag.BookCategory = new SelectList(IMasterManager.GetCategoryByType("Book"), "Id", "CategoryNm", 0);
            return View();
        }

        // POST: TrxCRM/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(BookModel model, IFormFile Video)
        {
            try
            {
                model.CreatedBy = HelperController.GetCookie("UserId");

                int VideoId = 0;
                if (Video != null)
                {
                    var VideoIds = await FilesController.UploadPhotosFile(Video);
                    VideoId = VideoIds.Id;
                }

                if (VideoId != 0)
                {
                    model.FileId = VideoId;
                }

                model.UpdatedBy = HelperController.GetCookie("UserId");
                ITransactionManager.CreateBookData(model);
                // TODO: Add insert logic here
                return Json("OK");
            }
            catch
            {
                return Json("False");
            }
        }

        // GET: TrxCRM/Edit/5
        public ActionResult Edit(int id)
        {
            var model = new BookModel();
            try
            {
                model.Id = Convert.ToInt32(id);
                model = ITransactionManager.GetDataBook(model);
                ViewBag.ListPaymentMethod = new SelectList(HelperController.PaymentMethodList, "value", "text", model.PayMethod);
                ViewBag.ListTalent = new SelectList(IMasterManager.GetAllTalent(), "Id", "TalentNm", model.TalentId);
                ViewBag.BookCategory = new SelectList(IMasterManager.GetCategoryByType("Book"), "Id", "CategoryNm", model.BookCategory);
                ViewBag.ListUser = new SelectList(IUserManagementManager.GetAllUser(), "Id", "UserName", model.BookedBy);
                ViewBag.ListTransactionStatus = new SelectList(HelperController.TransactionStatusList, "value", "text", model.Status);
                // var AdmBookdata = ITransactionManager.AdmGetAllBook().Where(x => x.Id == model.Id).FirstOrDefault();


            }
            catch (Exception)
            {

                //HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("TalentId")), "Details", ex.Message);
                throw;
            }

            return View(model);
        }

        // POST: TrxCRM/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BookModel model, IFormFile FileVideo,string TotalPay)
        {
            try
            {
                TotalPay = TotalPay.Replace(",", "").Replace(".", "");
                var Amount = Convert.ToInt32(TotalPay);
                int VideoId = 0;
                if (FileVideo != null)
                {
                    var VideoIds = await FilesController.UploadVideoFilesData(FileVideo);
                    VideoId = VideoIds.Id;
                }

                if (VideoId != 0)
                {
                    model.FileId = VideoId;
                }
                model.TotalPay = Amount;
                model.UpdatedBy = HelperController.GetCookie("UserId");
                ITransactionManager.AdmUpdateBookData(model);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }

        }

        // GET: TrxCRM/Delete/5
        public IActionResult Delete(int id)
        {

            BookModel model = new BookModel();
            model.Id = id;
            model = ITransactionManager.GetDataBook(model);
            ViewBag.ListPaymentMethod = new SelectList(HelperController.PaymentMethodList, "value", "text", model.PayMethod);
            ViewBag.ListTalent = new SelectList(IMasterManager.GetAllTalent(), "Id", "TalentNm", model.TalentId);
            ViewBag.BookCategory = new SelectList(IMasterManager.GetCategoryByType("Book"), "Id", "CategoryNm", model.BookCategory);
            ViewBag.ListUser = new SelectList(IUserManagementManager.GetAllUser(), "Id", "UserName", model.BookedBy);
            ViewBag.ListTransactionStatus = new SelectList(HelperController.TransactionStatusList, "value", "text", model.Status);

            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(BookModel model)
        {
            ITransactionManager.DeleteBook(model.Id);

            return RedirectToAction("Index");
        }
    }
}