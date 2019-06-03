using System;
using System.Collections.Generic;
using System.Text;

namespace mtg_services.Models.MTG.Deck
{
    public class DeckTemplate
    {
        public long TargetLands { get; set; }
        public long TargetCreatures { get; set; }
        public long TargetEnchantments { get; set; }
        public long TargetArtifacts { get; set; }
        public long TargetPlaneswalkers { get; set; }
        public long TargetInstants { get; set; }
        public long TargetSorceries { get; set; }
    }
}
