using System;
using System.Collections.Generic;
using System.Text;

namespace mtg_services.Models.TCG
{
    public class TCGProduct
    {
        public long productId { get; set; }
        public string name { get; set; }
        public string cleanName { get; set; }
        public string imageUrl { get; set; }
        public long categoryId { get; set; }
        public long groupId { get; set; }
        public string url { get; set; }
        public string modifiedOn { get; set; }
        public long imageCount { get; set; }
        public PresaleInfo presaleInfo { get; set; }
        public List<TCGProductExtendedData> extendedData { get; set; }
    }

    public class PresaleInfo
    {
        public bool isPresale { get; set; }
        public string releasedOn { get; set; }
        public string note { get; set; }
    }

    public class TCGProductExtendedData
    {
        public string name { get; set; }
        public string displayName { get; set; }
        public string value { get; set; }
    }
}
