using Amazon.DynamoDBv2.DataModel;
using EnumExtensions;
using MTGLambda.MTGLambda.Helpers.Enum;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MTGLambda.MTGLambda.DataClass.MTGLambdaCard
{
    [DynamoDBTable("Card")]
    public class CreatureCard : Card
    {
        [DynamoDBProperty]
        public long Power { get; set; }
        [DynamoDBProperty]
        public long Toughness { get; set; }

        public CreatureCard()
        {
            Colors = new Dictionary<string, int>();
            Keywords = new List<string>();
        }

        public CreatureCard(long power, long toughness)
        {
            Colors = new Dictionary<string, int>();
            Keywords = new List<string>();

            Power = power;
            Toughness = toughness;
            Type = CardEnum.Creature.GetDescription();
        }

        public override string DescribeOverview()
        {
            return JsonConvert.SerializeObject(this);
        } 
    }
}
