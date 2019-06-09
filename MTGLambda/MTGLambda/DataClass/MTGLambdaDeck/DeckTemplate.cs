using System;
using System.Collections.Generic;
using System.Text;

namespace MTGLambda.MTGLambda.DataClass.MTGLambdaDeck
{
    //TODO: Make abstract class
    // - Extend with custom DeckTemplates (do something clever for this)
    // - If this is user defined, this maybe is better as a json
    public class DeckTemplate
    {
        //# of Lands, Creatures, etc..
        public long TargetLands { get; set; }
        public long TargetCreatures { get; set; }
        public long TargetEnchantments { get; set; }
        public long TargetArtifacts { get; set; }
        public long TargetPlaneswalkers { get; set; }
        public long TargetInstants { get; set; }
        public long TargetSorceries { get; set; }
    }
}
