namespace Storage.Config
{
    public class AccountOptions
    {
        public string AccountUrl { get; set; }
        public string AccountName { get; set; }

        public bool IsConfigurationValid()
        {
            return !string.IsNullOrEmpty(AccountUrl) && !string.IsNullOrEmpty(AccountName);
        }
    }
}