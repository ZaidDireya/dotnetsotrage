using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using Storage.DTO;
using Storage.Interfaces;

namespace Storage.Service
{
    public class AzureService : IStorage
    {
        private readonly Dictionary<string, BlobServiceClient> _blobServiceClients;
        private readonly IPathFinder _pathfinder;
        public AzureService(Dictionary<string, BlobServiceClient> blobClients, IPathFinder pathfinder)
        {
            _blobServiceClients = blobClients;
            _pathfinder = pathfinder;
        }

        public async Task SaveAsync(MemoryStream file, string contentType, string path)
        {
            var blobClient = GetBlobClient(path); 
            
            file.Position = 0;
            await blobClient.UploadAsync(file, true);
        }

        public async Task<StorageFile> ReadAsync(string path)
        {
            var blobClient = GetBlobClient(path);
            var memoryStream = new MemoryStream();
            var blobResponse = await blobClient.DownloadToAsync(memoryStream);
            var download = await blobClient.DownloadAsync();
            memoryStream.Seek(0, SeekOrigin.Begin);
            return new StorageFile()
            {
                ContentType = blobResponse.Headers.ContentType,
                FileContentStream = memoryStream
            };
        }

        public async Task<Uri> GetSecureUrlAsync(string path, int minutesToExpire)
        {
            var blobClient = GetBlobClient(path);
            var blobContainerClient = blobClient.GetParentBlobContainerClient();
            var blobServiceClient = blobContainerClient.GetParentBlobServiceClient();
            
            var utcNow = DateTimeOffset.UtcNow;
            var utcExpiresOn = utcNow.AddMinutes(minutesToExpire);
            var delegationKey = blobServiceClient.GetUserDelegationKeyAsync(utcNow, utcExpiresOn).Result;

            var blobSasBuilder = new BlobSasBuilder
            {
                ExpiresOn = utcExpiresOn,
                Resource = "b",
                BlobContainerName = blobClient.BlobContainerName,
                BlobName = blobClient.Name 
            };
            
            blobSasBuilder.SetPermissions(BlobSasPermissions.Read);
            
            blobSasBuilder.SetPermissions(BlobSasPermissions.Read);

            var blobUriBuilder = new BlobUriBuilder(blobClient.Uri)
            {
                Sas = blobSasBuilder.ToSasQueryParameters(delegationKey, blobServiceClient.AccountName)
            };

            return blobUriBuilder.ToUri();
        }

        public Uri GetSecureUrl(string path, int minutesToExpire)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(string path)
        {
            var blobClient = GetBlobClient(path);
            await blobClient.DeleteAsync();
        }

        private BlobClient GetBlobClient(string path)
        {
            var (account, container, filePath) = _pathfinder.GetCloudStorageHierarchy(path);
            _blobServiceClients.TryGetValue(account, out var blobServiceClient);
            
            if (blobServiceClient == null)
                throw new Exception("account not configured");
            
            var blobClient = blobServiceClient.GetBlobContainerClient(container).GetBlobClient(filePath);
            return blobClient;
        }

        public string StorageType()
        {
            return "AZURE";
        }
    }
}