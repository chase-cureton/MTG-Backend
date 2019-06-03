using System;
using System.Collections.Generic;
using System.Text;

namespace mtg_services.Models.TCG
{
    public class TCGSearchRequest
    {
        public string sort { get; set; }
        public long limit { get; set; }
        public long offset { get; set; }
        public List<TCGSearchFilter> filters { get; set; } 
    }

    public class TCGSearchFilter
    {
        public string name { get; set; }
        public List<string> values { get; set; }
    }
}
