using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookieManager;
using Jingl.General.Model.Admin.Master;
using Jingl.General.Model.Admin.Transaction;
using Jingl.General.Model.User.ViewModel;
using Jingl.Service.Interface;
using Jingl.Service.Manager;
using Jingl.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Jingl.Controllers
{
    public class ActivityController : Controller
    {
        private readonly ITransactionManager ITransactionManager;
        private readonly IMasterManager IMasterManager;

        private readonly ICookie _cookie;
        private readonly HelperController HelperController;

        public ActivityController(IConfiguration config, ICookie cookie)
        {
            this.ITransactionManager = new TransactionManager(config);
            this.IMasterManager = new MasterManager(config);
            this._cookie = cookie;
            this.HelperController = new HelperController(config, cookie);
        }


        public IActionResult Activity()
        {
            var model = new NotificationViewModel();
            var talentMdl = new TalentModel();
            try
            {
                var userId = Convert.ToInt32(HelperController.GetCookie("UserId"));
                talentMdl = IMasterManager.GetTalentProfiles(userId);

                model.ListActiveBook = ITransactionManager.GetBookingByTalentId(talentMdl.Id).Where(book=>book.Status > 1 && book.Status < 5).ToList();
                model.ListFinishBook = ITransactionManager.GetBookingByTalentId(talentMdl.Id).Where(book => book.Status < 0 || book.Status >= 5).ToList();
                model.CountActiveBook = model.ListActiveBook.Count();
                model.CountFinishBook = model.ListFinishBook.Count();

                model.RoleId = Convert.ToInt32(HelperController.GetCookie("Role_ID"));
            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "Activity", ex.Message);

                throw ex;
            }
          
            return View(model);
        }
        public IActionResult Workspace()
        {
            IList<BookModel> data = new List<BookModel>();

            try
            {
                var userId = Convert.ToInt32(HelperController.GetCookie("UserId"));
                data = ITransactionManager.GetBookingByUserId(Convert.ToInt32(userId));
            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "Workspace", ex.Message);

                throw ex;
            }

            return View(data);
        }

       


    }
}