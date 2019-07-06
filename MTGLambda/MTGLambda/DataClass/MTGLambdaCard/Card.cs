﻿using Amazon.DynamoDBv2.DataModel;
using MTGLambda.MTGLambda.Helpers.Common;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MTGLambda.MTGLambda.DataClass.MTGLambdaCard
{
    [DynamoDBTable("Card")]
    public class Card
    {
        [DynamoDBProperty]
        public virtual string Name { get; set; }
        [DynamoDBRangeKey]
        public virtual int ManaCost { get; set; }
        [DynamoDBProperty]
        public virtual Dictionary<string, long> Colors { get; set; }
        [DynamoDBProperty]
        public virtual string CardText { get; set; }
        [DynamoDBHashKey]
        public virtual string Type { get; set; }
        [DynamoDBProperty]
        public virtual List<string> Keywords { get; set; }
        [DynamoDBProperty]
        public virtual int Power { get; set; }
        [DynamoDBProperty]
        public virtual int Toughness { get; set; }
        
        public Card()
        {
            Colors = new Dictionary<string, long>();
            Keywords = new List<string>();
        }

        public virtual string DescribeOverview()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
