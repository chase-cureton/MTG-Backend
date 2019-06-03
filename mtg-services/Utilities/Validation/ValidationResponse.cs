using System;
using System.Collections.Generic;
using System.Text;

namespace mtg_services.Utilities.Validation
{
    public class ValidationResponse
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; }

        public void AddError(string error)
        {
            Errors.Add(error);
        }

        public string GetErrors()
        {
            return string.Join(" | ", Errors);
        }
    }
}
