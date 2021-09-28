
using System;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using Amazon;
using Amazon.S3;
using Azure.Identity;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using Storage.Config;
using Storage.Extensions;
using Storage.Helpers;
using Storage.Interfaces;
using Storage.Service;
namespace Storage
{
    public static class StorageConfiguration
    {
        public static void SetupStorage(this IServiceCollection services, StorageOptions options)
        {
            // var regionEndpoint = Amazon.RegionEndpoint.GetBySystemName(region);

            if (options == null) throw new Exception("cant find options");

            services.AddSingleton<IPathFinder, PathFinder>();
            
            var s3Options = options.S3 ?? Enumerable.Empty<S3Options>();
            var amazonS3Clients = s3Options.validate().ToDictionary(s3Option => s3Option.Region,
                s3Option => new AmazonS3Client(s3Option.AccessKey, s3Option.SecretKey,
                    RegionEndpoint.GetBySystemName(s3Option.Region)));
            
            var azureOptions = options.AzureStorage;
            azureOptions.Validate();

            var client = new ClientSecretCredential(azureOptions.TenantId, azureOptions.ClientId, azureOptions.ClientSecret);
            var azureBlobServiceClients = azureOptions.Accounts.Validate().ToDictionary(account => account.AccountName,
                account => new BlobServiceClient(new Uri(account.AccountUrl), client));
            
            services.AddSingleton(amazonS3Clients);
            services.AddSingleton(azureBlobServiceClients);
            services.AddSingleton<IStorage, AzureService>();
            services.AddSingleton<IStorage, AmazonService>();
            services.AddSingleton<StorageFactory>();
        }
    }
}
