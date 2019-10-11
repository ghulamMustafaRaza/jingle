using System;
using System.Collections.Generic;
using System.Text;

using Jingl.Service.Interface;
using Jingl.General.Utility;
using Microsoft.Extensions.Configuration;
using Jingl.General.Model.Admin.Transaction;
using Jingl.Transaction.Model.Dao;
using Jingl.General.Model.User.ViewModel;
using Jingl.Master.Model.Dao;

namespace Jingl.Service.Manager
{
    public class TransactionManager : ITransactionManager
    {
        private readonly PaymentBookLogDao PaymentBookLogDao;
        private readonly NotificationDao NotificationDao;
        private readonly SupportDao SupportDao;
        private readonly BookDao BookDao;
        private readonly TalentVideoDao TalentVideoDao;
        private readonly FilesDao FilesDao;
        private readonly WishlistDao WishlistDao;
        private readonly IConfiguration _config;
        private readonly Logger _logger;
        private readonly ClaimDao ClaimDao;
        private readonly RefundDao RefundDao;
        private readonly TalentRegistrationDao TalentRegistrationDao;
        private readonly SaldoDao SaldoDao;
        private readonly TopupDao TopupDao;


        public TransactionManager(IConfiguration _config)
        {
            this.PaymentBookLogDao = new PaymentBookLogDao(_config);
            this.NotificationDao = new NotificationDao(_config);
            this.SupportDao = new SupportDao(_config);
            this._config = _config;
            this.FilesDao = new FilesDao(_config);
            this.BookDao = new BookDao(_config);
            this.TalentVideoDao = new TalentVideoDao(_config);
            this.WishlistDao = new WishlistDao(_config);
            this._logger = new Logger(_config);
            this.RefundDao = new RefundDao(_config);
            this.ClaimDao = new ClaimDao(_config);
            this.TalentRegistrationDao = new TalentRegistrationDao(_config);
            this.SaldoDao = new SaldoDao(_config);
            this.TopupDao = new TopupDao(_config);

        }


        #region WISHLIST
        public WishlistModel CreateWishlistData(WishlistModel model)
        {
            var data = new WishlistModel();

            try
            {
                data = WishlistDao.CreateWishlistData(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateWishlistData", ex.Message, "Service");

            }

            return data;
        }
        public IList<TalentCategoryViewModel> GetWishListByUserId(int UserId)
        {
            IList<TalentCategoryViewModel> data = new List<TalentCategoryViewModel>();

            try
            {
                data = WishlistDao.GetWishListByUserId(UserId);

            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetWishListByUserId", ex.Message, "Service");

            }

            return data;
        }
        public int GetWishlistIdByUserTalent(WishlistModel model)
        {
            int id = 0;
            try
            {
                id = WishlistDao.GetWishlistIdByUserTalent(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetWishlistIdByUserTalent", ex.Message, "Service");

            }
            return id;
        }
        public void RemoveWishlistData(WishlistModel model)
        {
            try
            {
                WishlistDao.RemoveWishlistData(model);

            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "RemoveWishlistData", ex.Message, "Service");
            }
        }
        #endregion

        public string DestinationLogFolder()
        {
            return _config.GetSection("Logging:DestinationFolder:Service").Value.ToString();
        }

        public BookModel CreateBookData(BookModel model)
        {
            var data = new BookModel();

            try
            {
                data = BookDao.CreateBookData(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateBookData", ex.Message, "Service");

            }

            return data;
        }

        public IList<BookModel> GetAllBook()
        {
            IList<BookModel> data = new List<BookModel>();

            try
            {
                data = BookDao.GetAllBook();



            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllBook", ex.Message, "Service");

            }

            return data;
        }

        public IList<BookModel> GetBookingByUserId(int UserId)
        {
            IList<BookModel> data = new List<BookModel>();

            try
            {
                data = BookDao.GetBookingByUserId(UserId);



            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetBookingByUserId", ex.Message, "Service");

            }

            return data;
        }

        public BookModel GetDataBook(BookModel model)
        {
            var data = new BookModel();

            try
            {
                data = BookDao.GetDataBook(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetDataBook", ex.Message, "Service");

            }

            return data;
        }

        public BookModel GetBookConfirmation(BookModel model)
        {
            var data = new BookModel();

            try
            {
                data = BookDao.GetBookConfirmation(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetBookConfirmation", ex.Message, "Service");

            }

            return data;
        }


        public BookModel UpdateBookData(BookModel model)
        {
            var data = new BookModel();

            try
            {
                data = BookDao.UpdateBookData(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdateBookData", ex.Message, "Service");

            }


            return data;
        }
              

        public IList<SupportModel> GetAllSupport()
        {
            IList<SupportModel> data = new List<SupportModel>();

            try
            {
                data = SupportDao.GetAllSupport();
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllBook", ex.Message, "Service");

            }

            return data;
        }
        public SupportModel CreateSupport(SupportModel model)
        {
            var data = new SupportModel();

            try
            {
                data = SupportDao.CreateSupport(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateSupport", ex.Message, "Service");

            }

            return data;
        }

        public FilesModel CreateFiles(FilesModel model)
        {
            var data = new FilesModel();

            try
            {
                data = FilesDao.CreateFiles(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateFiles", ex.Message, "Service");

            }

            return data;
        }


        public SupportModel GetSupport(SupportModel model)
        {
            var data = new SupportModel();

            try
            {
                data = SupportDao.GetSupport(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetSupport", ex.Message, "Service");

            }

            return data;

        }

        public IList<NotificationModel> GetNotificationForUser(int UserId)
        {
            IList<NotificationModel> data = new List<NotificationModel>();

            try
            {
                data = NotificationDao.GetNotificationForUser(UserId);
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetNotificationForUser", ex.Message, "Service");

            }

            return data;
        }

        public IList<NotificationModel> GetNotificationForTalent(int UserId)
        {
            IList<NotificationModel> data = new List<NotificationModel>();

            try
            {
                data = NotificationDao.GetNotificationForTalent(UserId);
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetNotificationForTalent", ex.Message, "Service");

            }

            return data;
        }

        public NotificationModel InsertNotification(NotificationModel model)
        {
            NotificationModel data = new NotificationModel();

            try
            {
                data = NotificationDao.InsertNotification(model);
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "InsertNotification", ex.Message, "Service");

            }

            return data;
        }

        public PaymentBookLogModel CreatePaymentBookLog(PaymentBookLogModel model)
        {
            var data = new PaymentBookLogModel();

            try
            {
                data = PaymentBookLogDao.CreatePaymentBookLog(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreatePaymentBookLog", ex.Message, "Service");

            }

            return data;
        }

        public BookModel GetDataBookByOrderId(string orderNo)
        {
            var data = new BookModel();

            try
            {
                data = BookDao.GetDataBookByOrderId(orderNo);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetDataBookByOrderId", ex.Message, "Service");

            }

            return data;
        }

        public IList<TalentVideoModel> GetTalentVideos(int TalentId)
        {
            IList<TalentVideoModel> data = new List<TalentVideoModel>();

            try
            {
                data = TalentVideoDao.GetTalentVideos(TalentId);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetTalentVideos", ex.Message, "Service");

            }

            return data;
        }

        public IList<TalentVideoModel> GetUserVideos(int UserId)
        {
            IList<TalentVideoModel> data = new List<TalentVideoModel>();

            try
            {
                data = BookDao.GetUserVideos(UserId);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetUserVideos", ex.Message, "Service");

            }

            return data;
        }

        public RatingModel CreateRatingFiles(RatingModel model)
        {
            var data = new RatingModel();

            try
            {
                data = FilesDao.CreateRatingFiles(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateRatingFiles", ex.Message, "Service");

            }

            return data;
        }

        public IList<BookModel> GetBookingByTalentId(int TalentId)
        {
            IList<BookModel> data = new List<BookModel>();

            try
            {
                data = BookDao.GetBookingByTalentId(TalentId);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetBookingByTalentId", ex.Message, "Service");

            }

            return data;
        }
        public IList<BookModel> GetBookingPaidByTalentId(int TalentId)
        {
            IList<BookModel> data = new List<BookModel>();

            try
            {
                data = BookDao.GetBookingPaidByTalentId(TalentId);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetBookingPaidByTalentId", ex.Message, "Service");

            }

            return data;
        }

        public void IsReadedNotification(int Id)
        {
            try
            {
                NotificationDao.IsReadedNotification(Id);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "IsReadedNotification", ex.Message, "Service");

            }
        }
        public IList<BookModel> AdmGetAllBook()
        {
            IList<BookModel> data = new List<BookModel>();

            try
            {
                data = BookDao.AdmGetAllBook();
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "AdmGetAllBook", ex.Message, "Service");

            }

            return data;
        }
        public BookModel AdmGetDataBook(BookModel model)
        {
            var data = new BookModel();

            try
            {
                data = BookDao.AdmGetDataBook(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "AdmGetDataBook", ex.Message, "Service");

            }

            return data;
        }

        public SupportModel UpdateSupport(SupportModel model)
        {
            var data = new SupportModel();

            try
            {
                data = SupportDao.UpdateSupport(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdateSupport", ex.Message, "Service");

            }

            return data;
        }

        public void CreateTalentVideo(TalentVideoModel model)
        {
            try
            {
                TalentVideoDao.CreateTalentVideo(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateTalentVideo", ex.Message, "Service");

            }
        }

        public void DeleteTalentVideoByTalentId(int TalentId)
        {
            try
            {
                TalentVideoDao.DeleteTalentVideoByTalentId(TalentId);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteTalentVideoByTalentId", ex.Message, "Service");

            }
        }

        public void DeleteTalentVideo(int Id)
        {
            try
            {
                TalentVideoDao.DeleteTalentVideo(Id);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteTalentVideo", ex.Message, "Service");

            }
        }

        public IList<TalentVideoModel> GetAllVideo()
        {
            IList<TalentVideoModel> data = new List<TalentVideoModel>();

            try
            {
                data = TalentVideoDao.GetAllVideo();
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllVideo", ex.Message, "Service");

            }

            return data;
        }

        public IList<ClaimModel> GetClaimByPeriod(string Period)
        {
            IList<ClaimModel> data = new List<ClaimModel>();

            try
            {
                data = ClaimDao.GetClaimByPeriod(Period);
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetClaimByPeriod", ex.Message, "Service");

            }

            return data;
        }

        public IList<ClaimModel> GetAllClaim()
        {
            IList<ClaimModel> data = new List<ClaimModel>();

            try
            {
                data = ClaimDao.GetAllClaim();
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllClaim", ex.Message, "Service");

            }

            return data;
        }

        public ClaimModel GetClaim(int id)
        {
           ClaimModel data = new ClaimModel();

            try
            {
                data = ClaimDao.GetClaim(id);
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetClaim", ex.Message, "Service");

            }

            return data;
        }

        public ClaimModel CreateClaim(ClaimModel model)
        {
            var data = new ClaimModel();

            try
            {
                data = ClaimDao.CreateClaim(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateClaim", ex.Message, "Service");

            }

            return data;
        }

        public ClaimModel UpdateClaim(ClaimModel model)
        {
            var data = new ClaimModel();

            try
            {
                data = ClaimDao.UpdateClaim(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdateClaim", ex.Message, "Service");

            }

            return data;
        }

        public IList<RefundModel> GetRefundByBatchNumber(string BatchNumber)
        {
            IList<RefundModel> data = new List<RefundModel>();

            try
            {
                data = RefundDao.GetRefundByBatchNumber(BatchNumber);
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetRefundByBatchNumber", ex.Message, "Service");

            }

            return data;
        }

        public IList<RefundModel> GetAllRefund()
        {
            IList<RefundModel> data = new List<RefundModel>();

            try
            {
                data = RefundDao.GetAllRefund();
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllRefund", ex.Message, "Service");

            }

            return data;
        }

        public RefundModel GetRefund(int id)
        {
            var data = new RefundModel();

            try
            {
                data = RefundDao.GetRefund(id);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetRefund", ex.Message, "Service");

            }

            return data;
        }

        public RefundModel CreateRefund(RefundModel model)
        {
            var data = new RefundModel();

            try
            {
                data = RefundDao.CreateRefund(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateRefund", ex.Message, "Service");

            }

            return data;
        }

        public RefundModel UpdateRefund(RefundModel model)
        {
            var data = new RefundModel();

            try
            {
                data = RefundDao.UpdateRefund(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdateRefund", ex.Message, "Service");

            }

            return data;
        }

        public IList<BookModel> GetDailyPayment(BookModel model)
        {
            IList<BookModel> data = new List<BookModel>();

            try
            {
                data = BookDao.GetDailyPayment(model);
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetDailyPayment", ex.Message, "Service");

            }

            return data;
        }

        public IList<TalentVideoModel> GetAllVideoByCategory(int? CategoryId)
        {
            IList<TalentVideoModel> data = new List<TalentVideoModel>();

            try
            {
                data = FilesDao.GetAllVideoByCategory(CategoryId);
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllVideoByCategory", ex.Message, "Service");

            }

            return data;
        }

        public TalentRegModel GetTalentRegistration(int Id)
        {
            var data = new TalentRegModel();

            try
            {
                data = TalentRegistrationDao.GetTalentRegistration(Id);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetTalentRegistration", ex.Message, "Service");

            }

            return data;
        }

        public IList<TalentRegModel> GetAllTalentRegistration()
        {
            IList<TalentRegModel> data = new List<TalentRegModel>();

            try
            {
                data = TalentRegistrationDao.GetAllTalentRegistration();
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllVideoByCategory", ex.Message, "Service");

            }

            return data;
        }

        public TalentRegModel CreateTalentRegistration(TalentRegModel model)
        {
            var data = new TalentRegModel();

            try
            {
                data = TalentRegistrationDao.CreateTalentRegistration(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateTalentRegistration", ex.Message, "Service");

            }

            return data;
        }

        public TalentRegModel UpdateTalentRegistration(TalentRegModel model)
        {
            var data = new TalentRegModel();

            try
            {
                data = TalentRegistrationDao.UpdateTalentRegistration(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdateTalentRegistration", ex.Message, "Service");

            }

            return data;
        }

        public FilesWatchModel CreateFilesWatch(FilesWatchModel model)
        {
            var data = new FilesWatchModel();

            try
            {
                data = FilesDao.CreateFilesWatch(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateFilesWatch", ex.Message, "Service");

            }

            return data;
        }

        public void DeleteBook(int id)
        {
            try
            {
                BookDao.DeleteBook(id);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteBook", ex.Message, "Service");

            }
        }

        public void DeleteSupport(int id)
        {
            try
            {
                SupportDao.DeleteSupport(id);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteSupport", ex.Message, "Service");

            }
        }

        public BookModel AdmUpdateBookData(BookModel model)
        {
            var data = new BookModel();

            try
            {
                data = BookDao.AdmUpdateBookData(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "AdmUpdateBookData", ex.Message, "Service");

            }


            return data;
        }

        #region SALDO & TOPUP
        public SaldoModel CreateSaldo(SaldoModel model)
        {
            var data = new SaldoModel();

            try
            {
                data = SaldoDao.CreateSaldo(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateSaldo", ex.Message, "Service");

            }


            return data;
        }
        public SaldoModel UpdateSaldo(SaldoModel model)
        {
            var data = new SaldoModel();

            try
            {
                data = SaldoDao.UpdateSaldo(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdateSaldo", ex.Message, "Service");

            }


            return data;
        }
        public IList<SaldoModel> GetAllSaldo()
        {
            IList<SaldoModel> data = new List<SaldoModel>();

            try
            {
                data = SaldoDao.GetAllSaldo();


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllSaldo", ex.Message, "Service");

            }


            return data;
        }
        public SaldoModel GetSaldoById(SaldoModel model)
        {
            var data = new SaldoModel();

            try
            {
                data = SaldoDao.GetSaldoById(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetSaldoById", ex.Message, "Service");

            }


            return data;
        }
        public SaldoModel GetSaldoByTalentId(SaldoModel model)
        {
            var data = new SaldoModel();

            try
            {
                data = SaldoDao.GetSaldoByTalentId(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetSaldoByTalentId", ex.Message, "Service");

            }


            return data;
        }

        public TopupModel CreateTopup(TopupModel model)
        {
            var data = new TopupModel();

            try
            {
                data = TopupDao.CreateTopup(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateSaldo", ex.Message, "Service");

            }


            return data;

        }
        public IList<TopupModel> GetTopupBySaldoId(TopupModel model)
        {
            IList<TopupModel> data = new List<TopupModel>();

            try
            {
                data = TopupDao.GetTopupBySaldoId(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllSaldo", ex.Message, "Service");

            }


            return data;
        }
        public IList<TopupModel> GetTopupByStatus(TopupModel model)
        {
            IList<TopupModel> data = new List<TopupModel>();

            try
            {
                data = TopupDao.GetTopupByStatus(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetRequestTopup", ex.Message, "Service");

            }


            return data;
        }
        public TopupModel TopupApproval(TopupModel model)
        {
            TopupModel data = new TopupModel();

            try
            {
                data = TopupDao.TopupApproval(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "TopupApproval", ex.Message, "Service");

            }


            return data;
        }
        public TopupModel GetTopupById(TopupModel model)
        {
            TopupModel data = new TopupModel();

            try
            {
                data = TopupDao.GetTopupById(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "TopupApproval", ex.Message, "Service");

            }


            return data;
        }
        #endregion

    }
}
