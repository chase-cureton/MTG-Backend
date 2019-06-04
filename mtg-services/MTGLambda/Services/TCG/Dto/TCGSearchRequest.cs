using System;
using System.Collections.Generic;
using System.Text;

namespace MTGLambda.MTGLambda.Services.TCG.Dto
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
