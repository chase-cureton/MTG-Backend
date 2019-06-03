using System;
using System.Collections.Generic;
using System.Text;

namespace mtg_services.Models.TCG
{
    public class TCGProductResponse
    {
        public bool success { get; set; }
        public List<string> errors { get; set; }
        public List<TCGProduct> results { get; set; }
    }
}
