using System;
using System.Collections.Generic;
using System.Text;

namespace MTGLambda.MTGLambda.Services.TCG.Dto
{
    public class TCGSearchResponse
    {
        public long totalItems { get; set; }
        public bool success { get; set; }
        public List<string> errors { get; set; }
        public List<long> results { get; set; }
    }
}
