using System;
using System.Collections.Generic;
using System.Text;

namespace MTGLambda.MTGLambda.Services.ScryFall
{
    public class ScryCardImportRequest
    {
        public bool import_all { get; set; }
        public bool import_prices { get; set; }
        public int card_start { get; set; }
        public int card_end { get; set; }
        public List<string> names { get; set; }

        public ScryCardImportRequest()
        {
            names = new List<string>();
        }
    }
}
