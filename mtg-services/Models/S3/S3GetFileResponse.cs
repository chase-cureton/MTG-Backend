using mtg_services.Models.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace mtg_services.Models.S3
{
    public class S3GetFileResponse : Response
    {
        public string FilePath { get; set; }
        public string FileContent { get; set; }
    }
}
