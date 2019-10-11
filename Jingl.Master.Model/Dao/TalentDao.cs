using Dapper;
using Jingl.General.Enum;
using Jingl.General.Model.Admin.Master;
using Jingl.General.Model.Admin.Transaction;
using Jingl.General.Model.User.ViewModel;
using Jingl.General.Utility;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Jingl.Master.Model.Dao
{
    public class TalentDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public TalentDao(IConfiguration config)
        {
            this._Logger = new Logger(config);
            this._config = config;
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DbConnection"));
            }
        }

        public TalentModel CreateTalent(TalentModel model)
        {
            var data = new TalentModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@UserId",model.UserId);                                 
                    param.Add("@FollowersCount", model.FollowersCount);
                    param.Add("@RdyVideo", model.RdyVideo);
                    param.Add("@Rate", model.Rate);
                    param.Add("@PriceAmount", model.PriceAmount);
                    param.Add("@Status", model.Status);
                    param.Add("@IsActive",true);
                    param.Add("@CreatedBy", model.UserId);
                    param.Add("@Profession", model.Profesion);
                    param.Add("@CreatedDate", DateTime.Now);
                    param.Add("@IdCardFileId", model.IdCardFileId);
                    param.Add("@AccountNumberFileId", model.AccountNumberFileId);
                    param.Add("@NPWPFileId", model.NPWPFileId);
                    param.Add("@Instagram", model.Instagram);
                    param.Add("@Facebook", model.Facebook);
              

                    data = conn.Query<TalentModel>("sp_Tbl_Mst_TalentInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }


        public TalentCategoryViewModel CreateTalentCategory(TalentCategoryViewModel model)
        {
            var data = new TalentCategoryViewModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@TalentId", model.TalentId);
                    param.Add("@CategoryId", model.CategoryId);
                   
                    //param.Add("@RoleId", 2);

                    data = conn.Query<TalentCategoryViewModel>("sp_Tbl_Mst_TalentCategoryInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }


        public void DeleteTalentCategoryByTalentId(int TalentId)
        {
           
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@TalentId", TalentId);
                 

                    //param.Add("@RoleId", 2);

                  conn.Execute("sp_Tbl_Mst_TalentCategoryDelete", param,
                               commandType: CommandType.StoredProcedure);

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }





        public TalentModel GetTalent(TalentModel model)
        {
            var data = new TalentModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.Id);


                    data = conn.Query<TalentModel>("sp_Tbl_Mst_TalentSelect", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }


        public IList<TalentCategoryViewModel> GetTalentCategoryData(int TalentId)
        {
            var data = new List<TalentCategoryViewModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<TalentCategoryViewModel>("sp_Tbl_Mst_TalentCategorySelect", param,
                               commandType: CommandType.StoredProcedure).ToList();

                    data = data.Where(x => x.TalentId == TalentId).ToList(); 


                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }



        public TalentModel GetTalentProfiles(int UserId)
        {
            var data = new TalentModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@UserId", UserId);


                    data = conn.Query<TalentModel>("Sp_GetTalentProfiles", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }


        public IList<TalentModel> GetAllTalent()
        {
            var data = new List<TalentModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<TalentModel>("sp_Tbl_Mst_TalentSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

      


        public IList<TalentCategoryViewModel> GetAllTalentByCategory(int CategoryId)
        {
            var data = new List<TalentCategoryViewModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@CategoryId", CategoryId);


                    data = conn.Query<TalentCategoryViewModel>("Sp_GetTalentByCategory", param,
                               commandType: CommandType.StoredProcedure).ToList();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }




        public TalentModel UpdateTalent(TalentModel model)
        {
            var data = new TalentModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.Id);                 
                    param.Add("@UserId", model.UserId);               
                  
                    param.Add("@FollowersCount", model.FollowersCount);
                    param.Add("@Profession", model.Profesion);
                    param.Add("@RdyVideo", model.RdyVideo);
                    param.Add("@Level", model.Level);
                    param.Add("@Email", model.Email);
                    param.Add("@Rate", model.Rate);
                    param.Add("@PriceAmount", model.PriceAmount);
                    param.Add("@Status", model.Status);
                    param.Add("@UpdatedBy", model.UserId);
                    param.Add("@IsActive", model.IsActive);
                    param.Add("@IdCardFileId", model.IdCardFileId);
                    param.Add("@AccountNumberFileId", model.AccountNumberFileId);
                    param.Add("@NPWPFileId", model.NPWPFileId);                   
                    param.Add("@Instagram", model.Instagram);
                    param.Add("@Facebook", model.Facebook);
                    param.Add("@Note", model.Note);
                    param.Add("@IsPriority", model.IsPriority);


                    data = conn.Query<TalentModel>("sp_Tbl_Mst_TalentUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public TalentModel UpdateBilling(TalentModel model)
        {
            var data = new TalentModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.Id);
                    param.Add("@Bank", model.Bank);
                    param.Add("@AccountNumber", model.AccountNumber);
                    param.Add("@BeneficiaryName", model.BeneficiaryName);
             


                    data = conn.Query<TalentModel>("sp_Tbl_Mst_TalentUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }


        public IList<TalentCategoryViewModel> GetTalentCategoryAllData()
        {
            var data = new List<TalentCategoryViewModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<TalentCategoryViewModel>("sp_Tbl_Mst_TalentCategoryAll", param,
                               commandType: CommandType.StoredProcedure).ToList();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }


        #region TalentPerformance
        public IList<TalentPerformanceModel> GetTalentPerformance()
        {
            var data = new List<TalentPerformanceModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<TalentPerformanceModel>("sp_Tbl_Mst_TalentPerformance", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }
        public IList<TalentPerformanceModel> GetTalentPerformanceByPeriod(string Period)
        {
            var data = new List<TalentPerformanceModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Period", Period);


                    data = conn.Query<TalentPerformanceModel>("sp_Tbl_Mst_TalentPerformanceByPeriod", param,
                               commandType: CommandType.StoredProcedure).ToList();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }
        #endregion


    }
}
