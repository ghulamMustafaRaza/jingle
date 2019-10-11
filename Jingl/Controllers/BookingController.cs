using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookieManager;
using Jingl.General.Enum;
using Jingl.General.Model.Admin.Master;
using Jingl.General.Model.Admin.Transaction;
using Jingl.General.Model.Admin.Transaction.API;
using Jingl.General.Model.Admin.UserManagement;
using Jingl.Service.Interface;
using Jingl.Service.Manager;
using Jingl.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace Jingl.Web.Controllers
{
    public class BookingController : Controller
    {
        private readonly IUserManagementManager IUserManagementManager;
        private readonly ITransactionManager ITransactionManager;
        private readonly IMasterManager IMasterManager;
        private readonly ICookie _cookie;
        private readonly HelperController HelperController;


        public BookingController(IConfiguration config, ICookie cookie)
        {
            this.IUserManagementManager = new UserManagementManager(config);
            this.IMasterManager = new MasterManager(config);
            this.ITransactionManager = new TransactionManager(config);
            this.HelperController = new HelperController(config, cookie);
        }
        public IActionResult Hire(string TalentId)
        {

            try
            {
                var checkLogin = HelperController.HasLogin();
                //ViewBag.BookCategory = new SelectList(IMasterManager.GetCategoryByType("Book"), "Id", "CategoryNm", 0);
                ViewBag.BookCategory = IMasterManager.GetCategoryByType("Book");
                if (checkLogin == false)
                {

                    return RedirectToAction("Onboarding", "Account");
                }
                else
                {
                    var model = new BookModel();
                    var talentModel = new TalentModel();
                    var userModel = new UserModel();
                    talentModel.Id = Convert.ToInt32(TalentId);
                    userModel.Id = Convert.ToInt32(HelperController.GetCookie("UserId"));
                    var checkTalentData = IMasterManager.GetTalent(talentModel);
                    var getUserData = IUserManagementManager.GetUser(userModel);
                    model.TalentId = checkTalentData.Id;
                    model.TalentNm = checkTalentData.TalentNm;
                    model.TalentPhotos = checkTalentData.LinkImg;
                    model.From = getUserData.FirstName;
                    return View(model);
                }


            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "Hire", ex.Message);

                throw ex;
            }



        }

        [HttpPost]
        public IActionResult Hire(BookModel model)
        {
            try
            {
                if(string.IsNullOrEmpty(model.ProjectNm))
                {
                    CategoryModel cat = new CategoryModel();
                    cat.Id = Convert.ToInt32(model.BookCategory);
                    cat = IMasterManager.GetDataCategory(cat);
                    model.ProjectNm = cat.CategoryNm;
                }
                model.CreatedBy = HelperController.GetCookie("UserId");
                model.BookedBy = Convert.ToInt32(HelperController.GetCookie("UserId"));
                var periodData= IMasterManager.AdmGetAllParameter().Where(x => x.ParamCode == "Period" && x.ParamName == "Period").FirstOrDefault();
                model.Period = periodData != null ? periodData.ParamValue : "";
               var getCurrentData = ITransactionManager.CreateBookData(model);
                return Json(new { BookId = getCurrentData.Id, Status = "OK" });


            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "Hire", ex.Message);

                return Json(new { BookId = 0, Status = "Error" });
                throw ex;
            }



        }

        [HttpPost]
        public ActionResult UlangiPembayaran(BookModel model)
        {
            try
            {
                var getcurrentdata = ITransactionManager.GetDataBook(model);
                getcurrentdata.Status = (int)BookingFlow.Submit;
                getcurrentdata.UpdatedBy = HelperController.GetCookie("UserId");
                ITransactionManager.UpdateBookData(getcurrentdata);
                UserModel userModel = new UserModel();
                userModel.Id = Convert.ToInt32(HelperController.GetCookie("UserId"));
                var getUser = IUserManagementManager.GetUser(userModel);

                HelperController.NotificationEmail(getcurrentdata.Email, "100", EmailTargetType.User, getcurrentdata.Id, getcurrentdata.OrderNo, userModel.FirstName).Wait();

                return Json(new { BookId = getcurrentdata.Id, Status = "OK" });

            }
            catch (Exception ex)
            {

                HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "UlangiPembayaran", ex.Message);
                return Json(new { BookId = 0, Status = "Error" });
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetMidTransToken(BookModel model)
        {
            try
            {

                transaction_details transdetails = new transaction_details();
                transdetails.order_id = model.OrderNo;
                transdetails.gross_amount = model.TotalPay.Value;
                TransactionOrder Order = new TransactionOrder()
                {
                    transaction_details = transdetails
                }
                    ;
                var data = await HelperController.SnapApiPost<TokenResultModel, TransactionOrder>("snap/v1/transactions", Order);
                //ITransactionManager.CreateWishlistData(model);

                var getcurrentdata = ITransactionManager.GetDataBookByOrderId(model.OrderNo);
                getcurrentdata.PriceAmount = model.PriceAmount;
                getcurrentdata.Potongan = model.Potongan;
                getcurrentdata.TotalPay = model.TotalPay;
                getcurrentdata.VoucherCode = model.VoucherCode;
                ITransactionManager.UpdateBookData(getcurrentdata);

                return Json(data);
            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "GetMidTransToken", ex.Message);

                return Json("Error");
                throw ex;
            }
        }


        public IActionResult CheckBookingData(string bookId,string notificationId, int? IsEmail)
        {
            try
            {
                var bookModel = new BookModel();
                try
                {
                    ITransactionManager.IsReadedNotification(Convert.ToInt32(notificationId));

                    bookModel.Id = Convert.ToInt32(bookId);
                    bookModel = ITransactionManager.GetDataBook(bookModel);
                }
                catch (Exception)
                {

                    throw;
                }

                if (bookModel != null)
                {
                    var userModel = new UserModel();
                    userModel = HelperController.GetUserData();
                    if (bookModel.BookedBy == userModel.Id)
                    {
                        if (bookModel.Status > 1)
                        {
                            return RedirectToAction("BookingHistory", new { bookId = bookId, IsEmail = IsEmail });
                        }
                        else
                        {
                            return RedirectToAction("OrderDetail", new { bookId = bookId, IsEmail = IsEmail });
                        }
                    }
                    else
                    {
                        return RedirectToAction("NotAuthorized", "NoAccess");
                    }
                }
                else
                {
                    return RedirectToAction("ErrorPage", "NoAccess");
                }
            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "CheckBookingData", ex.Message);
                return Json("Error");
                throw ex;
            }
        }


        public async Task<IActionResult> OrderDetail(string BookId,int? IsEmail)
        {
            var model = new BookModel();

            try
            {
                var transactionModel = new TransactionResultModel();
                var data = new BookModel();
                data.Id = Convert.ToInt32(BookId);
                model.Id = data.Id;
               
                data = ITransactionManager.GetDataBook(data);
                //transactionModel = await HelperController.GetTransactionStatus(data.SnapToken);
                model = ITransactionManager.GetBookConfirmation(model);
                model.VaNumber = model.SnapToken;
                if(model.PayMethod == "bank_transfer")
                {
                    if (model.PaymentChannel == "802")
                    {
                        model.BankName = "Mandiri";
                    }
                    else if (model.PaymentChannel == "801")
                    {
                        model.BankName = "BNI";
                    }
                    else if (model.PaymentChannel == "800")
                    {
                        model.BankName = "BRI";
                    }
                    else if (model.PaymentChannel == "702")
                    {
                        model.BankName = "BCA";
                    }
                }
                

                //if (transactionModel.transaction_id != null)
                //{
                //    if(transactionModel.va_numbers != null)
                //    {
                //        model.VaNumber = transactionModel.va_numbers.FirstOrDefault() != null ? transactionModel.va_numbers.FirstOrDefault().va_number : "";
                //        model.BankName = transactionModel.va_numbers.FirstOrDefault() != null ? transactionModel.va_numbers.FirstOrDefault().bank : "";
                //    }
                //    else
                //    {
                //        model.VaNumber = "";
                //        model.BankName = "";
                //    }

                //}
                model.ClientKeyId = HelperController.MidtransClientKey();
                model.IsEmail = IsEmail;
                //model.TransactionStatus = 
            }
            catch (Exception ex)
            {
                HelperController.InsertLog(0, "OrderDetail", ex.Message);
                return Json("Error");
                throw ex;
            }
            return View(model);
        }


        public IActionResult DataConfirm(int BookId)
        {
            var model = new BookModel();
            model.Id = BookId;
            model.ClientKeyId = HelperController.MidtransClientKey();
            model = ITransactionManager.GetBookConfirmation(model);

            return View(model);
        }

        public IActionResult PayConfirm(int BookId, string PaymentMethodId)
        {
            var model = new BookModel();
            model.Id = BookId;
            model = ITransactionManager.GetBookConfirmation(model);
            model.PayMethod = PaymentMethodId;
            return View(model);
        }


        [HttpPost]

        public IActionResult CheckPaidBook(string BookId)
        {

            try
            {
                var Model = new BookModel();
                Model.Id = Convert.ToInt32(BookId);
                var getdata = ITransactionManager.GetDataBook(Model);

                if (getdata != null)
                {
                    if (getdata.Status > 1)
                    {
                        return Json(true);
                    }
                    else
                    {
                        return Json(false);
                    }
                }
                else
                {
                    return Json(false);
                }
            }
            catch (Exception ex)
            {
                HelperController.InsertLog(0, "CheckPaidBook", ex.Message);
                return Json(false);
            }
        }

        [HttpPost]
        public IActionResult UpdateMaterialSent(string BookId, string status, string FileId)
        {
            try
            {
                var model = new BookModel();
                model.Id = Convert.ToInt32(BookId);
                var getcurrentdata = ITransactionManager.GetDataBook(model);
                getcurrentdata.UpdatedBy = HelperController.GetCookie("UserId");
                var sendTo = "";
                var body = "";
                var subject = "";
                var firstName = "";

                var Userdata = new UserModel();
                var Talentdata = new TalentModel();
                var TalentUserdata = new UserModel();

                Userdata.Id = Convert.ToInt32(getcurrentdata.BookedBy);
                Talentdata.Id = getcurrentdata.TalentId.Value;
                Talentdata = IMasterManager.GetTalent(Talentdata);

                TalentUserdata.Id = Talentdata.UserId.Value;
                TalentUserdata = IUserManagementManager.GetUser(TalentUserdata);

                Userdata = IUserManagementManager.GetUser(Userdata);
                firstName = Userdata.FirstName;
                //getcurrentdata.TotalPay = model.TotalPay;
                //getcurrentdata.PriceAmount = model.PriceAmount;
                //getcurrentdata.Potongan = model.Potongan;

                int Statusdata = Convert.ToInt32(status);

                if (Statusdata == 2)
                {
                    getcurrentdata.Status = (int)BookingFlow.Paid;
                    sendTo = Userdata.Email;
                    body = getcurrentdata.OrderNo + ' ' + BookingFlow.Paid.ToString();
                    subject = getcurrentdata.OrderNo + ' ' + BookingFlow.Paid.ToString();
                }
                else if (Statusdata == 3)
                {
                    getcurrentdata.Status = (int)BookingFlow.ProjectAccepted;
                    sendTo = Userdata.Email;
                    body = getcurrentdata.OrderNo + ' ' + BookingFlow.ProjectAccepted.ToString();
                    subject = getcurrentdata.OrderNo + ' ' + BookingFlow.ProjectAccepted.ToString();
                }
                else if (Statusdata == 4)
                {
                    getcurrentdata.Status = (int)BookingFlow.RecordingProcess;
                    sendTo = Userdata.Email;
                    body = getcurrentdata.OrderNo + ' ' + BookingFlow.RecordingProcess.ToString();
                    subject = getcurrentdata.OrderNo + ' ' + BookingFlow.RecordingProcess.ToString();
                }
                else if (Statusdata == 5)
                {
                    getcurrentdata.Status = (int)BookingFlow.MaterialAccepted;
                    sendTo = Userdata.Email;
                    body = getcurrentdata.OrderNo + ' ' + BookingFlow.MaterialAccepted.ToString();
                    subject = getcurrentdata.OrderNo + ' ' + BookingFlow.MaterialAccepted.ToString();
                }
                else if (Statusdata == 6)
                {
                    getcurrentdata.Status = (int)BookingFlow.ProjectCompleted;
                    sendTo = Userdata.Email;
                    body = getcurrentdata.OrderNo + ' ' + BookingFlow.ProjectCompleted.ToString();
                    subject = getcurrentdata.OrderNo + ' ' + BookingFlow.ProjectCompleted.ToString();
                }
                else if (Statusdata == 7)
                {
                    getcurrentdata.Status = (int)BookingFlow.RateTalent;
                    sendTo = Userdata.Email;
                    body = getcurrentdata.OrderNo + ' ' + BookingFlow.RateTalent.ToString();
                    subject = getcurrentdata.OrderNo + ' ' + BookingFlow.RateTalent.ToString();
                }

                getcurrentdata.IsActive = (int)Status.Active;
             //   getcurrentdata.PayMethod = model.PayMethod;
                getcurrentdata.FileId = Convert.ToInt32(FileId);
                var getCurrentData = ITransactionManager.UpdateBookData(getcurrentdata);




                //send notifikasi
                if ((Convert.ToInt32(status) == 2))
                {
                    HelperController.NotificationEmail(TalentUserdata.Email, status, EmailTargetType.Talent, getcurrentdata.Id, getcurrentdata.OrderNo, firstName).Wait();

                }
                else
                {
                    HelperController.NotificationEmail(getcurrentdata.Email, status, EmailTargetType.User, getcurrentdata.Id, getcurrentdata.OrderNo, firstName).Wait();
                }



                return Json(new { BookId = getCurrentData.Id, Status = "OK" });

            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "UpdateBookingStatus", ex.Message);

                return Json(new { BookId = 0, Status = "Error" });
                throw ex;
            }

        }

        [HttpPost]
        public IActionResult UpdateBookingStatus(string BookId, string status)
        {
            try
            {
                var model = new BookModel();
                model.Id = Convert.ToInt32(BookId);
                var getcurrentdata = ITransactionManager.GetDataBook(model);
                getcurrentdata.UpdatedBy = HelperController.GetCookie("UserId");
                var sendTo = "";
                var body = "";
                var subject = "";
                var firstName = "";

                var Userdata = new UserModel();
                var Talentdata = new TalentModel();
                var TalentUserdata = new UserModel();

                Userdata.Id = Convert.ToInt32(getcurrentdata.BookedBy);
                Talentdata.Id = getcurrentdata.TalentId.Value;
                Talentdata = IMasterManager.GetTalent(Talentdata);

                TalentUserdata.Id = Talentdata.UserId.Value;
                TalentUserdata = IUserManagementManager.GetUser(TalentUserdata);

                Userdata = IUserManagementManager.GetUser(Userdata);
                firstName = Userdata.FirstName;
                //getcurrentdata.TotalPay = model.TotalPay;
                //getcurrentdata.PriceAmount = model.PriceAmount;
                //getcurrentdata.Potongan = model.Potongan;

                int Statusdata = Convert.ToInt32(status);

                if (Statusdata == 2)
                {
                    getcurrentdata.Status = (int)BookingFlow.Paid;
                    sendTo = Userdata.Email;
                    body = getcurrentdata.OrderNo + ' ' + BookingFlow.Paid.ToString();
                    subject = getcurrentdata.OrderNo + ' ' + BookingFlow.Paid.ToString();
                }
                else if (Statusdata == 3)
                {
                    getcurrentdata.Status = (int)BookingFlow.ProjectAccepted;
                    sendTo = Userdata.Email;
                    body = getcurrentdata.OrderNo + ' ' + BookingFlow.ProjectAccepted.ToString();
                    subject = getcurrentdata.OrderNo + ' ' + BookingFlow.ProjectAccepted.ToString();
                }
                else if (Statusdata == 4)
                {
                    getcurrentdata.Status = (int)BookingFlow.RecordingProcess;
                    sendTo = Userdata.Email;
                    body = getcurrentdata.OrderNo + ' ' + BookingFlow.RecordingProcess.ToString();
                    subject = getcurrentdata.OrderNo + ' ' + BookingFlow.RecordingProcess.ToString();
                }
                else if (Statusdata == 5)
                {
                    getcurrentdata.Status = (int)BookingFlow.MaterialAccepted;
                    sendTo = Userdata.Email;
                    body = getcurrentdata.OrderNo + ' ' + BookingFlow.MaterialAccepted.ToString();
                    subject = getcurrentdata.OrderNo + ' ' + BookingFlow.MaterialAccepted.ToString();
                }
                else if (Statusdata == 6)
                {
                    getcurrentdata.Status = (int)BookingFlow.ProjectCompleted;
                    sendTo = Userdata.Email;
                    body = getcurrentdata.OrderNo + ' ' + BookingFlow.ProjectCompleted.ToString();
                    subject = getcurrentdata.OrderNo + ' ' + BookingFlow.ProjectCompleted.ToString();
                }
                else if (Statusdata == 7)
                {
                    getcurrentdata.Status = (int)BookingFlow.RateTalent;
                    sendTo = Userdata.Email;
                    body = getcurrentdata.OrderNo + ' ' + BookingFlow.RateTalent.ToString();
                    subject = getcurrentdata.OrderNo + ' ' + BookingFlow.RateTalent.ToString();
                }

                getcurrentdata.IsActive = (int)Status.Active;
               // getcurrentdata.PayMethod = model.PayMethod;
                var getCurrentData = ITransactionManager.UpdateBookData(getcurrentdata);




                //send notifikasi
                if ((Convert.ToInt32(status) == 2))
                {
                    HelperController.NotificationEmail(TalentUserdata.Email, status, EmailTargetType.Talent, getcurrentdata.Id, getcurrentdata.OrderNo, firstName).Wait();
                    if(Talentdata.IsPriority)
                    {
                        SaldoModel saldoData = new SaldoModel();
                        saldoData.TalentId = Talentdata.Id;
                        saldoData = ITransactionManager.GetSaldoByTalentId(saldoData);
                        if(saldoData != null)
                        {
                            saldoData.SaldoUsedAmt += getcurrentdata.TotalPay;
                            saldoData = ITransactionManager.UpdateSaldo(saldoData);
                        }
                    }

                }
                else
                {
                    HelperController.NotificationEmail(getcurrentdata.Email, status, EmailTargetType.User, getcurrentdata.Id, getcurrentdata.OrderNo, firstName).Wait();
                }


                return Json(new { BookId = getCurrentData.Id, Status = "OK" });

            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "UpdateBookingStatus", ex.Message);

                return Json(new { BookId = 0, Status = "Error" });
                throw ex;
            }



        }

        [HttpPost]
        public IActionResult UpdatCompletedBookingStatus(string BookId, string status, string Rate, string Review = "")
        {
            try
            {
                var model = new BookModel();
                model.Id = Convert.ToInt32(BookId);
                var getcurrentdata = ITransactionManager.GetDataBook(model);
                getcurrentdata.UpdatedBy = HelperController.GetCookie("UserId");
                getcurrentdata.Review = Review;
                var sendTo = "";
                var body = "";
                var subject = "";
                var firstName = "";

                var Userdata = new UserModel();
                var Talentdata = new TalentModel();
                var TalentUserdata = new UserModel();

                Userdata.Id = Convert.ToInt32(getcurrentdata.BookedBy);
                Talentdata.Id = getcurrentdata.TalentId.Value;
                Talentdata = IMasterManager.GetTalent(Talentdata);

                TalentUserdata.Id = Talentdata.UserId.Value;
                TalentUserdata = IUserManagementManager.GetUser(TalentUserdata);

                Userdata = IUserManagementManager.GetUser(Userdata);
                firstName = Userdata.FirstName;
                //getcurrentdata.TotalPay = model.TotalPay;
                //getcurrentdata.PriceAmount = model.PriceAmount;
                //getcurrentdata.Potongan = model.Potongan;

                int Statusdata = Convert.ToInt32(status);

                if (Statusdata == 2)
                {
                    getcurrentdata.Status = (int)BookingFlow.Paid;
                    sendTo = Userdata.Email;
                    body = getcurrentdata.OrderNo + ' ' + BookingFlow.Paid.ToString();
                    subject = getcurrentdata.OrderNo + ' ' + BookingFlow.Paid.ToString();
                }
                else if (Statusdata == 3)
                {
                    getcurrentdata.Status = (int)BookingFlow.ProjectAccepted;
                    sendTo = Userdata.Email;
                    body = getcurrentdata.OrderNo + ' ' + BookingFlow.ProjectAccepted.ToString();
                    subject = getcurrentdata.OrderNo + ' ' + BookingFlow.ProjectAccepted.ToString();
                }
                else if (Statusdata == 4)
                {
                    getcurrentdata.Status = (int)BookingFlow.RecordingProcess;
                    sendTo = Userdata.Email;
                    body = getcurrentdata.OrderNo + ' ' + BookingFlow.RecordingProcess.ToString();
                    subject = getcurrentdata.OrderNo + ' ' + BookingFlow.RecordingProcess.ToString();
                }
                else if (Statusdata == 5)
                {
                    getcurrentdata.Status = (int)BookingFlow.MaterialAccepted;
                    sendTo = Userdata.Email;
                    body = getcurrentdata.OrderNo + ' ' + BookingFlow.MaterialAccepted.ToString();
                    subject = getcurrentdata.OrderNo + ' ' + BookingFlow.MaterialAccepted.ToString();
                }
                else if (Statusdata == 6)
                {
                    getcurrentdata.Status = (int)BookingFlow.ProjectCompleted;
                    sendTo = Userdata.Email;
                    body = getcurrentdata.OrderNo + ' ' + BookingFlow.ProjectCompleted.ToString();
                    subject = getcurrentdata.OrderNo + ' ' + BookingFlow.ProjectCompleted.ToString();
                }
                else if (Statusdata == 7)
                {
                    getcurrentdata.Status = (int)BookingFlow.RateTalent;
                    sendTo = Userdata.Email;
                    body = getcurrentdata.OrderNo + ' ' + BookingFlow.RateTalent.ToString();
                    subject = getcurrentdata.OrderNo + ' ' + BookingFlow.RateTalent.ToString();
                }

                getcurrentdata.IsActive = (int)Status.Active;
              //  getcurrentdata.PayMethod = model.PayMethod;
                var getCurrentData = ITransactionManager.UpdateBookData(getcurrentdata);




                //send notifikasi
                if ((Convert.ToInt32(status) == 2))
                {
                    HelperController.NotificationEmail(TalentUserdata.Email, status, EmailTargetType.Talent, getcurrentdata.Id, getcurrentdata.OrderNo, firstName).Wait();
                    if (Talentdata.IsPriority)
                    {
                        SaldoModel saldoData = new SaldoModel();
                        saldoData.TalentId = Talentdata.Id;
                        saldoData = ITransactionManager.GetSaldoByTalentId(saldoData);
                        if (saldoData != null)
                        {
                            saldoData.SaldoUsedAmt += getcurrentdata.TotalPay;
                            saldoData = ITransactionManager.UpdateSaldo(saldoData);
                        }
                    }

                }
                else
                {
                    HelperController.NotificationEmail(getcurrentdata.Email, status, EmailTargetType.User, getcurrentdata.Id, getcurrentdata.OrderNo, firstName).Wait();
                }

                RatingModel RateModel = new RatingModel();
                RateModel.UserId = Convert.ToInt32(HelperController.GetCookie("UserId"));
                RateModel.FileId = getcurrentdata.FileId;
                RateModel.Rate = Convert.ToInt32(Rate);
                RateModel.CreatedBy = HelperController.GetCookie("UserId");
                ITransactionManager.CreateRatingFiles(RateModel);


                return Json(new { BookId = getCurrentData.Id, Status = "OK" });

            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "UpdateBookingStatus", ex.Message);

                return Json(new { BookId = 0, Status = "Error" });
                throw ex;
            }



        }



        public IActionResult BookingHistory(string bookId, int? IsEmail)
        {
            var model = new BookModel();
            try
            {
                model.Id = Convert.ToInt32(bookId);
               
                model = ITransactionManager.GetDataBook(model);
                model.IsEmail = IsEmail;
            }
            catch (Exception ex)
            {

                HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "BookingHistory", ex.Message);

                throw;
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult PayConfirm(BookModel model)
        {
            try
            {
                var getcurrentdata = ITransactionManager.GetBookConfirmation(model);
                getcurrentdata.UpdatedBy = HelperController.GetCookie("UserId");
                getcurrentdata.TotalPay = model.TotalPay;
                getcurrentdata.PriceAmount = model.PriceAmount;
                getcurrentdata.Potongan = model.Potongan;
                getcurrentdata.Status = (int)BookingFlow.Paid;
                getcurrentdata.IsActive = (int)Status.Active;
                getcurrentdata.PayMethod = model.PayMethod;
                var getCurrentData = ITransactionManager.UpdateBookData(getcurrentdata);
                return Json(new { BookId = getCurrentData.Id, Status = "OK" });

            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "BookingHistory", ex.Message);

                return Json(new { BookId = 0, Status = "Error" });
            }



        }

        public IActionResult PaymentSuccess()
        {
            return View();
        }

        public IActionResult PaymentVA()
        {
            return View();
        }


        #region WISHLIST
        [HttpPost]
        public IActionResult AddToWishlist(WishlistModel model)
        {
            try
            {
                model.UserId = Convert.ToInt32(HelperController.GetCookie("UserId"));
                ITransactionManager.CreateWishlistData(model);
                return Json("OK");
            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "AddToWishlist", ex.Message);

                return Json("Error");
                throw ex;
            }
        }
        [HttpPost]
        public IActionResult RemoveFromWishlist(WishlistModel model)
        {
            try
            {
                model.UserId = Convert.ToInt32(HelperController.GetCookie("UserId"));
                ITransactionManager.RemoveWishlistData(model);
                return Json("OK");
            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "AddToWishlist", ex.Message);

                return Json("Error");
                throw ex;
            }
        }
        #endregion


        #region VOUCHER
        [HttpPost]
        public IActionResult VerifyVoucherCode(string VoucherCd, decimal PriceAmount)
        {
            decimal voucherAmt = 0;
            try
            {
                var getdata = IMasterManager.CheckVoucherCOde(VoucherCd);
                if (getdata != null)
                {
                    if (getdata.Amount > 0)
                    {
                        voucherAmt = getdata.Amount;
                    }
                    else if(getdata.Percentage > 0)
                    {
                        voucherAmt = (PriceAmount * getdata.Percentage) / 100;
                    }
                }
            }
            catch (Exception ex)
            {

                HelperController.InsertLog(0, "VerifyVoucherCode", ex.Message);
                return Json(voucherAmt);
                throw ex;
            }
            return Json(voucherAmt);
        }
        #endregion



    }
}