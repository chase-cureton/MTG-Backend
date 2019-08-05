using Amazon.Lambda.Core;
using MtgApiManager.Lib.Service;
using MTGLambda.MTGLambda.DataClass.MTGLambdaCard;
using MTGLambda.MTGLambda.Helpers.S3;
using MTGLambda.MTGLambda.Helpers.S3.Dto;
using MTGLambda.MTGLambda.Services.Common;
using MTGLambda.MTGLambda.Services.MagicIO.Dto;
using MTGLambda.MTGLambda.Services.MTG.Dto;
using MTGLambda.MTGLambda.Services.TCG;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTGLambda.MTGLambda.Services.MTG
{
    public class ImportService : BaseService
    {
        public KeywordDictionary Dictionary { get; set; }

        public ImportService()
        {
            Dictionary = new KeywordDictionary();

            var s3Response = S3Helper.GetFile(new S3GetFileRequest()
            {

            }).Result;

            if (s3Response.IsSuccess)
                Dictionary = JsonConvert.DeserializeObject<KeywordDictionary>(s3Response.FileContent);
        }

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

                var tcgService = ServiceFactory.GetService<TCGService>();

                int i = 0;
                foreach (var ioCard in importCardList.Where(x => x.Border == null))
                {
                    if (ioCard != null)
                    {
                        var newCard = ioCard.Map(ioCard);
                        var floatIdString = $"{page}.{i}";

                        newCard.FloatId = float.Parse(floatIdString);

                        tcgService.AddTCGDetails(newCard);

                        newCard.Keywords = TranslateCardText(newCard.CardText);

                        cardList.Add(newCard);
                    }

                    i++;
                }

                tcgService.AddTCGPrice(cardList);

                SvcContext.Repository
                          .Cards
                          .Save(cardList);
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

        /// <summary>
        /// Translates card text into keywords using KeywordDictionary
        /// </summary>
        /// <param name="cardText"></param>
        /// <returns></returns>
        private List<string> TranslateCardText(string cardText)
        {
            List<string> response = new List<string>();

            try
            { 
                //1.) Associate primary keywords
                //    - Keywords related directly to Card Text
                foreach(var primaryKeyPair in Dictionary.Primary)
                {
                    var keyword = primaryKeyPair.Key;
                    var prefix = primaryKeyPair.Value.Prefix;
                    var suffix = primaryKeyPair.Value.Suffix;
                    var negate = primaryKeyPair.Value.Negate;

                    var case1 = (prefix != null && prefix.Count > 0) &&
                                (suffix != null && suffix.Count > 0) &&
                                (negate != null && negate.Count > 0);

                    var case2 = (prefix != null && prefix.Count > 0) &&
                                (suffix != null && suffix.Count > 0);

                    var case3 = (prefix != null && prefix.Count > 0) &&
                                (negate != null && negate.Count > 0);

                    var case4 = (prefix != null && prefix.Count > 0);

                    //TODO: Need to break card text up into sections based on \n
                    //      - Accounts for things like separate abilities on planeswalkers
                    //      - Ex. Sphinx's Tutelage - 1st part is not card draw, but a Trigger: Card Draw & 2nd part is card draw
                    //      - Translate should add keywords: "On:Card Draw" (Secondary), "On:Card Draw => Action:Mill" (Primary), "Card Draw" (Secondary), "Card Draw: Sustained" (Primary)

                    if (case1) //Contains prefix, suffix and negate
                    {
                        UpdateSuffixKeywords(cardText, response, keyword, prefix, suffix);
                        UpdateNegateKeywords(cardText, response, keyword, negate);
                    }
                    else if (case2) //Contains prefix and suffix
                    {
                        UpdateSuffixKeywords(cardText, response, keyword, prefix, suffix);
                    }
                    else if (case3) //Contains prefix and negate
                    {
                        UpdatePrefixKeywords(cardText, response, keyword, prefix);
                        UpdateNegateKeywords(cardText, response, keyword, negate);
                    }
                    else if(case4) //Contains prefix
                    {
                        UpdatePrefixKeywords(cardText, response, keyword, prefix);
                    }
                    else //Constraint doesn't contain information correctly, log
                    {
                        LambdaLogger.Log($"Keyword Dictionary incorrect for keyword => { primaryKeyPair.Key }");
                    }
                }

                //2.) Associate secondary keywords
                //    - Keywords related to other keywords
                foreach(var secondaryKeyPair in Dictionary.Secondary.OrderByDescending(x => x.Value.Primary.Count))
                {
                    var keyword = secondaryKeyPair.Key;

                    var primaryKeywords = secondaryKeyPair.Value.Primary;
                    var secondaryKeywords = secondaryKeyPair.Value.Secondary;

                    if (primaryKeywords != null && primaryKeywords.Count > 0)
                    {
                        foreach(var primaryKeyword in primaryKeywords)
                        {
                            if (response.Contains(primaryKeyword))
                            {
                                if (!response.Contains(keyword))
                                    response.Add(keyword);

                                break;
                            }
                        }
                    }
                    else if (secondaryKeywords != null && secondaryKeywords.Count > 0)
                    {
                        foreach(var secondaryKeyword in secondaryKeywords)
                        {
                            if (response.Contains(secondaryKeyword))
                            {
                                if (!response.Contains(keyword))
                                    response.Add(keyword);

                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                LambdaLogger.Log($"TranslateCardText() Error: { exp }");
            }

            return response;
        }

        private static void UpdatePrefixKeywords(string cardText, List<string> response, string keyword, List<string> prefix)
        {
            foreach (var prefixCondition in prefix)
            {
                if (cardText.Contains(prefixCondition))
                {
                    if (!response.Contains(keyword))
                        response.Add(keyword);

                    break;
                }
            }
        }

        private static void UpdateSuffixKeywords(string cardText, List<string> response, string keyword, List<string> prefix, List<string> suffix)
        {
            bool containsPrefix = false;
            string shortCardText = string.Empty;

            foreach(var prefixCondition in prefix)
            {
                if (cardText.Contains(prefixCondition))
                {
                    //Chop off everything up to and including prefix from cardText
                    int index = cardText.IndexOf(prefixCondition);
                    shortCardText = cardText.Remove(0, index + prefixCondition.Length);

                    containsPrefix = true;
                    break;
                }
            }

            if (containsPrefix)
            {
                foreach(var suffixCondition in suffix)
                {
                    if (shortCardText.Contains(suffixCondition))
                    {
                        if (!response.Contains(keyword))
                            response.Add(keyword);

                        break;
                    }
                }
            }
        }

        private static void UpdateNegateKeywords(string cardText, List<string> response, string keyword, List<string> negate)
        {
            foreach(var negateCondition in negate)
            {
                if (cardText.Contains(negateCondition))
                {
                    if (response.Contains(keyword))
                        response.Remove(keyword);

                    break;
                }
            }
        }
    }
}
