using System;
using System.Collections.Generic;
using System.Text;

namespace mtg_services.Models.S3
{
    public class S3CreateFileRequest
    {
        public string FilePath { get; set; }
        public string Content { get; set; }
    }
}
