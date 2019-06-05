using MTGLambda.MTGLambda.DataClass.MTGLambdaCard;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTGLambda.MTGLambda.DataClass.MTGLambdaDeck
{
    public class Deck
    {
        //Object-identifiable
        public long id { get; set; }
        public string Name { get; set; }

        //User-identifiable
        public long UserId { get; set; }

        //Content
        public List<Card> DeckList { get; set; }
        public DeckTemplate Template { get; set; }

        public Deck()
        {
            DeckList = new List<Card>();
        }
    }

    public class DeckOverviewDto
    {
        public long id { get; set; }
        public string deckName { get; set; }
        public long instants { get; set; }
        public long sorceries { get; set; }
        public long enchantments { get; set; }
        public long creatures { get; set; }
        public long artifacts { get; set; }
        public long planeswalkers { get; set; }
        public long lands { get; set; }
    }
}
