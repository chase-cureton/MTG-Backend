using MTGLambda.MTGLambda.Helpers.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTGLambda.MTGLambda.Helpers.S3.Dto
{
    public class S3CreateFileResponse : Response
    {
        public string S3FilePath { get; set; }
    }
}
