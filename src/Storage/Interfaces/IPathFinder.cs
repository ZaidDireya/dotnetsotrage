using System;

namespace Storage.Interfaces
{
    public interface IPathFinder
    {
        (string, string) SplitRoot(string path);
        string GetLinuxStylePath(string path);
        (string, string, string) GetCloudStorageHierarchy(string path);
    }
}