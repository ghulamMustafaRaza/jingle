using Jingl.General.Model.Admin.Transaction;
using Jingl.General.Model.User.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.Service.Interface
{
    public interface ITransactionManager
    {
        IList<BookModel> GetAllBook();
        BookModel GetDataBook(BookModel model);
        BookModel CreateBookData(BookModel model);
        BookModel UpdateBookData(BookModel model);
        WishlistModel CreateWishlistData(WishlistModel model);
        BookModel GetBookConfirmation(BookModel model);
        IList<TalentCategoryViewModel> GetWishListByUserId(int userId);
        IList<BookModel> GetBookingByUserId(int UserId);
        IList<SupportModel> GetAllSupport();
        SupportModel CreateSupport(SupportModel model);
        SupportModel GetSupport(SupportModel model);
        IList<NotificationModel> GetNotificationForUser(int UserId);
        IList<NotificationModel> GetNotificationForTalent(int UserId);
        NotificationModel InsertNotification(NotificationModel model);
        PaymentBookLogModel CreatePaymentBookLog(PaymentBookLogModel model);
        BookModel GetDataBookByOrderId(string orderNo);
        FilesModel CreateFiles(FilesModel model);
        IList<TalentVideoModel> GetTalentVideos(int TalentId);
        IList<TalentVideoModel> GetUserVideos(int UserId);
        RatingModel CreateRatingFiles(RatingModel model);
        IList<BookModel> GetBookingByTalentId(int UserId);
        IList<BookModel> GetBookingPaidByTalentId(int TalentId);
        void IsReadedNotification(int Id);
        IList<BookModel> AdmGetAllBook();
        BookModel AdmGetDataBook(BookModel model);
        SupportModel UpdateSupport(SupportModel model);
        void CreateTalentVideo(TalentVideoModel model);
        void DeleteTalentVideoByTalentId(int TalentId);
        void DeleteTalentVideo(int Id);
        IList<TalentVideoModel> GetAllVideo();
        IList<ClaimModel> GetClaimByPeriod(string Period);
        IList<ClaimModel> GetAllClaim();
        ClaimModel GetClaim(int id);
        ClaimModel CreateClaim(ClaimModel model);
        ClaimModel UpdateClaim(ClaimModel model);
        IList<RefundModel> GetRefundByBatchNumber(string Period);
        IList<RefundModel> GetAllRefund();
        RefundModel GetRefund(int id);
        RefundModel CreateRefund(RefundModel model);
        RefundModel UpdateRefund(RefundModel model);
        IList<BookModel> GetDailyPayment(BookModel model);
        IList<TalentVideoModel> GetAllVideoByCategory(int? CategoryId);
        TalentRegModel GetTalentRegistration(int Id);
        IList<TalentRegModel> GetAllTalentRegistration();
        TalentRegModel CreateTalentRegistration(TalentRegModel model);
        TalentRegModel UpdateTalentRegistration(TalentRegModel model);
        FilesWatchModel CreateFilesWatch(FilesWatchModel model);
        void DeleteBook(int id);
        void DeleteSupport(int id);
        BookModel AdmUpdateBookData(BookModel model);
        int GetWishlistIdByUserTalent(WishlistModel model);
        void RemoveWishlistData(WishlistModel model);

        #region SALDO & TOPUP
        SaldoModel CreateSaldo(SaldoModel model);
        SaldoModel UpdateSaldo(SaldoModel model);
        IList<SaldoModel> GetAllSaldo();
        SaldoModel GetSaldoById(SaldoModel model);
        SaldoModel GetSaldoByTalentId(SaldoModel model);

        TopupModel CreateTopup(TopupModel model);
        IList<TopupModel> GetTopupBySaldoId(TopupModel model);
        IList<TopupModel> GetTopupByStatus(TopupModel model);
        TopupModel TopupApproval(TopupModel model);
        TopupModel GetTopupById(TopupModel model);
        #endregion
    }
}
