using mtg_services.Models.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace mtg_services.Models.S3
{
    public class S3CreateFileResponse : Response
    {
        public string S3FilePath { get; set; }
    }
}
