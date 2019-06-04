using System;
using System.Collections.Generic;
using System.Text;

namespace MTGLambda.MTGLambda.Helpers.S3.Dto
{
    public class S3CreateFileRequest
    {
        public string FilePath { get; set; }
        public string Content { get; set; }
    }
}
