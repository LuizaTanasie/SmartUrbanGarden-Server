using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Sg_functions.Helpers
{
    public static class StringHelper
    {
        public static string MaskEmailAddress(string email) {
            string pattern = @"(?<=[\w]{1})[\w-\._\+%]*(?=[\w]{2}@)";
            return Regex.Replace(email, pattern, m => new string('*',1));
        }
    }
}
