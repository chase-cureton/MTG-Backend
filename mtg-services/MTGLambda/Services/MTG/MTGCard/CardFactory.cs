using System.Collections.Generic;
using MTGLambda.MTGLambda.DataClass.MTGLambdaCard;

namespace MTGLambda.MTGLambda.Services.MTG.MTGCard
{
    public abstract class CardFactory
    {
        public abstract Card CreateCard(CreateCardRequest request);
    }

    public class CreateCardRequest
    {
        public string CardName { get; set; }
        public string CardType { get; set; }
        public Dictionary<string, string> CardAttributes { get; set; }

        public CreateCardRequest()
        {
            CardAttributes = new Dictionary<string, string>();
        }
    }
}
