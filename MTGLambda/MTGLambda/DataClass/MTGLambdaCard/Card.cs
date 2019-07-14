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
        [DynamoDBHashKey]
        public virtual string BaseType { get; set; }
        [DynamoDBProperty]
        public virtual List<string> Keywords { get; set; }
        [DynamoDBProperty]
        public virtual string Power { get; set; }
        [DynamoDBProperty]
        public virtual string Toughness { get; set; }
        [DynamoDBProperty]
        public virtual string ImageUrl { get; set; }
        [DynamoDBRangeKey]
        public string Id { get; set; }
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
        
        public Card()
        {
            Colors = new Dictionary<string, int>();
            Keywords = new List<string>();
        }

        public virtual string DescribeOverview()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
