﻿using System;
using System.Collections.Generic;
using System.Text;

namespace mtg_services.Utilities.Constants
{
    public class TCGConstants
    {
        public const string BASE_URL = "https://api.tcgplayer.com";
        public const string SEARCH_URL = "https://api.tcgplayer.com/v1.27.0/catalog/categories/1/search";
        public const string GET_PRODUCT_URL_TEMPLATE = "https://api.tcgplayer.com/v1.27.0/catalog/products/{0}?getExtendedFields=true";

        public const string AUTH_STRING = "grant_type=client_credentials&client_id={0}&client_secret={1}";
    }
}
