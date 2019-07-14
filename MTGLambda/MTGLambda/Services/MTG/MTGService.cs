using Amazon.Lambda.Core;
using MTGLambda.MTGLambda.DataClass.MTGLambdaCard;
using MTGLambda.MTGLambda.Services.Common;
using MTGLambda.MTGLambda.Services.MTG.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTGLambda.MTGLambda.Services.MTG
{
    public class MTGService : BaseService
    {
        //S3 Stored Approach
        public GetUserDecksResponse GetUserDecksS3(GetUserDecksRequest request)
        {
            LambdaLogger.Log($"Entering: GetUserDeck({ JsonConvert.SerializeObject(request) })");

            GetUserDecksResponse response = new GetUserDecksResponse
            {
                IsSuccess = true
            };

            try
            {
                //Get user decks from request.userId
                //S3 path: Users/Decks/* - 1 file per deck
            }
            catch(Exception exp)
            {
                LambdaLogger.Log($"Error: { exp }");

                response.IsSuccess = false;
                response.ErrorMessage = $"Error while retrieving decks for userId: { request.userId }";
            }

            LambdaLogger.Log($"Leaving: GetUserDeck({ JsonConvert.SerializeObject(response) })");

            return response;
        }

        public GetUserDecksResponse GetUserDecks(GetUserDecksRequest request)
        {
            LambdaLogger.Log($"Entering: GetUserDecks( { JsonConvert.SerializeObject(request) })");

            var response = new GetUserDecksResponse
            {
                IsSuccess = true
            };

            try
            {
                var userDecks = SvcContext.Repository.Decks.FindByUserId(request.userId);

                LambdaLogger.Log($"User Decks: { JsonConvert.SerializeObject(userDecks) }");

                response.decks = userDecks;
            }
            catch(Exception exp)
            {
                LambdaLogger.Log($"Error: { exp }");

                response.IsSuccess = false;
                response.ErrorMessage = "Error while getting user decks.";

                LambdaLogger.Log($"Leaving: GetUserDecks( { JsonConvert.SerializeObject(response) })");
                return response;
            }


            LambdaLogger.Log($"Leaving: GetUserDecks( { JsonConvert.SerializeObject(response) })");

            return response;
        }

        /// <summary>
        /// Get cards by searching name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Card> GetCardsFromName(string name)
        {
            LambdaLogger.Log($"Entering: GetCardFromName({name})");

            List<Card> responseCards = null;

            try
            {
                responseCards = SvcContext.Repository
                                          .Cards
                                          .FindFromName(name)
                                          .GroupBy(x => x.Name)
                                          .Select(x => x.First())
                                          .ToList();

                LambdaLogger.Log($"Retrieved Card: { JsonConvert.SerializeObject(responseCards) }");
            }
            catch(Exception exp)
            {
                LambdaLogger.Log($"Error: { exp }");
            }

            LambdaLogger.Log($"Leaving: GetCardFromName({ JsonConvert.SerializeObject(responseCards) })");
            return responseCards;
        }

        /// <summary>
        /// Gets cards by request
        /// - NameFilter: Searches against name
        /// - TextFilter: Searches against the card text
        /// - ColorFilter: Searches filters based on color
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<Card> GetCardsFromRequest(GetCardRequest request)
        {
            LambdaLogger.Log($"Entering: FindFromRequest({ JsonConvert.SerializeObject(request) })");

            var response = new List<Card>();

            try
            {
                if (!string.IsNullOrWhiteSpace(request.NameFilter))
                {
                    LambdaLogger.Log($"Name Filter: { request.NameFilter }");

                    if (request.ManaCostFilter != null && request.ColorFilter != null && request.BaseTypeFilter != null)
                    {
                        response = SvcContext.Repository
                                             .Cards
                                             .FindFromNameAndManaCostAndColorsAndBaseType(request)
                                             .ToList();
                    }
                    else if (request.ManaCostFilter != null && request.BaseTypeFilter != null)
                    {
                        response = SvcContext.Repository
                                             .Cards
                                             .FindFromNameAndManaCostAndBaseType(request)
                                             .ToList();
                    }
                    else if (request.ManaCostFilter != null && request.ColorFilter != null)
                    {
                        response = SvcContext.Repository
                                             .Cards
                                             .FindFromNameAndManaCostAndColors(request)
                                             .ToList();
                    }
                    else if (request.BaseTypeFilter != null && request.ColorFilter != null)
                    {
                        response = SvcContext.Repository
                                             .Cards
                                             .FindFromNameAndBaseTypeAndColors(request)
                                             .ToList();
                    }
                    else if (request.ManaCostFilter != null)
                    {
                        response = SvcContext.Repository
                                             .Cards
                                             .FindFromNameAndManaCost(request)
                                             .ToList();
                    }
                    else if (request.ColorFilter != null)
                    {
                        response = SvcContext.Repository
                                             .Cards
                                             .FindFromNameAndColors(request)
                                             .ToList();
                    }
                    else if (request.BaseTypeFilter != null)
                    {
                        response = SvcContext.Repository
                                             .Cards
                                             .FindFromNameAndBaseType(request)
                                             .ToList();
                    }
                    else
                    {
                        response = SvcContext.Repository
                                             .Cards
                                             .FindFromName(request.NameFilter)
                                             .ToList();
                    }
                }
                else if (!string.IsNullOrWhiteSpace(request.TextFilter))
                {
                    LambdaLogger.Log($"Text Filter: { request.TextFilter }");

                    if (request.ManaCostFilter != null && request.ColorFilter != null && request.BaseTypeFilter != null)
                    {
                        response = SvcContext.Repository
                                             .Cards
                                             .FindFromTextAndManaCostAndColorsAndBaseType(request)
                                             .ToList();
                    }
                    else if (request.ManaCostFilter != null && request.BaseTypeFilter != null)
                    {
                        response = SvcContext.Repository
                                             .Cards
                                             .FindFromTextAndManaCostAndBaseType(request)
                                             .ToList();
                    }
                    else if (request.ManaCostFilter != null && request.ColorFilter != null)
                    {
                        response = SvcContext.Repository
                                             .Cards
                                             .FindFromTextAndManaCostAndColors(request)
                                             .ToList();
                    }
                    else if (request.BaseTypeFilter != null && request.ColorFilter != null)
                    {
                        response = SvcContext.Repository
                                             .Cards
                                             .FindFromTextAndBaseTypeAndColors(request)
                                             .ToList();
                    }
                    else if (request.ManaCostFilter != null)
                    {
                        response = SvcContext.Repository
                                             .Cards
                                             .FindFromTextAndManaCost(request)
                                             .ToList();
                    }
                    else if (request.ColorFilter != null)
                    {
                        response = SvcContext.Repository
                                             .Cards
                                             .FindFromTextAndColors(request)
                                             .ToList();
                    }
                    else if (request.BaseTypeFilter != null)
                    {
                        response = SvcContext.Repository
                                             .Cards
                                             .FindFromTextAndBaseType(request)
                                             .ToList();
                    }
                    else
                    {
                        response = SvcContext.Repository
                                             .Cards
                                             .FindFromText(request.TextFilter)
                                             .ToList();
                    }
                }
                else if (!string.IsNullOrWhiteSpace(request.KeywordsFilter))
                {
                    throw new NotImplementedException("Keyword filter not supported yet");
                }
                else
                {
                    LambdaLogger.Log($"No search bar content: { JsonConvert.SerializeObject(request) }");

                    if (request.ManaCostFilter != null && request.ColorFilter != null && request.BaseTypeFilter != null)
                    {
                        response = SvcContext.Repository
                                             .Cards
                                             .FindFromManaCostAndBaseTypeAndColors(request)
                                             .ToList();
                    }
                    else if (request.ManaCostFilter != null && request.ColorFilter != null)
                    {
                        response = SvcContext.Repository
                                             .Cards
                                             .FindFromManaCostAndColors(request)
                                             .ToList();
                    }
                    else if (request.ManaCostFilter != null && request.BaseTypeFilter != null)
                    {
                        response = SvcContext.Repository
                                             .Cards
                                             .FindFromManaCostAndBaseType(request)
                                             .ToList();
                    }
                    else if (request.BaseTypeFilter != null && request.ColorFilter != null)
                    {
                        response = SvcContext.Repository
                                             .Cards
                                             .FindFromBaseTypeAndColors(request)
                                             .ToList();
                    }
                    else if (request.ManaCostFilter != null)
                    {
                        response = SvcContext.Repository
                                             .Cards
                                             .FindFromManaCost(request)
                                             .ToList();
                    }
                    else if (request.BaseTypeFilter != null)
                    {
                        response = SvcContext.Repository
                                             .Cards
                                             .FindFromBaseType(request)
                                             .ToList();
                    }
                    else if (request.ColorFilter != null)
                    {
                        response = SvcContext.Repository
                                             .Cards
                                             .FindFromColors(request)
                                             .ToList();
                    }
                }
            }
            catch (Exception exp)
            {
                LambdaLogger.Log($"Error: { exp }");
                throw;
            }

            LambdaLogger.Log($"Leaving: FindFromRequest({ JsonConvert.SerializeObject(response) })");

            return response.Where(c => !string.IsNullOrWhiteSpace(c.ImageUrl))
                           .GroupBy(c => c.CardText)
                           .Select(g => g.First())
                           .ToList();
        }

        public void SaveCard(Card card)
        {
            LambdaLogger.Log($"Entering: SaveCard({ JsonConvert.SerializeObject(card) })");

            try
            {
                var cardList = new List<Card>
                {
                    card
                };

                SvcContext.Repository
                          .Cards
                          .Save(cardList);
            }
            catch (Exception exp)
            {
                LambdaLogger.Log($"Error: { exp }");
            }

            LambdaLogger.Log($"Leaving: SaveCard({ JsonConvert.SerializeObject(card) })");
        }

        public void SaveUserDeck()
        {
            LambdaLogger.Log($"Entering: SaveUserDeck({ JsonConvert.SerializeObject(string.Empty) })");

            LambdaLogger.Log($"Leaving: SaveUserDeck({ JsonConvert.SerializeObject(string.Empty) })");
        }
    }
}
