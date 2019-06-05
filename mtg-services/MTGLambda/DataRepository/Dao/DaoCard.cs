using MTGLambda.MTGLambda.DataClass.MTGLambdaCard;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTGLambda.MTGLambda.DataRepository.Dao
{
    public class DaoCard : Dao<Card>
    {
        public DaoCard(DaoContext daoContext) : base(daoContext) { }
    }
}
