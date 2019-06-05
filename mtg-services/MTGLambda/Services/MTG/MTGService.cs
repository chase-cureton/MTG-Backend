using Amazon.Lambda.Core;
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
            }


            LambdaLogger.Log($"Leaving: GetUserDecks( { JsonConvert.SerializeObject(response) })");

            return response;
        }

        public void SaveUserDeck()
        {
            LambdaLogger.Log($"Entering: SaveUserDeck({ JsonConvert.SerializeObject(string.Empty) })");

            LambdaLogger.Log($"Leaving: SaveUserDeck({ JsonConvert.SerializeObject(string.Empty) })");
        }
    }
}
