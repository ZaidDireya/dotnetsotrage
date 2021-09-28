using System;
using System.Collections.Generic;
using Storage.Config;

namespace Storage.Extensions
{
    public static class AzureAccounts
    {
        public static IEnumerable<AccountOptions> Validate(this IEnumerable<AccountOptions> accounts)
        {
            foreach (var account in accounts)
            {
                Console.Write("in first validation");
                if (!account.IsConfigurationValid()) 
                    throw new Exception("missing azure accounts");
                yield return account;
            }
        }
    }
}