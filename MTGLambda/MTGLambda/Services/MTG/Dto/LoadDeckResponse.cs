using MTGLambda.MTGLambda.DataClass.MTGLambdaDeck;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTGLambda.MTGLambda.Services.MTG.Dto
{
    public class LoadDeckResponse
    {
        public string DeckName { get; set; }
        public string CommanderName { get; set; }
        public string UserName { get; set; }

        public List<DeckCard> DeckCards { get; set; }
        public List<DeckMetric> DeckMetrics { get; set; }

        public LoadDeckResponse()
        {
            DeckCards = new List<DeckCard>();
            DeckMetrics = new List<DeckMetric>();
        }
    }
}
