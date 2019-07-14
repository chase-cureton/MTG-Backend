using Amazon.Lambda.Core;
using MtgApiManager.Lib.Service;
using MTGLambda.MTGLambda.DataClass.MTGLambdaCard;
using MTGLambda.MTGLambda.Helpers.S3;
using MTGLambda.MTGLambda.Helpers.S3.Dto;
using MTGLambda.MTGLambda.Services.Common;
using MTGLambda.MTGLambda.Services.MagicIO.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTGLambda.MTGLambda.Services.MTG
{
    public class ImportService : BaseService
    {
        /// <summary>
        /// Deprecated - DONUT USE (will be repurposed to grab from API to S3)
        /// - Really just used to grab card info initially
        /// - Then will read from S3 to db
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        public void ImportFromAPI(int page, int pageSize)
        {
            CardService service = new CardService();

            var pageResults = service.Where(x => x.Page, page)
                                     .Where(x => x.PageSize, pageSize)
                                     .All();

            if (pageResults.IsSuccess)
            {
                LambdaLogger.Log($"Page Results success my guy!");

                string importString = JsonConvert.SerializeObject(pageResults.Value);
                var importCards = JsonConvert.DeserializeObject<List<ImportCardDto>>(importString);

                var file = S3Helper.CreateFile(new S3CreateFileRequest
                {
                    Content = JsonConvert.SerializeObject(importCards),
                    FilePath = $"Imports/CardImports/{page}.json"
                }).Result;

                var cardList = new List<Card>();

                importCards.ForEach(x => cardList.Add(x.Map(x)));

                LambdaLogger.Log($"Card List: { JsonConvert.SerializeObject(cardList) }");

                //For some reason the list that I'm mapping to isn't working to add, so I'm gonna loop through it
                foreach (var card in cardList)
                {
                    SvcContext.Repository
                              .Cards
                              .Save(new List<Card>() { card });
                }
            }
            else
            {
                LambdaLogger.Log($"Page Results Error");
            }

            //LambdaLogger.Log($"Page Results: { JsonConvert.SerializeObject(pageResults.Value.GetRange(1, 10)) }");
        }

        public void ImportFromAPI(int pageStart, int pageEnd, int pageSize)
        {
            LambdaLogger.Log($"Entering: ImportCards({pageStart}, { pageEnd }, {pageSize})");

            try
            {
                CardService service = new CardService();

                for (int i = pageStart; i < pageEnd; i++)
                {
                    ImportFromAPI(i, pageSize);
                }
            }
            catch (Exception exp)
            {
                LambdaLogger.Log($"Error: { exp }");
                throw;
            }

            LambdaLogger.Log($"Leaving: ImportCards({pageStart}, { pageEnd }, {pageSize})");
        }

        /// <summary>
        /// Imports cards from S3 jsons
        /// - Imports for a single page
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        public void ImportCards(int page, int pageSize)
        {
            LambdaLogger.Log($"Entering: ImportCards({page}, {pageSize})");

            try
            {
                //Retrieving import entries
                var importCardList = new List<ImportCardDto>();

                var ioCards = JsonConvert.DeserializeObject<List<ImportCardDto>>(S3Helper.GetFile(new S3GetFileRequest
                {
                    FilePath = $"Imports/CardImports/{page}.json"
                }).Result.FileContent);

                importCardList.AddRange(ioCards);

                //Building dao entries
                var cardList = new List<Card>();

                LambdaLogger.Log($"Saving Card List: { JsonConvert.SerializeObject(importCardList) }");

                int i = 0;
                foreach (var ioCard in importCardList)
                {
                    if (ioCard != null)
                    {
                        var newCard = ioCard.Map(ioCard);
                        var floatIdString = $"{page}.{i}";

                        newCard.FloatId = float.Parse(floatIdString);

                        SvcContext.Repository
                                  .Cards
                                  .Save(new List<Card>() { newCard });
                    }

                    i++;
                }
            }
            catch(Exception exp)
            {
                LambdaLogger.Log($"Error: { exp }");
                throw;
            }

            LambdaLogger.Log($"Leaving: ImportCards({page}, {pageSize})");
        }

        /// <summary>
        /// Imports cards from S3 jsons
        /// - Imports from range of pages
        /// Use: 1 - 5, 6 - 10 (range of page imports to avoid timeout issues)
        /// </summary>
        /// <param name="pageStart"></param>
        /// <param name="pageEnd"></param>
        /// <param name="pageSize"></param>
        public void ImportCards(int pageStart, int pageEnd, int pageSize)
        {
            LambdaLogger.Log($"Entering: ImportCards({pageStart}, { pageEnd }, {pageSize})");

            try
            {
                CardService service = new CardService();

                for (int i = pageStart; i < pageEnd; i++)
                {
                    ImportCards(i, pageSize);
                }
            }
            catch (Exception exp)
            {
                LambdaLogger.Log($"Error: { exp }");
                throw;
            }

            LambdaLogger.Log($"Leaving: ImportCards({pageStart}, { pageEnd }, {pageSize})");
        }
    }
}
