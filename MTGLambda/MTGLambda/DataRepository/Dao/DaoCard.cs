using MTGLambda.MTGLambda.DataClass.MTGLambdaCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTGLambda.MTGLambda.DataRepository.Dao
{
    public class DaoCard : Dao<Card>
    {
        public DaoCard(DaoContext daoContext) : base(daoContext) { }

        public Card FindByName(string name)
        {
            return FindAll(string.Format("Name = {0}", name)).FirstOrDefault();
        }

        public IEnumerable<Card> FindByColors(List<string> colors)
        {
            return FindAll(string.Format("Colors in ({0})", String.Join(',', colors)));
        }

        public IEnumerable<Card> FindByManaCost(string convertedManaCost)
        {
            return FindAll(string.Format("ManaCost = {0}", convertedManaCost));
        }
    }
}
