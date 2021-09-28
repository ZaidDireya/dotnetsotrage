using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Storage.DTO;
using Storage.Interfaces;

namespace Storage.Service
{
    public class AmazonService : IStorage
    {
        private readonly Dictionary<string, AmazonS3Client> _amazonS3Clients;
        private readonly IPathFinder _pathFinder;
        
        public AmazonService(Dictionary<string, AmazonS3Client> amazonS3Clients, IPathFinder pathFinder)
        {
            _amazonS3Clients = amazonS3Clients;
            _pathFinder = pathFinder;
        }

        public async Task SaveAsync(MemoryStream file, string contentType, string path)
        {
            var (regionName, bucketName, fileName) = _pathFinder.GetCloudStorageHierarchy(path);
            var amazonS3Client = getClient(regionName);
            
            await amazonS3Client.PutObjectAsync(new PutObjectRequest
            {
                BucketName = bucketName,
                Key = fileName,
                InputStream = file,
                ContentType = contentType,
            });
        }

        public async Task<StorageFile> ReadAsync(string path)
        {
            var (regionName, bucketName, fileName) = _pathFinder.GetCloudStorageHierarchy(path);
            var amazonS3Client = getClient(regionName);
            var amazonFile = await amazonS3Client.GetObjectAsync(bucketName, fileName);
            return new StorageFile()
            {
                FileContentStream = amazonFile.ResponseStream,
                ContentType = amazonFile.Headers.ContentType
            };
        }

        public Task<Uri> GetSecureUrlAsync(string path, int minutesToExpire)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(string path)
        {
            var (regionName, bucketName, fileName) = _pathFinder.GetCloudStorageHierarchy(path);
            var amazonS3Client = getClient(regionName);
            await amazonS3Client.DeleteObjectAsync(bucketName, fileName);
        }

        public Uri GetSecureUrl(string path, int minutesToExpire)
        {
            var (regionName, bucketName, fileName) = _pathFinder.GetCloudStorageHierarchy(path);
            var amazonS3Client = getClient(regionName);
            var request = new GetPreSignedUrlRequest()
            {
               BucketName = bucketName,
               Key = fileName,
               Expires = DateTime.UtcNow.AddDays(minutesToExpire)
            };
            return new Uri(amazonS3Client.GetPreSignedURL(request));
        }

        private AmazonS3Client getClient(string regionName)
        {
            _amazonS3Clients.TryGetValue(regionName, out var amazonS3Client);
            
            if(amazonS3Client == null)
                throw new Exception("region not configured");
            return amazonS3Client;
        }

        public string StorageType()
        {
            return "S3";
        }
    }
}