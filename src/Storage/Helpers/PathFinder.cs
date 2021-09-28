using System;
using System.Text.RegularExpressions;
using Storage.Interfaces;

namespace Storage.Helpers
{
    public class PathFinder : IPathFinder
    {
        // TODO add path validation 
        private const string LinuxStyleRoot = "/(.*?)/(.*)";
        private readonly Regex _regex = new Regex(LinuxStyleRoot);

        public (string, string) SplitRoot(string path)
        {
            var groups = _regex.Match(path).Groups;
            return  (groups[1].ToString(), groups[2].ToString());
        }

        public string GetLinuxStylePath(string path)
        {
            return _regex.Match(path).Groups[2].ToString();
        }
        
        public (string, string, string) GetCloudStorageHierarchy(string path)
        {
            string filePath;
            string account;
            string container;
            (account, filePath) = SplitRoot(path);
            (container, filePath) = SplitRoot("/" + filePath);
            return (account, container, filePath);
        }

    }
}