﻿using System;
using System.Collections.Generic;
using System.Text;

using Jingl.Service.Interface;
using Jingl.Admin.UserManagement.Model.Dao;
using Jingl.General.Model.Admin.ViewModel;
using Jingl.General.Utility;
using Microsoft.Extensions.Configuration;
using Jingl.General.Model.Admin.Master;
using Jingl.Master.Model.Dao;
using Jingl.General.Model.User.ViewModel;
using Jingl.General.Model.Admin.Transaction;
using Jingl.General.Model.User.Notification;

namespace Jingl.Service.Manager
{
    public class MasterManager:IMasterManager
    {
        private readonly ParameterDao ParameterDao;
        private readonly TalentDao TalentDao;
        private readonly CategoryDao CategoryDao;
        private readonly PayMethodDao PayMethodDao;
        private readonly EmailNotificationDao EmailNotificationDao;
        private readonly IConfiguration _config;
        private readonly Logger _logger;
        private readonly DeviceDao DeviceDao;
        private readonly RegionDao RegionDao;
        private readonly BankDao BankDao;
        private readonly BannerDao BannerDao;
        private readonly VoucherDao VoucherDao;


        public MasterManager(IConfiguration _config)
        {
            this.RegionDao = new RegionDao(_config);
            this.DeviceDao = new DeviceDao(_config);
            this.ParameterDao = new ParameterDao(_config);
            this._config = _config;
            this.TalentDao = new TalentDao(_config);
            this.CategoryDao = new CategoryDao(_config);
            this.PayMethodDao = new PayMethodDao(_config);
            this.EmailNotificationDao = new EmailNotificationDao(_config);
            this._logger = new Logger(_config);
            this.BankDao = new BankDao(_config);
            this.BannerDao = new BannerDao(_config);
            this.VoucherDao = new VoucherDao(_config);
        }

        public string DestinationLogFolder()
        {
            return _config.GetSection("Logging:DestinationFolder:Service").Value.ToString();
        }


        public TalentModel CreateTalent(TalentModel model)
        {
            var data = new TalentModel();

            try
            {
                data = TalentDao.CreateTalent(model);


            }
            catch (Exception ex)
            { 
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateTalent", ex.Message, "Service");

            }

            return data;
        }

        public TalentModel GetTalent(TalentModel model)
        {
            var data = new TalentModel();

            try
            {
                data = TalentDao.GetTalent(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetTalent", ex.Message, "Service");

            }

            return data;
        }

       

        public IList<TalentModel> GetAllTalent()
        {
            IList<TalentModel> data = new List<TalentModel>();

            try
            {
                data = TalentDao.GetAllTalent();


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllTalent", ex.Message, "Service");

            }

            return data;
        }

        

        public TalentModel UpdateTalent(TalentModel model)
        {
            var data = new TalentModel();

            try
            {
                data = TalentDao.UpdateTalent(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdateTalent", ex.Message, "Service");

            }

            return data;
        }

        public TalentModel UpdateBilling(TalentModel model)
        {
            var data = new TalentModel();

            try
            {
                data = TalentDao.UpdateBilling(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdateBilling", ex.Message, "Service");

            }

            return data;
        }


        public IList<PayMethodModel> GetAllPayMethod()
        {
            IList<PayMethodModel> data = new List<PayMethodModel>();

            try
            {
                data = PayMethodDao.GetAllPayMethod();
              
                

            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllPayMethod", ex.Message, "Service");

            }

            return data;
        }

        public PayMethodModel GetDataPayMethod(PayMethodModel model)
        {
            var data = new PayMethodModel();

            try
            {
                data = PayMethodDao.GetDataPayMethod(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetDataPayMethod", ex.Message, "Service");

            }

            return data;
        }

        public PayMethodModel CreatePayMethodData(PayMethodModel model)
        {
            var data = new PayMethodModel();

            try
            {
                data = PayMethodDao.CreatePayMethodData(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreatePayMethodData", ex.Message, "Service");

            }

            return data;
        }

        public PayMethodModel UpdatePayMethodData(PayMethodModel model)
        {
            var data = new PayMethodModel();

            try
            {
                data = PayMethodDao.UpdatePayMethodData(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdatePayMethodData", ex.Message, "Service");

            }

            return data;
        }

        public IList<CategoryModel> GetCategoryByType(string CategoryType)
        {
            IList<CategoryModel> data = new List<CategoryModel>();

            try
            {
                data = CategoryDao.GetCategoryByType(CategoryType);



            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllCategory", ex.Message, "Service");

            }

            return data;
        }

        public IList<CategoryModel> GetAllCategory()
        {
            IList<CategoryModel> data = new List<CategoryModel>();

            try
            {
                data = CategoryDao.GetAllCategory();



            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllCategory", ex.Message, "Service");

            }

            return data;
        }

        public CategoryModel GetDataCategory(CategoryModel model)
        {
            var data = new CategoryModel();

            try
            {
                data = CategoryDao.GetDataCategory(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetDataCategory", ex.Message, "Service");

            }

            return data;
        }

        public CategoryModel CreateCategoryData(CategoryModel model)
        {
            var data = new CategoryModel();

            try
            {
                data = CategoryDao.CreateCategoryData(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateCategoryData", ex.Message, "Service");

            }

            return data;
        }

        public CategoryModel UpdateCategoryData(CategoryModel model)
        {
            var data = new CategoryModel();

            try
            {
                data = CategoryDao.UpdateCategoryData(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdateCategoryData", ex.Message, "Service");

            }

            return data;
        }


        public TalentViewModel GetAllTalentCategory()
        {
            var data = new TalentViewModel();

            try
            {
                data.ListCategoryModel = GetCategoryByType("Talent");
                data.ListTalentModel = GetAllTalent();

            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllTalentCategory", ex.Message, "Service");

            }

            return data;
        }

        public IList<TalentCategoryViewModel> GetAllTalentByCategory(int CategoryId)
        {
           IList<TalentCategoryViewModel> data = new List<TalentCategoryViewModel>();

            try
            {
                data = TalentDao.GetAllTalentByCategory(CategoryId);

            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllTalentByCategory", ex.Message, "Service");

            }

            return data;
        }

        public TalentModel GetTalentProfiles(int UserId)
        {
            var data = new TalentModel();

            try
            {
                data = TalentDao.GetTalentProfiles(UserId);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetTalentProfiles", ex.Message, "Service");

            }

            return data;
        }

        public EmailNotificationModel GetEmailNotification(int Id)
        {
            var data = new EmailNotificationModel();

            try
            {
                data = EmailNotificationDao.GetEmailNotification(Id);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetEmailNotification", ex.Message, "Service");

            }

            return data;
        }

        public ParameterModel GetParameter(int Id)
        {
            var data = new ParameterModel();

            try
            {
                data = ParameterDao.GetParameter(Id);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetParameter", ex.Message, "Service");

            }

            return data;
        }

        public ParameterModel GetParameterByCode(string Code)
        {
            var data = new ParameterModel();

            try
            {
                data = ParameterDao.GetParameterByCode(Code);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetParameterByCode", ex.Message, "Service");

            }

            return data;
        }

       public IList<TalentCategoryViewModel> GetTalentCategoryData(int TalentId)
        {
            IList<TalentCategoryViewModel> data = new List<TalentCategoryViewModel>();

            try
            {
                data = TalentDao.GetTalentCategoryData(TalentId);

            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetTalentCategoryData", ex.Message, "Service");

            }

            return data;
        }

        public IList<TalentCategoryViewModel> GetTalentCategoryAllData()
        {
            IList<TalentCategoryViewModel> data = new List<TalentCategoryViewModel>();

            try
            {
                data = TalentDao.GetTalentCategoryAllData();

            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetTalentCategoryData", ex.Message, "Service");

            }

            return data;
        }

        public void DeleteTalentCategoryById(int TalentId)
        {
            try
            {
                 TalentDao.DeleteTalentCategoryByTalentId(TalentId);



            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteTalentCategoryById", ex.Message, "Service");

            }
        }

        public TalentCategoryViewModel CreateTalentCategory(TalentCategoryViewModel model)
        {
            var data = new TalentCategoryViewModel();

            try
            {
                data = TalentDao.CreateTalentCategory(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateTalentCategory", ex.Message, "Service");

            }

            return data;
        }

        public DeviceModel CreateDevice(DeviceModel model)
        {
            var data = new DeviceModel();

            try
            {
                data = DeviceDao.CreateDevice(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateDevice", ex.Message, "Service");

            }

            return data;
        }

        public IList<DeviceModel> GetAllDevice()
        {
            IList<DeviceModel> data = new List<DeviceModel>();

            try
            {
                data = DeviceDao.GetAllDevice();
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllDevice", ex.Message, "Service");

            }

            return data;
        }

        public IList<DeviceModel> GetDeviceByUserId(int UserId)
        {
            IList<DeviceModel> data = new List<DeviceModel>();

            try
            {
                data = DeviceDao.GetDeviceByUserId(UserId);
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetDeviceByUserId", ex.Message, "Service");

            }

            return data;
        }

        public void DeleteDevice(int Id)
        {
            try
            {
              DeviceDao.DeleteDevice(Id);
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteDevice", ex.Message, "Service");

            }
        }

        public IList<RegionModel> GetAllRegion()
        {
            IList<RegionModel> data = new List<RegionModel>();

            try
            {
                data = RegionDao.GetAllRegion();
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllRegion", ex.Message, "Service");

            }

            return data;
        }

        public IList<ParameterModel> AdmGetAllParameter()
        {
            IList<ParameterModel> data = new List<ParameterModel>();

            try
            {
                data = ParameterDao.AdmGetAllParameter();
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "AdmGetAllParameter", ex.Message, "Service");

            }

            return data;
        }


        public ParameterModel CreateParam(ParameterModel model)
        {
            var data = new ParameterModel();

            try
            {
                data = ParameterDao.CreateParam(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateParam", ex.Message, "Service");

            }

            return data;

        }

        public ParameterModel UpdateParam(ParameterModel model)
        {
            var data = new ParameterModel();

            try
            {
                data = ParameterDao.UpdateParam(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdateParam", ex.Message, "Service");

            }

            return data;
        }

        public IList<BankModel> GetAllBank()
        {
            IList<BankModel> data = new List<BankModel>();

            try
            {
                data = BankDao.GetAllBank();
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllBank", ex.Message, "Service");

            }

            return data;
        }

        public IList<BannerModel> GetAllBanner()
        {
            IList<BannerModel> data = new List<BannerModel>();

            try
            {
                data = BannerDao.GetAllBanner();
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllBanner", ex.Message, "Service");

            }

            return data;
        }

        public BannerModel GetBanner(BannerModel model)
        {
            var data = new BannerModel();

            try
            {
                data = BannerDao.GetBanner(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetBanner", ex.Message, "Service");

            }

            return data;
        }

        public BannerModel CreateBanner(BannerModel model)
        {
            var data = new BannerModel();

            try
            {
                data = BannerDao.CreateBanner(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateBanner", ex.Message, "Service");

            }

            return data;
        }

        public BannerModel UpdateBanner(BannerModel model)
        {
            var data = new BannerModel();

            try
            {
                data = BannerDao.UpdateBanner(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdateBanner", ex.Message, "Service");

            }

            return data;
        }

        public void DeleteParameter(int id)
        {
            try
            {
                 ParameterDao.DeleteParameter(id);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteParameter", ex.Message, "Service");

            }
        }

        public void DeleteBanner(int id)
        {
            try
            {
                BannerDao.DeleteBanner(id);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteBanner", ex.Message, "Service");

            }
        }

        public void DeleteCategoryData(int id)
        {
            try
            {
                CategoryDao.DeleteCategoryData(id);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteCategoryData", ex.Message, "Service");

            }
        }

        #region TalentPerform
        public IList<TalentPerformanceModel> GetTalentPerformance()
        {
            IList<TalentPerformanceModel> data = new List<TalentPerformanceModel>();

            try
            {
                data = TalentDao.GetTalentPerformance();


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetTalentPerformance", ex.Message, "Service");

            }

            return data;
        }
        public IList<TalentPerformanceModel> GetTalentPerformanceByPeriod(string Period)
        {
            IList<TalentPerformanceModel> data = new List<TalentPerformanceModel>();

            try
            {
                data = TalentDao.GetTalentPerformanceByPeriod(Period);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetTalentPerformanceByPeriod", ex.Message, "Service");

            }

            return data;
        }
        #endregion

        #region Voucher
        public VoucherModel CheckVoucherCOde(string VoucherCd)
        {
            VoucherModel voucher = new VoucherModel();
            try
            {
                voucher = VoucherDao.CheckVoucherCOde(VoucherCd);

            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CheckVoucherCOde", ex.Message, "Service");

            }

            return voucher;
        }
        public VoucherModel GetVoucherByCode(string VoucherCd)
        {
            VoucherModel voucher = new VoucherModel();
            try
            {
                voucher = VoucherDao.GetVoucherByCode(VoucherCd);

            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetVoucherByCode", ex.Message, "Service");

            }

            return voucher;
        }
        public VoucherModel UpdateVoucher(VoucherModel model)
        {
            var data = new VoucherModel();

            try
            {
                data = VoucherDao.UpdateVoucher(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdateVoucher", ex.Message, "Service");

            }

            return data;
        }
        #endregion
    }
}
