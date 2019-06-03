using System;
using System.Collections.Generic;
using System.Text;

namespace mtg_services.Models.MTG.Deck
{
    public class Deck
    {
        //Object-identifiable
        public long Id { get; set; }
        public string Name { get; set; }

        //User-identifiable
        public long UserId { get; set; }

        //Content
        //TODO: Change to card object
        public List<string> DeckList { get; set; }
        public DeckTemplate Template { get; set; }
    }

    public class DeckOverviewDto
    {
        public long deckId { get; set; }
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
