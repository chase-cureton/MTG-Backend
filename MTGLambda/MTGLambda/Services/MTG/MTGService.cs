﻿using Amazon.Lambda.Core;
using AutoMapper;
using MTGLambda.MTGLambda.DataClass.MTGLambdaCard;
using MTGLambda.MTGLambda.DataClass.MTGLambdaDeck;
using MTGLambda.MTGLambda.Services.Common;
using MTGLambda.MTGLambda.Services.MTG.Constants;
using MTGLambda.MTGLambda.Services.MTG.Dto;
using MTGLambda.MTGLambda.Services.TCG;
using MTGLambda.MTGLambda.Services.TCG.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTGLambda.MTGLambda.Services.MTG
{
    public class MTGService : BaseService
    {
        //S3 Stored Approach

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
                    response = SvcContext.Repository
                                         .Cards
                                         .FindFromKeywordAndManaCostAndColorsAndBaseType(request)
                                         .ToList();
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

            return response;

            //if (request.IncludePrice)
            //{
            //    var tcgService = ServiceFactory.GetService<TCGService>();

            //    List<TCGSearchFilter> filters = new List<TCGSearchFilter>();
            //    filters.Add(new TCGSearchFilter() { name = "ProductName", values = cardList.Select(x => x.Name).ToList() });

            //    TCGSearchRequest tcgRequest = new TCGSearchRequest
            //    {
            //        filters = filters
            //    };

            //    var tcgResponse = tcgService.Search(tcgRequest).Result;

            //    if (tcgResponse.success)
            //    {
            //        LambdaLogger.Log($"TCG Response Results: { JsonConvert.SerializeObject(tcgResponse) }");
            //    }
            //    else
            //        LambdaLogger.Log($"Request Failure for TCG Search: { JsonConvert.SerializeObject(tcgRequest) }");
            //}
        }

        /// <summary>
        /// Saves deck for a user
        /// </summary>
        public async Task SaveUserDeck(SaveDeckRequest request)
        {
            LambdaLogger.Log($"Entering: SaveUserDeck({ JsonConvert.SerializeObject(request) })");

            try
            {
                DeckStats deckStats = new DeckStats
                {
                    CommanderName = request.CommanderName,
                    DeckName = request.DeckName,
                    Name = MTGServiceConstants.DeckStatsCardName,
                    UserName = request.UserName,
                    DeckMetricsJson = JsonConvert.SerializeObject(request.DeckMetrics)
                };

                var existingDeckStats = SvcContext.Repository
                                                  .DeckStats
                                                  .FindFromUserAndDeckName(request.UserName, request.DeckName);

                if (existingDeckStats != null)
                {
                    LambdaLogger.Log($"About to delete old deck stats: { request.DeckName }");

                    await SvcContext.Repository
                                    .DeckStats
                                    .Delete(existingDeckStats);
                }

                var existingDeckCards = SvcContext.Repository
                                                  .DeckCards
                                                  .FindFromUserAndDeckName(request.UserName, request.DeckName);

                if (existingDeckCards.Any())
                {
                    LambdaLogger.Log($"About to delete old deck cards: { request.DeckName }");

                    await SvcContext.Repository
                                    .DeckCards
                                    .Delete(existingDeckCards);
                }

                LambdaLogger.Log($"About to save deck stats: { JsonConvert.SerializeObject(deckStats) }");

                await SvcContext.Repository
                                .DeckStats
                                .SaveAsync(new List<DeckStats> { deckStats });

                LambdaLogger.Log($"About to save deck cards for user: { request.UserName } & commander: { request.CommanderName }");

                var config = new MapperConfiguration(x =>
                {
                    x.CreateMap<Card, DeckCard>();
                });

                var mapper = config.CreateMapper();

                var cards = SvcContext.Repository
                                      .Cards
                                      .FindFromNames(request.DeckCards.Select(x => x.Name).ToList())
                                      .ToList();

                var deckCards = new List<DeckCard>();

                foreach (var card in cards)
                {

                    if (card != null)
                    {
                        LambdaLogger.Log($"Found card for saved deck card: { JsonConvert.SerializeObject(card) }");

                        var newDeckCard = mapper.Map<Card, DeckCard>(card);

                        newDeckCard.DeckName = request.DeckName;
                        newDeckCard.UserName = request.UserName;

                        deckCards.Add(newDeckCard);
                    }
                }

                LambdaLogger.Log($"About to save cards: { JsonConvert.SerializeObject(deckCards) }");

                await SvcContext.Repository
                                .DeckCards
                                .SaveAsync(deckCards);
            }
            catch(Exception exp)
            {
                LambdaLogger.Log($"Error: { exp }");
                throw;
            }

            LambdaLogger.Log($"Leaving: SaveUserDeck({ JsonConvert.SerializeObject(request) })");
        }

        /// <summary>
        /// Loads deck fora a user
        /// </summary>
        /// <param name="card"></param>
        public LoadDeckResponse LoadUserDeck(LoadDeckRequest request)
        {
            LambdaLogger.Log($"Entering: LoadUserDeck({ JsonConvert.SerializeObject(request) })");

            var response = new LoadDeckResponse()
            {
                DeckName = request.DeckName,
                UserName = request.UserName
            };

            try
            {
                var deckStats = SvcContext.Repository
                                          .DeckStats
                                          .FindFromUserAndDeckName(request.UserName, request.DeckName);

                if (deckStats.Any())
                {
                    var deckStat = deckStats.FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(deckStat.DeckMetricsJson))
                    {
                        var deckMetrics = JsonConvert.DeserializeObject<List<DeckMetric>>(deckStat.DeckMetricsJson);

                        response.DeckMetrics = deckMetrics;
                    }

                    if (!string.IsNullOrWhiteSpace(deckStat.CommanderName))
                    {
                        response.CommanderName = deckStat.CommanderName;
                    }
                }

                var deckCards = SvcContext.Repository
                                          .DeckCards
                                          .FindFromUserAndDeckName(request.UserName, request.DeckName)
                                          .ToList();

                if (deckCards.Any())
                {
                    response.DeckCards = deckCards;
                }
            }
            catch(Exception exp)
            {
                LambdaLogger.Log($"Error: { exp }");
                throw;
            }

            LambdaLogger.Log($"Leaving: LoadUserDeck({ JsonConvert.SerializeObject(response) })");

            return response;
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
    }
}
