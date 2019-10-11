using NReco.VideoConverter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Threading;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Web;
using Microsoft.Azure.WebJobs;

namespace Jingl.WatermarkVideos
{
    public class Program
    {

        public IDbConnection Connection
        {
            get
            {
                //return new SqlConnection("Server=tcp:sophielastic.database.windows.net,1433;Initial Catalog=JINGL_STG;Persist Security Info=False;User ID=sophielastic;Password=SophieHappy33;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=3600;");

                return new SqlConnection("Server=tcp:jinglprod.database.windows.net,1433;Initial Catalog=JINGPROD;Persist Security Info=False;User ID=jinglprod;Password=SophieHappy33;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=3600;");

            }
        }

        public static string StorageConn()
        {
            //return "DefaultEndpointsProtocol=https;AccountName=storagejingdev;AccountKey=YIFi0Prlo82TsPeY4I9pTpanpRfU596aJvJ/0p42GzonQXpb3aq0u6I2IQXtVtDqQIiHQp2BIhVkFxbmLpnjBA==;EndpointSuffix=core.windows.net";
            return "DefaultEndpointsProtocol=https;AccountName=storagejingle;AccountKey=wL5D9X0qdWLQu/Ov2V32ff9Q2NhCxAVqiMmEOK+oHVbkRk6+5HdqKkVtd86qq1RnArtX4oUb8urL7txCrxQd0g==;EndpointSuffix=core.windows.net";

        }

        public static void Main()
        {
            ProcessMethod().Wait();
        }

        static void WatermarkProcess()
        {
            try
            {
                Program program = new Program();

                var DestFolder = Directory.GetCurrentDirectory() + "/videos/temp/";
                var watermarkFolder = Directory.GetCurrentDirectory() + "/videos/watermark/";
                var ThumbnailsFolder = Directory.GetCurrentDirectory() + "/videos/thumb/";

                var ListFiles = program.GetFiles();

                if (ListFiles.Count > 0)
                {


                    foreach (var i in ListFiles)
                    {
                        Console.WriteLine("..........................................................................");
                        Console.WriteLine("Starting Watermark Process for " + i.Id);

                        FilesModel model = new FilesModel();
                        model.Id = i.Id;
                        model.Link = i.Link;
                        model.FileName = i.FileName;
                        model.FileType = i.FileType;

                        var FileData = i.FileName + i.FileType;

                        var filethumb = i.FileName + ".png";

                        var containerName = "videos";
                        string storageConnection = StorageConn();
                        CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(storageConnection);
                        CloudBlobClient blobClient = cloudStorageAccount.CreateCloudBlobClient();
                        CloudBlobContainer cloudBlobContainer = blobClient.GetContainerReference(containerName);
                        CloudBlockBlob blockBlob = cloudBlobContainer.GetBlockBlobReference(FileData);



                        MemoryStream memStream = new MemoryStream();

                        Console.WriteLine("starting download from " + i.Link + " to Temp Folder");
                        blockBlob.DownloadToStream(memStream);
                        Console.WriteLine("finish download ");

                        Console.WriteLine("..........................................................................");


                        SaveMemoryStream(memStream, Path.Combine(DestFolder, FileData));

                        Console.WriteLine("..........................................................................");

                        SetWatermark(Path.Combine(DestFolder, FileData), Path.Combine(watermarkFolder, FileData));
                        SetThumbnails(Path.Combine(DestFolder, FileData), Path.Combine(ThumbnailsFolder, filethumb));
                        Console.WriteLine("..........................................................................");
                        string Uri = UploadToAzure(Path.Combine(watermarkFolder, FileData), FileData,"watermark");
                        model.Link = Uri;
                        program.UpdateWaterMarkLink(model);

                        string Urithumb = UploadToAzure(Path.Combine(ThumbnailsFolder, filethumb), filethumb, "photos");
                        model.thumbnails = Urithumb;
                        program.UpdateThumbnailsLink(model);

                        Console.WriteLine("..........................................................................");


                        Console.WriteLine("Starting Delete File in Temp Folder");
                        if (File.Exists(Path.Combine(DestFolder, FileData)))
                        {
                            File.Delete(Path.Combine(DestFolder, FileData));
                        }

                        if (File.Exists(Path.Combine(ThumbnailsFolder, filethumb)))
                        {
                            File.Delete(Path.Combine(ThumbnailsFolder, filethumb));
                        }


                        if (File.Exists(Path.Combine(watermarkFolder, FileData)))
                        {
                            File.Delete(Path.Combine(watermarkFolder, FileData));
                        }

                        Console.WriteLine("Finish Delete File in Temp Folder");
                        Console.WriteLine("..........................................................................");


                        Console.WriteLine("Done Watermark Process for " + i.Id);
                        Console.WriteLine("..........................................................................");

                    }
                }
                else
                {
                    Console.Write(".");
                }

            }
            catch (Exception ex)
            {
                Program program = new Program();
                program.InputErrorLog("WatermarkService", ex.Message, "WatermarkProcess");
                throw ex;
            }



        }

        static void SetThumbnails(string PathSource, string PathDestination)
        {
            var ffMpeg = new NReco.VideoConverter.FFMpegConverter();

            ffMpeg.GetVideoThumbnail(PathSource, PathDestination, 1.0f);
        }
           

        [NoAutomaticTriggerAttribute]
        public static async Task ProcessMethod()
        {
            while (true)
            {
                try
                {
                    Program Program = new Program();
                   Program.WatermarkProcess();
                   // Program.SetThumbnails();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error occurred in processing pending  requests. Error : {0}", ex.Message); 
                   
                }
                await Task.Delay(TimeSpan.FromMinutes(10));
            }
        }



        public static void SetWatermark(string PathSource, string PathDestination)
        {
            Console.WriteLine("starting set watermark " + PathSource);
            var ImgFile = Directory.GetCurrentDirectory() + "/videos/img/inputImage.png";
            var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
            NReco.VideoConverter.FFMpegConverter wrap = new FFMpegConverter();
            wrap.Invoke("-i " + PathSource + " -i " + ImgFile + " -filter_complex \"overlay=main_w-overlay_w-5:5\" " + PathDestination + " ");
            Console.WriteLine("Finish set watermark " + PathSource);
        }

        public static string UploadToAzure(string FileSource, string filename,string reference)
        {
            Console.WriteLine("starting Upload to Azure  " + FileSource);
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(StorageConn());

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference(reference);

            var newBlob = container.GetBlockBlobReference(filename);
            newBlob.UploadFromFileAsync(FileSource).Wait();
            var blobUrl = newBlob.Uri.AbsoluteUri;

            Console.WriteLine("Finish Upload to Azure " + blobUrl);

            return blobUrl;
        }


        public static void SaveMemoryStream(MemoryStream ms, string FileName)
        {
            FileStream outStream = File.OpenWrite(FileName);
            ms.WriteTo(outStream);
            outStream.Flush();
            outStream.Close();
        }

        public IList<FilesModel> GetFiles()
        {
            Console.WriteLine("Get files processing ... ");
            var ListFiles = new List<FilesModel>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();

                ListFiles = conn.Query<FilesModel>("sp_GetFilesIsNotWatermark", param,
                           commandType: CommandType.StoredProcedure).ToList();



            }
            Console.WriteLine("Get files done ... ");
            return ListFiles;
        }

        public void UpdateThumbnailsLink(FilesModel model)
        {
            Console.WriteLine("Update thumbnails  For " + model.Id);
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();

                conn.Execute("update Tbl_Trx_Files set Thumbnails = @thumbnails where id = @Id ",
                    new
                    {
                        @thumbnails = model.thumbnails,                        
                        @Id = model.Id
                    }
                    );



            }
            Console.WriteLine("Update thumbnails For " + model.Id + " Done");


        }


        public void UpdateWaterMarkLink(FilesModel model)
        {
            Console.WriteLine("Update Iswatermark  For " + model.Id);
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();

                conn.Execute("update Tbl_Trx_Files set IsWatermark = @IsWatermark , link = @Link , fileName = @FileName, FileType = @FileType where id = @Id ",
                    new
                    {
                        @IsWatermark = 1,
                        @link = model.Link,
                        @FileName = model.FileName,
                        FileType = model.FileType,
                        @Id = model.Id
                    }
                    );



            }
            Console.WriteLine("Update Watermark For " + model.Id + " Done");


        }


        public void InputErrorLog(string ErrorSource, string message, string functionName)
        {
            var data = new ErrorLogModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@ErrorSource", ErrorSource);
                param.Add("@ErrorMessage", message);
                param.Add("@FunctionName", functionName);


                data = conn.Query<ErrorLogModel>("sp_Tbl_Error_LogInsert", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();

            }
        }

    }

    public class ErrorLogModel
    {
        public int Id { get; set; }

        public string ErrorSource { get; set; }

        public string ErrorMessage { get; set; }
        public string FunctionName { get; set; }

        public DateTime? CreatedDate { get; set; }
    }







    public class FilesModel
    {
        public int Id { get; set; }

        public string Link { get; set; }

        public string FileName { get; set; }
        public string thumbnails { get; set; }

        public string FileDesc { get; set; }

        public string FileType { get; set; }

        public int? FileCategory { get; set; }

        public int? ViewCount { get; set; }

        public TimeSpan? FileDuration { get; set; }

        public int? OwnerId { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool? IsWatermark { get; set; }

        public bool? IsActive { get; set; }
    }
}
