using System.Collections.Generic;

namespace Storage.Config
{
    public class AzureOptions
    {
        public List<AccountOptions> Accounts{ get; set; }
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        public bool IsConfigurationValid()
        {
            return !string.IsNullOrEmpty(TenantId) && !string.IsNullOrEmpty(ClientId) &&
                   !string.IsNullOrEmpty(ClientSecret);
        }
        
    }
}