using Amazon.Lambda.Core;
using MTGLambda.MTGLambda.DataClass.MTGLambdaDeck;
using MTGLambda.MTGLambda.DataRepository.Interface;
using MTGLambda.MTGLambda.Helpers.S3;
using MTGLambda.MTGLambda.Helpers.S3.Dto;
using MTGLambda.MTGLambda.Helpers.Validation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTGLambda.MTGLambda.DataRepository
{
    public class DeckRepository : IDeckRepository
    {
        public void CreateDeck(string userId, Deck dto)
        {
            string fileContent = JsonConvert.SerializeObject(dto);

            LambdaLogger.Log($"File Content: { JsonConvert.SerializeObject(dto) }");

            var request = new S3CreateFileRequest()
            {
                FilePath = string.Format("Users/{0}/Decks/{1}.json", userId, dto.id),
                Content = fileContent
            };

            var response = S3Helper.CreateFile(request).Result;
        }

        public void DeleteDeck(string userId, Deck dto)
        {
            throw new NotImplementedException();
        }

        public Deck GetUserDeck(string userId, string deckName)
        {
            throw new NotImplementedException();
        }

        public List<Deck> GetUserDecks(string userId)
        {
            throw new NotImplementedException();
        }

        public void UpdateDeck(string userId, Deck dto)
        {
            throw new NotImplementedException();
        }
    }
}
