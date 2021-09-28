using System;
using System.IO;
using System.Threading.Tasks;
using Storage.DTO;

namespace Storage.Interfaces
{
    public interface IStorage
    {
        Task SaveAsync(MemoryStream file, string contentType, string path);
        Task<StorageFile> ReadAsync(string path);
        Task<Uri> GetSecureUrlAsync(string path, int minutesToExpire);
        Uri GetSecureUrl(string path, int minutesToExpire);
        Task DeleteAsync(string path);
        string StorageType(); 
    }
}