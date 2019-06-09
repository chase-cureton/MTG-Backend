using MTGLambda.MTGLambda.DataClass.MTGLambdaDeck;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTGLambda.MTGLambda.DataRepository.Interface
{
    public interface IDeckRepository
    {
        Deck GetUserDeck(string userId, string deckName);
        List<Deck> GetUserDecks(string userId);

        void CreateDeck(string userId, Deck dto);
        void UpdateDeck(string userId, Deck dto);
        void DeleteDeck(string userId, Deck dto);
    }
}
