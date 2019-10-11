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
    public class VoucherDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;

        #region INITIAL
        public VoucherDao(IConfiguration config)
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
        #endregion

        #region METHOD
        public VoucherModel CheckVoucherCOde(string VoucherCd)
        {
            var voucher = new VoucherModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@VoucherCd", VoucherCd);


                    voucher = conn.Query<VoucherModel>("sp_GetVerifyVoucherCode", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return voucher;
        }
        public VoucherModel GetVoucherByCode(string VoucherCd)
        {
            var voucher = new VoucherModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@VoucherCd", VoucherCd);


                    voucher = conn.Query<VoucherModel>("sp_GetVoucherByCode", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return voucher;
        }
        public VoucherModel UpdateVoucher(VoucherModel model)
        {
            var data = new VoucherModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.Id);
                    param.Add("@VoucherCd", model.VoucherCd);
                    param.Add("@VoucherNm", model.VoucherNm);
                    param.Add("@VoucherDesc", model.VoucherDesc);
                    param.Add("@RemainingCount", model.RemainingCount);
                    param.Add("@StartDate", model.StartDate);
                    param.Add("@EndDate", model.EndDate);
                    param.Add("@UpdatedDate", DateTime.Now);
                    param.Add("@UpdatedBy",model.UpdatedBy);
                    param.Add("@IsActive", model.IsActive);
                    param.Add("@IsClaimed", model.IsClaimed);
                    param.Add("@IsUsed", model.IsUsed);
                    param.Add("@SentTo", model.SentTo);
                                                                          
                    data = conn.Query<VoucherModel>("sp_Tbl_Mst_VoucherUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

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
