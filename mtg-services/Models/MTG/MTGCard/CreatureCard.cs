using EnumExtensions;
using mtg_services.Utilities.Enum;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace mtg_services.Models.MTG.MTGCard
{
    public class CreatureCard : Card
    {
        public long Power { get; set; }
        public long Toughness { get; set; }

        public CreatureCard()
        {
            Colors = new List<string>();
            Keywords = new List<string>();
        }

        public CreatureCard(long power, long toughness)
        {
            Colors = new List<string>();
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
