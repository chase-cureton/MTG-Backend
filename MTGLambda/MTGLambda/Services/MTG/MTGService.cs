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
        /// Retrieve card by name from underlying DynamoDb table
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
