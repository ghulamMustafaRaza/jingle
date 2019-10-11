using Jingl.General.Model.Admin.Master;
using Jingl.General.Model.Admin.Transaction;
using Jingl.General.Model.User.Notification;
using Jingl.General.Model.User.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.Service.Interface
{
    public interface IMasterManager
    {
        TalentModel CreateTalent(TalentModel model);
        TalentModel GetTalent(TalentModel model);
        TalentModel UpdateTalent(TalentModel model);
        TalentModel UpdateBilling(TalentModel model);
        IList<TalentModel> GetAllTalent();
       

        IList<PayMethodModel> GetAllPayMethod();
        PayMethodModel GetDataPayMethod(PayMethodModel model);
        PayMethodModel CreatePayMethodData(PayMethodModel model);
        PayMethodModel UpdatePayMethodData(PayMethodModel model);

        IList<CategoryModel> GetAllCategory();
        CategoryModel GetDataCategory(CategoryModel model);
        CategoryModel CreateCategoryData(CategoryModel model);
        CategoryModel UpdateCategoryData(CategoryModel model);

        TalentViewModel GetAllTalentCategory();

        IList<TalentCategoryViewModel> GetAllTalentByCategory(int CategoryId);
      
        TalentModel GetTalentProfiles(int UserId);
        IList<CategoryModel> GetCategoryByType(string CategoryType);
        EmailNotificationModel GetEmailNotification(int Id);
        ParameterModel GetParameter(int Id);
        ParameterModel GetParameterByCode(string Code);
        IList<TalentCategoryViewModel> GetTalentCategoryData(int TalentId);
        IList<TalentCategoryViewModel> GetTalentCategoryAllData();
        void DeleteTalentCategoryById(int TalentId);
        TalentCategoryViewModel CreateTalentCategory(TalentCategoryViewModel model);
        DeviceModel CreateDevice(DeviceModel model);
        IList<DeviceModel> GetAllDevice();
        IList<DeviceModel> GetDeviceByUserId(int UserId);
        void DeleteDevice(int Id);
        IList<RegionModel> GetAllRegion();
        IList<ParameterModel> AdmGetAllParameter();
        ParameterModel CreateParam(ParameterModel model);
        ParameterModel UpdateParam(ParameterModel model);
        IList<BankModel> GetAllBank();

        IList<BannerModel> GetAllBanner();
        BannerModel GetBanner(BannerModel model);
        BannerModel CreateBanner(BannerModel model);
        BannerModel UpdateBanner(BannerModel model);
        void DeleteParameter(int id);
        void DeleteBanner(int id);
        void DeleteCategoryData(int id);

        #region TalentPerform
        IList<TalentPerformanceModel> GetTalentPerformance();
        IList<TalentPerformanceModel> GetTalentPerformanceByPeriod(string Period);
        #endregion

        #region Voucher
        VoucherModel CheckVoucherCOde(string VoucherCd);
        VoucherModel GetVoucherByCode(string VoucherCd);
        VoucherModel UpdateVoucher(VoucherModel model);
        #endregion
    }
}
