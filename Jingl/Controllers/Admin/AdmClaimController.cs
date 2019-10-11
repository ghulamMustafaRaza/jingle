using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CookieManager;
using Jingl.General.Model.Admin.Transaction;
using Jingl.General.Model.Admin.UserManagement;
using Jingl.General.Model.Admin.ViewModel;
using Jingl.Service.Interface;
using Jingl.Service.Manager;
using Jingl.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace Jingl.Web.Controllers.Admin
{
    public class AdmClaimController : AdmMenuController
    {
        private readonly IMasterManager IMasterManager;
        private readonly ITransactionManager ITransactionManager;
        private readonly HelperController HelperController;
        private readonly IUserManagementManager IUserManagementManager;

        public AdmClaimController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this.IMasterManager = new MasterManager(config);
            this.ITransactionManager = new TransactionManager(config);
            this.HelperController = new HelperController(config, cookie);
            this.IUserManagementManager = new UserManagementManager(config);
        }

        public IActionResult Index()
        {
            ClaimFormModel ClaimModel = new ClaimFormModel();
            IList<ClaimModel> model = new List<ClaimModel>();
            var period = IMasterManager.AdmGetAllParameter().Where(x => x.ParamCode == "Period").FirstOrDefault().ParamValue;
            model = ITransactionManager.GetClaimByPeriod(period);
            ClaimModel.ListClaimModel = model;
            ClaimModel.Period = period;
            ViewBag.Period = new SelectList(HelperController.PeriodData, "value", "text", period);
            ViewBag.ListStatus = new SelectList(HelperController.RegistrationStatusList, "value", "text", 1);
            return View(ClaimModel);
        }

        [HttpPost]
        public IActionResult SetStatus(string [] SelectedClaim,ClaimFormModel model)
        {
            foreach (var i in SelectedClaim)
            {
                var getClaimdata = ITransactionManager.GetClaim(int.Parse(i));
                getClaimdata.Status = Convert.ToInt32(model.Status);
                getClaimdata.UpdatedBy = HelperController.GetCookie("UserId");
                if(model.Status == "3")
                {
                    UserModel UserModel = new UserModel();
                    UserModel.Id = getClaimdata.UserId.Value;
                    UserModel = IUserManagementManager.GetUser(UserModel);
                    getClaimdata.PaidDate = DateTime.Now;
                    HelperController.EmailCompletedTransfer(UserModel.Email, "Claim",UserModel.Name, getClaimdata.ClmNumber).Wait();
                }

                ITransactionManager.UpdateClaim(getClaimdata);


            }
            return Json("OK");
        }

       
        public IActionResult SearchData(string Period, string Status)
        {
            ClaimFormModel ClaimModel = new ClaimFormModel();
            IList<ClaimModel> model = new List<ClaimModel>();
            if(Status != "")
            {
                model = ITransactionManager.GetClaimByPeriod(Period).Where(x=>x.Status == Convert.ToInt32(Status)).ToList();
            }
            else
            {
                model = ITransactionManager.GetClaimByPeriod(Period);
            }
           
            ClaimModel.ListClaimModel = model;
            ClaimModel.Period = Period;
            ViewBag.Period = new SelectList(HelperController.PeriodData, "value", "text", Period);
            ViewBag.ListStatus = new SelectList(HelperController.RegistrationStatusList, "value", "text", Status);
            return View("~/Views/AdmClaim/Index.cshtml", ClaimModel);

        }
    }
}