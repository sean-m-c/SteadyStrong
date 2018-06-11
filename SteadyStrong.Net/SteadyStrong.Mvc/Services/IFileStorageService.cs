using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;

namespace SteadyStrong.Mvc.Services
{
    public interface IFileStorageService
    {
        CloudBlobContainer GetCloudBlobContainer();
        void FindBlob(string name);
        void SaveBlob(Stream fileStream, string name);
    }
}
