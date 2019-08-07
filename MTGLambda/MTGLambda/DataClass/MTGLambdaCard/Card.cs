using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.Core;
using MTGLambda.MTGLambda.Helpers.Common;
using MTGLambda.MTGLambda.Services.MagicIO.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MTGLambda.MTGLambda.DataClass.MTGLambdaCard
{
    [DynamoDBTable("Card")]
    public class Card
    {
        [DynamoDBHashKey] //ToQuery: BaseType = (no comparison)
        public virtual string BaseType { get; set; }
        [DynamoDBRangeKey] //ToSort: Id >, <, contains (I think..)
        public string Id { get; set; }

        [DynamoDBProperty]
        public virtual string Name { get; set; }
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
        
        public Card()
        {
            Colors = new Dictionary<string, int>();
        }

        public virtual string DescribeOverview()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
