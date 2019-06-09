using MTGLambda.MTGLambda.DataClass.MTGLambdaDeck;
using MTGLambda.MTGLambda.Helpers.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTGLambda.MTGLambda.Services.MTG.Dto
{
    public class GetUserDecksResponse : Response
    {
        public List<Deck> decks { get; set; }

        public GetUserDecksResponse()
        {
            decks = new List<Deck>();
        }
    }
}
