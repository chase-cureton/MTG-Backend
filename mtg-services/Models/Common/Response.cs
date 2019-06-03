using System;
using System.Collections.Generic;
using System.Text;

namespace mtg_services.Models.Common
{
    public abstract class Response
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }
}
