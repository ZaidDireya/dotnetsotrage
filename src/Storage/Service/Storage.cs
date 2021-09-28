namespace Storage.Service
{
    public class Storage
    {
        private StorageFactory _storageFactory;
        
        public Storage(StorageFactory storageFactory)
        {
            _storageFactory = storageFactory;
        }
    }
}