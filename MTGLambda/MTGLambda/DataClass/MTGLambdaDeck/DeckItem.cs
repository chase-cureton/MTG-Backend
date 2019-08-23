using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTGLambda.MTGLambda.DataClass.MTGLambdaDeck
{
    [DynamoDBTable("Deck")]
    public class DeckCard
    {
        [DynamoDBHashKey]
        public string DeckName { get; set; }
        [DynamoDBRangeKey]
        public string Name { get; set; }
        [DynamoDBProperty]
        public string UserName { get; set; }

        [DynamoDBProperty]
        public virtual float? ManaCost { get; set; }
        [DynamoDBProperty]
        public virtual Dictionary<string, int> Colors { get; set; }
        [DynamoDBProperty]
        public virtual string CardText { get; set; }
        [DynamoDBProperty]
        public virtual string Type { get; set; }
        [DynamoDBProperty]
        public string Tags { get; set; }
        [DynamoDBProperty]
        public virtual string Power { get; set; }
        [DynamoDBProperty]
        public virtual string Toughness { get; set; }
        [DynamoDBProperty]
        public virtual string ImageUrl { get; set; }
        [DynamoDBProperty]
        public string Rarity { get; set; }
        [DynamoDBProperty]
        public string Set { get; set; }
        [DynamoDBProperty]
        public string SetName { get; set; }
        [DynamoDBProperty]
        public string Number { get; set; }
        [DynamoDBProperty]
        public float? FloatId { get; set; }
        [DynamoDBProperty]
        public virtual string ColorIdentity { get; set; }


        [DynamoDBProperty]
        public float TCGMarketPrice { get; set; }
        [DynamoDBProperty]
        public float TCGMarketPrice_Foil { get; set; }
        [DynamoDBProperty]
        public long TCGProductId { get; set; }
        [DynamoDBProperty]
        public long TCGGroupId { get; set; }

        //Newly ADDED!!!
        [DynamoDBProperty]
        public bool IsDouble { get; set; }
        [DynamoDBProperty]
        public string BackCardText { get; set; }
        [DynamoDBProperty]
        public string Loyalty { get; set; }
        [DynamoDBProperty]
        public string BackCardImageUrl { get; set; }
        [DynamoDBProperty]
        public string Mana { get; set; }
    }

    [DynamoDBTable("Deck")]
    public class DeckStats
    {
        [DynamoDBHashKey]
        public string DeckName { get; set; }
        [DynamoDBRangeKey]
        public string Name { get; set; }
        [DynamoDBProperty]
        public string UserName { get; set; }

        [DynamoDBProperty]
        public string DeckMetricsJson { get; set; }
        [DynamoDBProperty]
        public string CommanderName { get; set; }
    }
}
