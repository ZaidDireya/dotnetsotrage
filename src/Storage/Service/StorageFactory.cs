using System;
using System.Collections.Generic;
using System.Linq;
using Storage.Interfaces;

namespace Storage.Service
{
    public class StorageFactory
    {
        private IEnumerable<IStorage> _storages;

        public StorageFactory(IEnumerable<IStorage> storages) => _storages = storages; 

        public IStorage Create(string type)
        {
            var storageService = _storages.FirstOrDefault(service => string.Equals(service.StorageType(), type, StringComparison.CurrentCultureIgnoreCase));
            if (storageService == null)
            {
                throw new NotImplementedException(type);
            }

            return storageService;
        }
    }
}