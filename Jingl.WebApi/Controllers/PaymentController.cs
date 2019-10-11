using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jingl.General.Enum;
using Jingl.General.Model.Admin.Transaction;
using Jingl.General.Model.Admin.Transaction.API;
using Jingl.Service.Interface;
using Jingl.Service.Manager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Jingl.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : Controller
    {
        private readonly ITransactionManager ITransactionManager;
        private readonly IMasterManager IMasterManager;
        public PaymentController(IConfiguration config)
        {

            this.IMasterManager = new MasterManager(config);
            this.ITransactionManager = new TransactionManager(config);
          
        }

        [HttpPost]
        [Route("PaymentNotification")]
        public ActionResult PaymentNotification([FromBody] TransactionResultModel body)
        {
            try
            {
                var model = new TransactionResultModel();
                //HelperController.ApiPost<,>
                //var IsValid = HelperController.VerifyPaymentNotificationOrder(body).Result;

                //if (IsValid == true)
                //{
                //    PaymentBookLogModel paymodel = new PaymentBookLogModel();
                //    paymodel.OrderId = body.order_id;
                //    paymodel.SnapToken = body.transaction_id;
                //    paymodel.StatusCode = body.status_code;
                //    paymodel.TransactionStatus = body.transaction_status;
                //    paymodel.CreatedDate = DateTime.Now;
                //    ITransactionManager.CreatePaymentBookLog(paymodel);

                //    if (body.status_code == "200")
                //    {
                //        var getcurrentdata = ITransactionManager.GetDataBookByOrderId(body.order_id);
                //        getcurrentdata.Status = (int)BookingFlow.Paid;
                //        ITransactionManager.UpdateBookData(getcurrentdata);

                //    }
                //}

            }
            catch (Exception ex)
            {

                //HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "PaymentNotification", ex.Message);

            }
            return StatusCode(200);
        }
    }
}