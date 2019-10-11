using Dapper;
using Jingl.General.Enum;
using Jingl.General.Model.Admin.Transaction;
using Jingl.General.Utility;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Jingl.Transaction.Model.Dao
{
    public class TalentVideoDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public TalentVideoDao(IConfiguration config)
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

        public IList<TalentVideoModel> GetTalentVideos(int TalentId)
        {
            var data = new List<TalentVideoModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@TalentId", TalentId);


                    data = conn.Query<TalentVideoModel>("Sp_GetTalentVideos", param,
                               commandType: CommandType.StoredProcedure).ToList();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public void CreateTalentVideo(TalentVideoModel model)
        {
            var data = new List<TalentVideoModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@TalentId", model.TalentId);
                    param.Add("@FileId", model.FileId);
                    param.Add("@VideoNm", model.VideoNm);
                    param.Add("@IsActive", (int)Status.Active);
                    param.Add("@BookCategory", model.BookCategory);


                    data = conn.Query<TalentVideoModel>("sp_Tbl_Trx_TalentVideoInsert", param,
                               commandType: CommandType.StoredProcedure).ToList();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            
        }

        public void DeleteTalentVideoByTalentId(int TalentId)
        {
            var data = new List<TalentVideoModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@TalentId", TalentId);


                    data = conn.Query<TalentVideoModel>("Sp_DeleteTalentVideoByTalentId", param,
                               commandType: CommandType.StoredProcedure).ToList();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }

        }

        public void DeleteTalentVideo(int Id)
        {
            var data = new List<TalentVideoModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", Id);


                    data = conn.Query<TalentVideoModel>("sp_Tbl_Trx_TalentVideoDelete", param,
                               commandType: CommandType.StoredProcedure).ToList();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }

        }

        public IList<TalentVideoModel> GetAllVideo()
        {
            var data = new List<TalentVideoModel>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
          


                data = conn.Query<TalentVideoModel>("Sp_GetAllVideo", param,
                           commandType: CommandType.StoredProcedure).ToList();
            }

            return data;
        }


    }
}
