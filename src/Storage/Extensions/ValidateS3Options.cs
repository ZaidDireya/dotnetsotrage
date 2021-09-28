using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using Storage.Config;

namespace Storage.Extensions
{
    public static class ValidateS3Options
    {
        public static IEnumerable<S3Options> validate(this IEnumerable<S3Options> options)
        {
            foreach (var option in options)
            {
                if (!option.IsConfigurationValid())
                {
                    throw new Exception("missing S3 options");
                }

                yield return option;
            }
        }
    }
}