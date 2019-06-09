using System;
using System.Collections.Generic;
using System.Text;

namespace MTGLambda.MTGLambda.Services.MTG.Constants
{
    public class MTGServiceConstants
    {
        //All => 0: Username 1: Filename
        public const string DeckFilepathTemplate = "Users/{0}/Decks/{1}";
        public const string SearchFilepathTemplate = "Users/{0}/SearchResults/{1}";

        public const string AuthenticateJsonFilepath = "Configuration/TCG-Access.json";
    }
}
