using System;
using System.Collections.Generic;
using System.Text;

namespace mtg_services.Utilities.Constants
{
    public class MTGServiceConstants
    {
        //All => 0: Username 1: Filename
        public const string DECK_FILEPATH_TEMPLATE = "Users/{0}/Decks/{1}";
        public const string SEARCH_FILEPATH_TEMPLATE = "Users/{0}/SearchResults/{1}";

        public const string AUTHENTICATE_JSON_FILEPATH = "Configuration/TCG-Access.json";
    }
}
