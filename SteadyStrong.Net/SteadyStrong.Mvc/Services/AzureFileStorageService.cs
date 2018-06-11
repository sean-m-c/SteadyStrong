using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;


namespace SteadyStrong.Mvc.Services
{
    public class AzureFileStorageService : IFileStorageService
    {

        private readonly IConfiguration _configuration;


        public AzureFileStorageService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }


        void IFileStorageService.SaveBlob(Stream fileStream, string blobName)
        {
            if (string.IsNullOrWhiteSpace(blobName)) throw new ArgumentNullException(nameof(blobName));

            CloudBlobContainer container = this.DoGetCloudBlobContainer();
            CloudBlockBlob blob = container.GetBlockBlobReference(blobName.ToLowerInvariant());

            blob.UploadFromStreamAsync(fileStream);
        }

        /// <summary>
        /// Finds the blob with the given name (must include extension, e.g "myblob.xml"). 
        /// </summary>
        /// <param name="name">The name of the blob (must include extension, e.g "myblob.xml").</param>
        void IFileStorageService.FindBlob(string name)
        {
            throw new NotImplementedException();
            //string filePath = "https://steadystrong.blob.core.windows.net/workouts/" + name;

            //CloudBlobContainer container = this.DoGetCloudBlobContainer();
            //CloudBlockBlob blockBlob = container.GetBlockBlobReference(filePath);

            //blockBlob.
            //var localPath = @"C:\Temp" + names[0];
            //await blockBlob.DownloadToFileAsync(localPath, FileMode.Create);
        }


        CloudBlobContainer IFileStorageService.GetCloudBlobContainer()
        {
            return this.DoGetCloudBlobContainer();
        }


        private CloudBlobContainer DoGetCloudBlobContainer()
        {
            string connectionString = _configuration.GetConnectionString("AzureStorageSteadyStrongConnectionString");

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("workouts");

            return container;
        }

    }
}