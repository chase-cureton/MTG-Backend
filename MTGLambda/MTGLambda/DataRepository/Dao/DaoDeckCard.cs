using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using MTGLambda.MTGLambda.DataClass.MTGLambdaCard;
using MTGLambda.MTGLambda.DataClass.MTGLambdaDeck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTGLambda.MTGLambda.DataRepository.Dao
{
    public class DaoDeckCard : Dao<DeckCard>
    {
        public DaoDeckCard(DaoContext daoContext) : base(daoContext) { }

        public IEnumerable<DeckCard> FindFromUser(string username)
        {
            var conditions = new List<ScanCondition>
            {
                new ScanCondition("UserName", ScanOperator.Equal, username)
            };

            return FindAll(conditions);
        }

        public IEnumerable<DeckCard> FindFromDeckName(string deckName)
        {
            var conditions = new List<ScanCondition>
            {
                new ScanCondition("DeckName", ScanOperator.Equal, deckName)
            };

            return FindAll(conditions);
        }

        public IEnumerable<DeckCard> FindFromUserAndDeckName(string username, string deckName)
        {
            var conditions = new List<ScanCondition>
            {
                new ScanCondition("DeckName", ScanOperator.Equal, deckName),
                new ScanCondition("UserName", ScanOperator.Equal, username)
            };

            return FindAll(conditions);
        }
    }
}