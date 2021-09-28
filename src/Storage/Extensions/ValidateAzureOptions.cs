using System;
using System.Collections.Generic;
using Storage.Config;

namespace Storage.Extensions
{
    public static class ValidateAzureOptions
    {
        public static AzureOptions Validate(this AzureOptions options)
        {
            if (!options.IsConfigurationValid())
                throw new Exception("missing azure options");
            return options;
        }
    }
}