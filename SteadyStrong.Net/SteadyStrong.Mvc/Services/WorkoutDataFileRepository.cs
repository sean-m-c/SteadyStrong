using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using SteadyStrong.Mvc.Data;
using SteadyStrong.Mvc.Models;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace SteadyStrong.Mvc.Services
{
    public class WorkoutDataFileRepository : IWorkoutDataFileRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly IFileStorageService _fileStorageService;


        public WorkoutDataFileRepository(
            ApplicationDbContext context,
            IConfiguration configuration,
            IFileStorageService fileStorageService
            )
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _context = context ?? throw new ArgumentNullException(nameof(configuration));
            _fileStorageService = fileStorageService ?? throw new ArgumentNullException(nameof(fileStorageService));
        }


        void IWorkoutDataFileRepository.Create(Workout workout)
        {
            if (workout == null) throw new ArgumentNullException(nameof(workout));

            XmlSerializer x = new XmlSerializer(typeof(Workout));

            var memoryStream = new MemoryStream();
            x.Serialize(memoryStream, workout);
            memoryStream.Seek(0, SeekOrigin.Begin);

            _fileStorageService.SaveBlob(memoryStream, $"workout_{workout.Id}.xml");
        }


       // string IWorkoutDataFileRepository.GetFileFor(Guid workoutId, string username)
       // {
       //     if(workoutId == null) return string.Empty;
       //     if (string.IsNullOrWhiteSpace(username)) return string.Empty;

       //     // Make sure the user has a workout in the database with this ID.
       //     if (!_context.WorkoutMetas.Any(w => w.Username.ToLowerInvariant() == username.ToLowerInvariant() && w.Id == workoutId)) {
       //         return string.Empty;
       //     }

       //     return this.DoGetFilePathFor(workoutId);
       //}


       // private string DoGetFilePathFor(Guid workoutId)
       // {
       //     return Path.Combine(_configuration["Data:WorkoutFolderPath"], $"{workoutId}.xml");
       // }
    }
}
