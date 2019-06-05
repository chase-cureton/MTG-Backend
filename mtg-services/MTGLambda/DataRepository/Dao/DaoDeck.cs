using MTGLambda.MTGLambda.DataClass.MTGLambdaCard;
using MTGLambda.MTGLambda.DataClass.MTGLambdaDeck;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTGLambda.MTGLambda.DataRepository.Dao
{
    public class DaoDeck : Dao<Deck>
    {
        public DaoDeck(DaoContext daoContext) : base(daoContext) { }
    }
}