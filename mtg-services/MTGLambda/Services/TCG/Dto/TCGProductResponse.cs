using System;
using System.Collections.Generic;
using System.Text;

namespace MTGLambda.MTGLambda.Services.TCG.Dto
{
    public class TCGProductResponse
    {
        public bool success { get; set; }
        public List<string> errors { get; set; }
        public List<TCGProduct> results { get; set; }
    }
}
