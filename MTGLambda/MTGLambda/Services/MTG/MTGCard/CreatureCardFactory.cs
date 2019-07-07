using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Amazon.Lambda.Core;
using EnumExtensions;
using MTGLambda.MTGLambda.DataClass.MTGLambdaCard;
using MTGLambda.MTGLambda.Helpers.Enum;
using MTGLambda.MTGLambda.Helpers.Validation;
using Newtonsoft.Json;

namespace MTGLambda.MTGLambda.Services.MTG.MTGCard
{
    public class CreatureCardFactory : CardFactory
    {
        /// <summary>
        /// Generate Card model from CreateCardRequest()
        /// - Name
        /// - Type (creature, instant, artifact, etc..)
        /// - Card attributes dictionary [lazy gen. way of supporting different card types]
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public override Card CreateCard(CreateCardRequest request)
        {
            LambdaLogger.Log($"Entering: CreateCard({JsonConvert.SerializeObject(request)}");
            Card response = null;

            try
            {
                var validationResult = ValidateRequest(request);

                if (!validationResult.IsValid)
                    throw new ValidationException(validationResult.GetErrors());

                long power = long.Parse(request.CardAttributes["Power"]);
                long toughness = long.Parse(request.CardAttributes["Toughness"]);

                //TODO: abstract common card properties
                int manaCost = int.TryParse(request.CardAttributes["ManaCost"], out manaCost) ? manaCost : 0;
                string cardText = request.CardAttributes["CardText"];

                //List<string> colors = request.CardAttributes["Colors"]
                //                             .Split(',')
                //                             .ToList(); 

                Dictionary<string, int> colors = JsonConvert.DeserializeObject<Dictionary<string, int>>(request.CardAttributes["Colors"]);
                                                  

                List<string> keywords = request.CardAttributes["Keywords"]
                                               .Split(',')
                                               .ToList();

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
