using System;
using System.Collections.Generic;
using System.Text;

namespace MTGLambda.MTGLambda.Services.MTG.Dto
{
    public class KeywordDictionary
    {
        public Dictionary<string, PrimaryKeywordConstraint> Primary { get; set; }
        public Dictionary<string, SecondaryKeywordConstraint> Secondary { get; set; }
    }

    public class PrimaryKeywordConstraint
    {
        /// <summary>
        /// If contained, has keyword unless suffix or negate
        /// </summary>
        public List<string> Prefix { get; set; }

        /// <summary>
        /// If contained after listed prefix, has keyword unless suffix or negate
        /// </summary>
        public List<string> Target { get; set; }

        /// <summary>
        /// If contained after listed prefix and target (if exists), has keyword unless negate
        /// </summary>
        public List<string> Suffix { get; set; }

        /// <summary>
        /// If contained, does not count for keyword previously determined by Prefix/Suffix
        /// </summary>
        public List<string> Negate { get; set; }
    }

    public class SecondaryKeywordConstraint
    {
        /// <summary>
        /// If keywords are contained, add higher order keywords
        /// ex. Removal: Creature (contains all removals for creature - destroy, exile, bounce, etc.. for creature) 
        /// </summary>
        public List<string> Primary { get; set; }

        /// <summary>
        /// If primary higher order keywords are contained, add higher order keywords
        /// ex. Removal: Creature (contains all removals for creature - destroy, exile, bounce, etc.. for creature) 
        ///     Removal (contains all removals - Removal: Creature, Removal: Artifact, etc..)
        /// </summary>
        public List<string> Secondary { get; set; }
    }
}
