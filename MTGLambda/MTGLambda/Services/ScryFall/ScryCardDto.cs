using Amazon.Lambda.Core;
using MTGLambda.MTGLambda.DataClass.MTGLambdaCard;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTGLambda.MTGLambda.Services.ScryFall
{
    public class ScryCardDto
    {
        public long arena_id { get; set; }
        public string artist { get; set; }
        public bool booster { get; set; }
        public string border_color { get; set; }
        public string card_back_id { get; set; }
        public List<CardFace> card_faces { get; set; }
        public string cmc { get; set; } //Used
        public string collector_number { get; set; }
        public List<string> color_identity { get; set; }
        public List<string> colors { get; set; }
        public bool digital { get; set; }
        public long edhrec_rank { get; set; }
        public string flavor_text { get; set; }
        public bool foil { get; set; }
        //i.e. "paper", "mtgo", "arena"
        public List<string> games { get; set; }
        public string id { get; set; }
        public Dictionary<string, string> image_uris { get; set; }
        public string layout { get; set; }
        public Dictionary<string, string> legalities { get; set; }
        public string loyalty { get; set; }
        //i.e. "{1}{U}" or "{X}{B}{U}"
        public string mana_cost { get; set; }
        public List<long> multiverse_ids { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "object")]
        public string object_type { get; set; }

        public string oracle_id { get; set; }
        public string oracle_text { get; set; }
        public string power { get; set; }
        public bool promo { get; set; }
        public string rarity { get; set; }
        public string rulings_uri { get; set; }
        public string scryfall_set_uri { get; set; }
        public string scryfall_uri { get; set; }
        public string set { get; set; }
        public string set_name { get; set; }
        public string set_type { get; set; }
        public string set_uri { get; set; }
        public long tcgplayer_id { get; set; }
        public string toughness { get; set; }
        public string type_line { get; set; }

        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }

        public Card Map(ScryCardDto import)
        {
            if (import != null)
                LambdaLogger.Log($"Entering: Map({ JsonConvert.SerializeObject(import) }");
            else
                LambdaLogger.Log($"Entering: CardDto Map import is null!");

            var responseCard = new Card();

            try
            {
                responseCard.Name = import.Name;
                responseCard.ColorIdentity = String.Join("", import.color_identity.Select(x => '{' + x + '}'));

                if (import.card_faces != null && import.card_faces.Count > 1)
                {
                    responseCard.IsDouble = true;
                    responseCard.CardText = import.card_faces[0].oracle_text;
                    responseCard.BackCardText = import.card_faces[1].oracle_text;

                    if (card_faces[0].image_uris != null)
                        responseCard.ImageUrl = import.card_faces[0].image_uris["normal"];
                    else if (import.image_uris != null)
                        responseCard.ImageUrl = import.image_uris["normal"];

                    if (card_faces[1].image_uris != null)
                        responseCard.BackCardImageUrl = import.card_faces[1].image_uris["normal"];

                    if (!string.IsNullOrWhiteSpace(import.card_faces[0].mana_cost))
                        responseCard.Colors = MapColors(import.card_faces[0].mana_cost);
                    else if (!string.IsNullOrWhiteSpace(import.card_faces[1].mana_cost))
                        responseCard.Colors = MapColors(import.card_faces[1].mana_cost);

                    if (!string.IsNullOrWhiteSpace(import.card_faces[0].mana_cost))
                        responseCard.Mana = import.card_faces[0].mana_cost;

                    if (import.card_faces[1] != null)
                        responseCard.Loyalty = import.card_faces[1].loyalty;

                    responseCard.Power = import.card_faces[0].power;
                    responseCard.Toughness = import.card_faces[0].toughness;
                }
                else
                {
                    responseCard.IsDouble = false;
                    responseCard.CardText = import.oracle_text;

                    if (!string.IsNullOrWhiteSpace(import.mana_cost))
                        responseCard.Colors = MapColors(import.mana_cost);

                    responseCard.Mana = import.mana_cost;

                    responseCard.Loyalty = import.loyalty;

                    responseCard.Power = import.power;
                    responseCard.Toughness = import.toughness;

                    responseCard.ImageUrl = (import.image_uris != null && import.image_uris.ContainsKey("normal")) ?
                                             import.image_uris["normal"] : string.Empty;
                }

                responseCard.Type = import.type_line;
                responseCard.BaseType = import.type_line.Split(" — ")[0];
                responseCard.ManaCost = string.IsNullOrWhiteSpace(import.cmc) ? 0 : float.Parse(import.cmc);

                responseCard.Number = import.collector_number;
                responseCard.Rarity = import.rarity;
                responseCard.Set = import.set;
                responseCard.SetName = import.set_name;
                responseCard.Id = import.id;
                responseCard.TCGProductId = import.tcgplayer_id;
            }
            catch (Exception exp)
            {
                LambdaLogger.Log($"Error while Map({ JsonConvert.SerializeObject(import) }) - Exception: { exp }");
            }

            LambdaLogger.Log($"Leaving: Map({ JsonConvert.SerializeObject(responseCard) })");

            return responseCard;
        }

        private Dictionary<string, int> MapColors(string manaCost)
        {
            var localColors = new Dictionary<string, int>()
            {
                { "Black", 0 },
                { "Blue", 0 },
                { "White", 0 },
                { "Green", 0 },
                { "Red", 0 },
            };

            foreach (var importColor in manaCost.Split(new char[] { '{', '}' }))
            {
                //LambdaLogger.Log($"importColor: { importColor }");

                switch (importColor)
                {
                    //Black
                    case ("B"):
                        localColors["Black"]++;
                        break;
                    //Blue
                    case ("U"):
                        localColors["Blue"]++;
                        break;
                    //White
                    case ("W"):
                        localColors["White"]++;
                        break;
                    //Green
                    case ("G"):
                        localColors["Green"]++;
                        break;
                    //Red
                    case ("R"):
                        localColors["Red"]++;
                        break;
                    //Colorless & Fuck it who needs it!
                    default:
                        break;
                }
            }

            return localColors;
        }
    }

    public class CardFace
    {
        [JsonProperty(PropertyName = "object")]
        public string object_type { get; set; }
        public string name { get; set; }
        public string mana_cost { get; set; }
        public string type_line { get; set; }
        public string oracle_text { get; set; }
        public List<string> colors { get; set; }
        public Dictionary<string, string> image_uris { get; set; }
        public string loyalty { get; set; }
        public string power { get; set; }
        public string toughness { get; set; }
    }
}
