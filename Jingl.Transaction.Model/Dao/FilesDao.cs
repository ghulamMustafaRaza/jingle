using Dapper;
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
    public class FilesDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public FilesDao(IConfiguration config)
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


        public FilesModel CreateFiles(FilesModel model)
        {
            var data = new FilesModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Link", model.Link);
                    param.Add("@FileName", model.FileName);
                    param.Add("@FileDesc", model.FileDesc);
                    param.Add("@FileType", model.FileType);
                    param.Add("@FileCategory", model.FileCategory);
                    param.Add("@ViewCount", model.ViewCount);
                    param.Add("@FileDuration", model.FileDuration);
                    param.Add("@OwnerId", model.OwnerId);
                    param.Add("@CreatedBy", model.CreatedBy);
                    param.Add("@CreatedDate", DateTime.Now.AddHours(7));
                  


                    data = conn.Query<FilesModel>("sp_Tbl_Trx_FilesInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }


        public FilesWatchModel CreateFilesWatch(FilesWatchModel model)
        {
            var data = new FilesWatchModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@UserId", model.UserId);
                    param.Add("@FileId", model.FileId);                  

                    data = conn.Query<FilesWatchModel>("sp_Tbl_Trx_Files_WatchInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }


        public RatingModel CreateRatingFiles(RatingModel model)
        {
            var data = new RatingModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@UserId", model.UserId);
                    param.Add("@FileId", model.FileId);
                    param.Add("@Rate", model.Rate);                  
                    param.Add("@CreatedBy", model.CreatedBy);
                    param.Add("@CreatedDate", DateTime.Now.AddHours(7));



                    data = conn.Query<RatingModel>("sp_Tbl_Trx_Files_RatingInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public IList<TalentVideoModel> GetAllVideoByCategory(int? CategoryId)
        {
            var data = new List<TalentVideoModel>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@CategoryId", CategoryId);              


                data = conn.Query<TalentVideoModel>("Sp_GetVideoByCategory", param,
                           commandType: CommandType.StoredProcedure).ToList();



            }

            return data;
        }





    }
}
