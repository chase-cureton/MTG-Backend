using Amazon.Lambda.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTGLambda.MTGLambda.Services.MTG
{
    public class MTGService
    {
        public void GetUserDecks()
        {

        }

        public void SaveUserDeck()
        {
            LambdaLogger.Log($"Entering: SaveUserDeck({ JsonConvert.SerializeObject(string.Empty) })");

            LambdaLogger.Log($"Leaving: SaveUserDeck()");
        }
    }
}
