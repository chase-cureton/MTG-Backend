using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.Core;
using MTGLambda.MTGLambda.DataClass.MTGLambdaCard;
using MTGLambda.MTGLambda.Helpers.DynamoDb;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTGLambda.MTGLambda.DataRepository.Dao
{
    public class DaoCard : Dao<Card>
    {
        public DaoCard(DaoContext daoContext) : base(daoContext) { }

        public Card FindByName(string name, string manaCost)
        {
            LambdaLogger.Log($"Entering: FindByName({name})");

            //var cards = FindAll(string.Format("Name = {0}", name)).FirstOrDefault();
            var card = DynamoDbHelper.Load<Card>(name, manaCost);

            LambdaLogger.Log($"Leaving: FindByName({ JsonConvert.SerializeObject(card) })");

            return card;
        }

        public IEnumerable<Card> FindByColors(List<string> colors)
        {
            return FindAll(string.Format("Colors in ({0})", String.Join(',', colors)));
        }

        public IEnumerable<Card> FindByManaCost(string convertedManaCost)
        {
            return FindAll(string.Format("ManaCost = {0}", convertedManaCost));
        }

        public IEnumerable<Card> FindFromName(string name)
        {
            var conditions = new List<ScanCondition>
            {
                new ScanCondition("Name", ScanOperator.Equal, name)
            };

            return FindAll(conditions);
        }
    }
}
