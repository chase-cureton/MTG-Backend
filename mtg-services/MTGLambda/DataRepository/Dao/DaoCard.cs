using MTGLambda.MTGLambda.DataClass.MTGLambdaCard;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTGLambda.MTGLambda.DataRepository.Dao
{
    public class DaoCard : Dao<Card>
    {
        public DaoCard(DaoContext daoContext) : base(daoContext) { }

        public IEnumerable<Card> FindByColors(List<string> colors)
        {
            return FindAll(string.Format("Colors in ({0})", String.Join(',', colors)));
        }

        public IEnumerable<Card> FindByCMC(string convertedManaCost)
        {
            return FindAll(string.Format("CMC = {0}", convertedManaCost));
        }
    }
}
