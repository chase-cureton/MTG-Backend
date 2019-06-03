using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.Lambda.Core;
using EnumExtensions;
using mtg_services.Models.MTG.MTGCard;
using mtg_services.Utilities.Enum;
using mtg_services.Utilities.UtilException;
using mtg_services.Utilities.Validation;
using Newtonsoft.Json;

namespace mtg_services.Services.MTG.MTGCard
{
    public class CreatureCardFactory : CardFactory
    {
        public override Card CreateCard(CreateCardRequest request)
        {
            LambdaLogger.Log($"Entering: CreateCard({JsonConvert.SerializeObject(request)}");
            CreatureCard response = null;

            try
            {
                var validationResult = ValidateRequest(request);

                if (!validationResult.IsValid)
                    throw new ValidationException(validationResult.GetErrors());

                long power = long.Parse(request.CardAttributes["Power"]);
                long toughness = long.Parse(request.CardAttributes["Toughness"]);

                //TODO: abstract common card properties
                string manaCost = request.CardAttributes["ManaCost"];
                string cardText = request.CardAttributes["CardText"];
                List<string> colors = request.CardAttributes["Colors"].Split(',').ToList();
                List<string> keywords = request.CardAttributes["Keywords"].Split(',').ToList();

                response = new CreatureCard(power, toughness)
                {
                    Name = request.CardName,
                    Power = power,
                    Toughness = toughness,
                    Type = CardEnum.Creature.GetDescription(),
                    ManaCost = manaCost,
                    CardText = cardText,
                    Colors = colors,
                    Keywords = keywords
                };
            }
            catch(ValidationException vExp)
            {
                LambdaLogger.Log($"Error: {vExp}");
                throw new Exception($"{ vExp.Message }");
            }
            catch(Exception exp)
            {
                LambdaLogger.Log($"Error: {exp}");
                throw new Exception("Error while creating card object.");
            }

            LambdaLogger.Log($"Leaving: CreateCard({JsonConvert.SerializeObject(response)}");
            return response;
        }

        private ValidationResponse ValidateRequest(CreateCardRequest request)
        {
            LambdaLogger.Log($"Entering: ValidateRequest({ JsonConvert.SerializeObject(request) })");

            ValidationResponse response = new ValidationResponse
            {
                IsValid = true,
                Errors = new List<string>()
            };

            try
            {
                if (request == null)
                {
                    response.IsValid = false;
                    response.AddError("CreateCardRequest is null...have you tried making it not null?");

                    return response;
                }

                if (string.IsNullOrWhiteSpace(request.CardName))
                {
                    response.IsValid = false;
                    response.AddError($"This card name is no good...look at this CardName: { request.CardName }");
                }

                if (string.IsNullOrWhiteSpace(request.CardType))
                {
                    response.IsValid = false;
                    response.AddError($"This card type is no good...look at this CardType: { request.CardType }");
                }

                if (request.CardAttributes == null)
                {
                    response.IsValid = false;
                    response.AddError($"These card attributes (CardAttributes) are no good...they're null.");

                    return response;
                }

                if (request.CardAttributes.Count <= 0)
                {
                    response.IsValid = false;
                    response.AddError($"There are no card attributes (CardAttributes) attached here, what are you thinking?");

                    return response;
                }

                if (!request.CardAttributes.ContainsKey("Power") &&
                    !request.CardAttributes.ContainsKey("power"))
                {
                    response.IsValid = false;
                    response.AddError($"There is no power in this request: { JsonConvert.SerializeObject(request) } - why not?");
                }

                if (!request.CardAttributes.ContainsKey("Toughness") &&
                    !request.CardAttributes.ContainsKey("toughness"))
                {
                    response.IsValid = false;
                    response.AddError($"There is no toughness in this request: { JsonConvert.SerializeObject(request) } - why not?");
                }
            }
            catch(Exception exp)
            {
                LambdaLogger.Log($"Error: {exp}");
                throw;
            }

            LambdaLogger.Log($"Leaving: ValidateRequest({ JsonConvert.SerializeObject(response) })");
            return response;
        }
    }
}
