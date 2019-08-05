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

    public class TCGProductPriceResponse
    {
        public bool success { get; set; }
        public List<string> errors { get; set; }
        public List<TCGProductPriceDto> results { get; set; }
    }

    public class TCGProductPriceDto
    {
        public long productId { get; set; }
        public float lowPrice { get; set; }
        public float midPrice { get; set; }
        public float highPrice { get; set; }
        public float marketPrice { get; set; }
        public float directLowPrice { get; set; }
        public string subTypeName { get; set; }
    }
}
