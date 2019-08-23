using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.Core;
using MTGLambda.MTGLambda.DataClass.MTGLambdaCard;
using MTGLambda.MTGLambda.Helpers.Common;
using MTGLambda.MTGLambda.Helpers.DynamoDb;
using MTGLambda.MTGLambda.Services.MTG.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTGLambda.MTGLambda.DataRepository.Dao
{
    public class DaoCard : Dao<Card>
    {
        public DaoCard(DaoContext daoContext) : base(daoContext) { }

        public Card FindByName(string name, string manaCost)
        {
            LambdaLogger.Log($"Entering: FindByName({name})");

            //var cards = FindAll(string.Format("Name = {0}", name)).FirstOrDefault();
            var card = DynamoDbHelper.Load<Card>(name, manaCost);

            LambdaLogger.Log($"Leaving: FindByName({ JsonConvert.SerializeObject(card) })");

            return card;
        }

        //UNUSED
        public IEnumerable<Card> FindByColors(List<string> colors)
        {
            return FindAll(string.Format("Colors in ({0})", String.Join(',', colors)));
        }
        //UNUSED
        public IEnumerable<Card> FindByManaCost(string convertedManaCost)
        {
            return FindAll(string.Format("ManaCost = {0}", convertedManaCost));
        }

        //Name Filters
        public IEnumerable<Card> FindFromName(string name)
        {
            var conditions = new List<ScanCondition>
            {
                new ScanCondition("Name", ScanOperator.Contains, name)
            };

            return FindAll(conditions);
        }

        public IEnumerable<Card> FindFromNameAndManaCost(GetCardRequest request)
        {
            LambdaLogger.Log($"Entering: FindFromNameAndManaCost({ JsonConvert.SerializeObject(request) }");

            var conditions = new List<ScanCondition>();

            AddSearchConditions(request, conditions);

            AddManaCostConditions(request, conditions);

            return FindAll(conditions);
        }

        public IEnumerable<Card> FindFromNameAndColors(GetCardRequest request)
        {
            LambdaLogger.Log($"Entering: FindFromNameAndColors({ JsonConvert.SerializeObject(request) }");

            var conditions = new List<ScanCondition>();

            AddSearchConditions(request, conditions);

            AddColorConditions(request, conditions);

            var results = FindAll(conditions);

            if (request.ColorFilter.ContainsValue(false))
            {
                var negateColors = request.ColorFilter
                                          .Where(x => x.Value == false)
                                          .Select(x => x.Key)
                                          .ToList();

                foreach (var negateColor in negateColors)
                    results = results.Where(x => !x.ColorIdentity.Contains(negateColor)).ToList();

            }

            return results;
        }

        public IEnumerable<Card> FindFromNameAndBaseType(GetCardRequest request)
        {
            LambdaLogger.Log($"Entering: FindFromNameAndBaseType({ JsonConvert.SerializeObject(request) })");

            var conditions = new List<ScanCondition>();

            AddSearchConditions(request, conditions);

            AddBaseTypeConditions(request, conditions);

            return FindAll(conditions);
        }

        public IEnumerable<Card> FindFromNameAndManaCostAndColors(GetCardRequest request)
        {
            LambdaLogger.Log($"Entering: FindFromNameAndManaCostAndColors({ JsonConvert.SerializeObject(request) }");

            var conditions = new List<ScanCondition>();

            AddSearchConditions(request, conditions);

            AddManaCostConditions(request, conditions);

            AddColorConditions(request, conditions);

            var results = FindAll(conditions);

            if (request.ColorFilter.ContainsValue(false))
            {
                var negateColors = request.ColorFilter
                                          .Where(x => x.Value == false)
                                          .Select(x => x.Key)
                                          .ToList();

                foreach (var negateColor in negateColors)
                    results = results.Where(x => !x.ColorIdentity.Contains(negateColor)).ToList();

            }

            return results;
        }

        public IEnumerable<Card> FindFromNameAndManaCostAndBaseType(GetCardRequest request)
        {
            LambdaLogger.Log($"Entering: FindFromNameAndManaCostAndBaseType({ JsonConvert.SerializeObject(request) })");

            var conditions = new List<ScanCondition>();

            AddSearchConditions(request, conditions);

            AddManaCostConditions(request, conditions);

            AddBaseTypeConditions(request, conditions);

            return FindAll(conditions);
        }

        public IEnumerable<Card> FindFromNameAndBaseTypeAndColors(GetCardRequest request)
        {
            LambdaLogger.Log($"Entering: FindFromNameAndBaseTypeAndColors({ JsonConvert.SerializeObject(request) })");

            var conditions = new List<ScanCondition>();

            AddSearchConditions(request, conditions);

            AddBaseTypeConditions(request, conditions);

            AddColorConditions(request, conditions);

            var results = FindAll(conditions);

            if (request.ColorFilter.ContainsValue(false))
            {
                var negateColors = request.ColorFilter
                                          .Where(x => x.Value == false)
                                          .Select(x => x.Key)
                                          .ToList();

                foreach (var negateColor in negateColors)
                    results = results.Where(x => !x.ColorIdentity.Contains(negateColor)).ToList();

            }

            return results;
        }

        public IEnumerable<Card> FindFromNameAndManaCostAndColorsAndBaseType(GetCardRequest request)
        {
            LambdaLogger.Log($"Entering: FindFromNameAndManaCostAndColorsAndBaseType({ JsonConvert.SerializeObject(request) }");

            var conditions = new List<ScanCondition>();

            AddSearchConditions(request, conditions);

            AddManaCostConditions(request, conditions);

            AddColorConditions(request, conditions);

            AddBaseTypeConditions(request, conditions);

            var results = FindAll(conditions);

            if (request.ColorFilter.ContainsValue(false))
            {
                var negateColors = request.ColorFilter
                                          .Where(x => x.Value == false)
                                          .Select(x => x.Key)
                                          .ToList();

                foreach (var negateColor in negateColors)
                    results = results.Where(x => !x.ColorIdentity.Contains(negateColor)).ToList();

            }

            return results;
        }

        //Text Filters
        public IEnumerable<Card> FindFromText(string text)
        {
            var conditions = new List<ScanCondition>
            {
                new ScanCondition("CardText", ScanOperator.Contains, text)
            };

            return FindAll(conditions);
        }

        public IEnumerable<Card> FindFromTextAndManaCost(GetCardRequest request)
        {
            var conditions = new List<ScanCondition>();

            AddSearchConditions(request, conditions);

            AddManaCostConditions(request, conditions);

            return FindAll(conditions);
        }

        public IEnumerable<Card> FindFromTextAndColors(GetCardRequest request)
        {
            var conditions = new List<ScanCondition>();

            AddSearchConditions(request, conditions);

            AddColorConditions(request, conditions);

            var results = FindAll(conditions);

            //if (request.ColorFilter.ContainsValue(false))
            //{
            //    var negateColors = request.ColorFilter
            //                              .Where(x => x.Value == false)
            //                              .Select(x => x.Key)
            //                              .ToList();

            //    foreach (var negateColor in negateColors)
            //        results = results.Where(x => !x.ColorIdentity.Contains(negateColor)).ToList();

            //}

            return results;
        }

        public IEnumerable<Card> FindFromTextAndBaseType(GetCardRequest request)
        {
            LambdaLogger.Log($"Entering: FindFromTextAndBaseType({ JsonConvert.SerializeObject(request) })");

            var conditions = new List<ScanCondition>();

            AddSearchConditions(request, conditions);

            AddBaseTypeConditions(request, conditions);

            return FindAll(conditions);
        }

        public IEnumerable<Card> FindFromTextAndManaCostAndColors(GetCardRequest request)
        {
            LambdaLogger.Log($"Entering: FindFromTextAndManaCostAndColors({ JsonConvert.SerializeObject(request) })");

            var conditions = new List<ScanCondition>();

            AddSearchConditions(request, conditions);
            AddManaCostConditions(request, conditions);
            AddColorConditions(request, conditions);

            var results = FindAll(conditions);

            //if (request.ColorFilter.ContainsValue(false))
            //{
            //    var negateColors = request.ColorFilter
            //                              .Where(x => x.Value == false)
            //                              .Select(x => x.Key)
            //                              .ToList();

            //    foreach (var negateColor in negateColors)
            //        results = results.Where(x => !x.ColorIdentity.Contains(negateColor)).ToList();

            //}

            return results;
        }

        public IEnumerable<Card> FindFromTextAndManaCostAndBaseType(GetCardRequest request)
        {
            LambdaLogger.Log($"Entering: FindFromTextAndManaCostAndBaseType({ JsonConvert.SerializeObject(request) })");

            var conditions = new List<ScanCondition>();

            AddSearchConditions(request, conditions);
            AddManaCostConditions(request, conditions);
            AddBaseTypeConditions(request, conditions);

            return FindAll(conditions);
        }

        public IEnumerable<Card> FindFromTextAndBaseTypeAndColors(GetCardRequest request)
        {
            LambdaLogger.Log($"Entering: FindFromTextAndBaseTypeAndColors({ JsonConvert.SerializeObject(request) })");

            var conditions = new List<ScanCondition>();

            AddSearchConditions(request, conditions);
            AddBaseTypeConditions(request, conditions);
            AddColorConditions(request, conditions);

            var results = FindAll(conditions);

            //if (request.ColorFilter.ContainsValue(false))
            //{
            //    var negateColors = request.ColorFilter
            //                              .Where(x => x.Value == false)
            //                              .Select(x => x.Key)
            //                              .ToList();

            //    foreach (var negateColor in negateColors)
            //        results = results.Where(x => !x.ColorIdentity.Contains(negateColor)).ToList();

            //}

            return results;
        }

        public IEnumerable<Card> FindFromTextAndManaCostAndColorsAndBaseType(GetCardRequest request)
        {
            LambdaLogger.Log($"Entering: FindFromTextAndManaCostAndColorsAndBaseType({ JsonConvert.SerializeObject(request) })");

            var conditions = new List<ScanCondition>();

            AddSearchConditions(request, conditions);
            AddManaCostConditions(request, conditions);
            AddColorConditions(request, conditions);
            AddBaseTypeConditions(request, conditions);

            var results = FindAll(conditions);

            //if (request.ColorFilter.ContainsValue(false))
            //{
            //    var negateColors = request.ColorFilter
            //                              .Where(x => x.Value == false)
            //                              .Select(x => x.Key)
            //                              .ToList();

            //    foreach (var negateColor in negateColors)
            //        results = results.Where(x => !x.ColorIdentity.Contains(negateColor)).ToList();

            //}

            return results;
        }

        //Keyword (Tags) Filters
        public IEnumerable<Card> FindFromKeywordAndManaCostAndColorsAndBaseType(GetCardRequest request)
        {
            LambdaLogger.Log($"Entering: FindFromKeywordAndManaCostAndColorsAndBaseType({ JsonConvert.SerializeObject(request) })");

            var conditions = new List<ScanCondition>();

            AddSearchConditions(request, conditions);
            AddManaCostConditions(request, conditions);
            AddColorConditions(request, conditions);
            AddBaseTypeConditions(request, conditions);

            var results = FindAll(conditions);

            //if (request.ColorFilter.ContainsValue(false))
            //{
            //    var negateColors = request.ColorFilter
            //                              .Where(x => x.Value == false)
            //                              .Select(x => x.Key)
            //                              .ToList();

            //    foreach (var negateColor in negateColors)
            //        results = results.Where(x => !x.ColorIdentity.Contains(negateColor)).ToList();

            //}

            return results;
        }

        //No content (Mana Cost)
        public IEnumerable<Card> FindFromManaCost(GetCardRequest request)
        {
            LambdaLogger.Log($"Entering: FindFromManaCost({ JsonConvert.SerializeObject(request) })");

            var conditions = new List<ScanCondition>();

            AddManaCostConditions(request, conditions);

            return FindAll(conditions);
        }

        public IEnumerable<Card> FindFromManaCostAndColors(GetCardRequest request)
        {
            LambdaLogger.Log($"Entering: FindFromManaCostAndColors({ JsonConvert.SerializeObject(request) })");

            var conditions = new List<ScanCondition>();

            AddManaCostConditions(request, conditions);

            AddColorConditions(request, conditions);

            var results = FindAll(conditions);

            //if (request.ColorFilter.ContainsValue(false))
            //{
            //    var negateColors = request.ColorFilter
            //                              .Where(x => x.Value == false)
            //                              .Select(x => x.Key)
            //                              .ToList();

            //    foreach (var negateColor in negateColors)
            //        results = results.Where(x => !x.ColorIdentity.Contains(negateColor)).ToList();

            //}

            return results;
        }

        public IEnumerable<Card> FindFromManaCostAndBaseType(GetCardRequest request)
        {
            LambdaLogger.Log($"Entering: FindFromManaCostAndBaseType({ JsonConvert.SerializeObject(request) })");

            var conditions = new List<ScanCondition>();

            AddManaCostConditions(request, conditions);

            AddBaseTypeConditions(request, conditions);

            return FindAll(conditions);
        }

        public IEnumerable<Card> FindFromManaCostAndBaseTypeAndColors(GetCardRequest request)
        {
            LambdaLogger.Log($"Entering: FindFromManaCostAndBaseTypeAndColors({ JsonConvert.SerializeObject(request) })");

            var conditions = new List<ScanCondition>();

            AddManaCostConditions(request, conditions);

            AddBaseTypeConditions(request, conditions);

            AddColorConditions(request, conditions);

            var results = FindAll(conditions);

            //if (request.ColorFilter.ContainsValue(false))
            //{
            //    var negateColors = request.ColorFilter
            //                              .Where(x => x.Value == false)
            //                              .Select(x => x.Key)
            //                              .ToList();

            //    foreach (var negateColor in negateColors)
            //        results = results.Where(x => !x.ColorIdentity.Contains(negateColor)).ToList();

            //}

            return results;
        }

        //No content (Base Type)
        public IEnumerable<Card> FindFromBaseType(GetCardRequest request)
        {
            LambdaLogger.Log($"Entering: FindFromBaseType({ JsonConvert.SerializeObject(request) })");

            var conditions = new List<ScanCondition>();

            AddBaseTypeConditions(request, conditions);

            return FindAll(conditions);
        }

        public IEnumerable<Card> FindFromBaseTypeAndColors(GetCardRequest request)
        {
            LambdaLogger.Log($"Entering: FindFromBaseTypeAndColors({ JsonConvert.SerializeObject(request) })");

            var conditions = new List<ScanCondition>();

            AddBaseTypeConditions(request, conditions);

            AddColorConditions(request, conditions);

            var results = FindAll(conditions);

            //if (request.ColorFilter.ContainsValue(false))
            //{
            //    var negateColors = request.ColorFilter
            //                              .Where(x => x.Value == false)
            //                              .Select(x => x.Key)
            //                              .ToList();

            //    foreach (var negateColor in negateColors)
            //        results = results.Where(x => !x.ColorIdentity.Contains(negateColor)).ToList();

            //}

            return results;
        }

        //No content (Colors)
        public IEnumerable<Card> FindFromColors(GetCardRequest request)
        {
            LambdaLogger.Log($"Entering: FindFromColors({ JsonConvert.SerializeObject(request) })");

            var conditions = new List<ScanCondition>();

            AddColorConditions(request, conditions);

            var results = FindAll(conditions);

            //if (request.ColorFilter.ContainsValue(false))
            //{
            //    var negateColors = request.ColorFilter
            //                              .Where(x => x.Value == false)
            //                              .Select(x => x.Key)
            //                              .ToList();

            //    foreach (var negateColor in negateColors)
            //        results = results.Where(x => !x.ColorIdentity.Contains(negateColor)).ToList();

            //}

            return results;
        }
        
        private void AddSearchConditions(GetCardRequest request, List<ScanCondition> conditions)
        {
            if (!string.IsNullOrWhiteSpace(request.TextFilter))
            {
                conditions.Add(new ScanCondition("CardText", ScanOperator.Contains, request.TextFilter));
            }
            else if (!string.IsNullOrWhiteSpace(request.NameFilter))
            {
                //Capitalize first letter & lowercase all other letters for each search term
                var searchTerms = request.NameFilter
                                         .Split(new char[] { ' ' })
                                         .Select(x => !x.Contains('-') ? (char.ToUpper(x[0]) + x.Substring(1).ToLower()) : x)
                                         .Select(x => FormatPreposition(x))
                                         .ToList();

                searchTerms[0] = !searchTerms[0].Contains('-') ? char.ToUpper(searchTerms[0][0]) + searchTerms[0].Substring(1).ToLower() : searchTerms[0];

                conditions.Add(new ScanCondition("Name", ScanOperator.Contains, String.Join(' ', searchTerms.ToArray())));
                //addedTerms.Add(char.ToUpper(searchTerm[0]) + searchTerm.Substring(1));
                //string formattedTerm = char.ToUpper(searchTerm[0]) + searchTerm.Substring(1);
                //conditions.Add(new ScanCondition("Name", ScanOperator.Contains, searchTerm));
                //conditions.Add(new ScanCondition("Name", ScanOperator.Contains, formattedTerm));
            }
            else if (!string.IsNullOrWhiteSpace(request.KeywordsFilter))
            {
                conditions.Add(new ScanCondition("Tags", ScanOperator.Contains, request.KeywordsFilter));
            }
        }

        private string FormatPreposition(string prep)
        {
            List<string> noCaps = new List<string>() { "Of", "And", "Or", "In", "At", "The", "To" };

            if (noCaps.Contains(prep)) 
            {
                return char.ToLower(prep[0]) + prep.Substring(1);
            }

            return prep;
        }

        private void AddColorConditions(GetCardRequest request, List<ScanCondition> conditions)
        {
            List<string> containsList = new List<string>();
            List<string> excludeList = new List<string>();

            foreach (var keypair in request.ColorFilter)
            {
                if (keypair.Value == null)
                {
                    //Doesn't matter
                }
                else if (keypair.Value.Value) //True
                {
                    containsList.Add(keypair.Key);
                }
                else if (!keypair.Value.Value) //False
                {
                    excludeList.Add(keypair.Key);
                }
            }

            foreach(var colorCondition in containsList)
            {
                conditions.Add(new ScanCondition("ColorIdentity", ScanOperator.Contains, colorCondition));
            }

            foreach(var negateCondition in excludeList)
            {
                conditions.Add(new ScanCondition("ColorIdentity", ScanOperator.NotContains, negateCondition));
            }

            //if (containsList.Any())
            //    conditions.Add(new ScanCondition("ColorIdentity", ScanOperator.Contains, containsList.ToArray()));

            //if (excludeList.Any())
            //    conditions.Add(new ScanCondition("ColorIdentity", ScanOperator.NotContains, excludeList.ToArray()));
        }

        private void AddManaCostConditions(GetCardRequest request, List<ScanCondition> conditions)
        {
            LambdaLogger.Log($"Entering: AddManaCostConditions({ JsonConvert.SerializeObject(request) })");

            var queryList = new List<Single>();
            foreach (var manaCost in request.ManaCostFilter)
            {
                if (manaCost.Value)
                {
                    queryList.Add(Single.Parse(manaCost.Key.ToString()));
                }
            }

            if (queryList.Contains(Single.Parse("7")))
            {
                queryList.AddRange(new List<Single> { Single.Parse("8"), Single.Parse("9"),
                                                      Single.Parse("10"), Single.Parse("11"),
                                                      Single.Parse("12"), Single.Parse("13"),
                                                      Single.Parse("14"), Single.Parse("15") });
            }

            LambdaLogger.Log($"Query List: { JsonConvert.SerializeObject(queryList) }");

            if (queryList.Any())
            {
                var objects = queryList.Select(x => (object)x).ToArray();

                var condition = new ScanCondition("ManaCost", ScanOperator.In, objects );
                conditions.Add(condition);
            }
        }

        private void AddBaseTypeConditions(GetCardRequest request, List<ScanCondition> conditions)
        {
            LambdaLogger.Log($"Entering: AddBaseTypeConditions({ JsonConvert.SerializeObject(request) })");

            var queryList = new List<string>();
            foreach(var baseType in request.BaseTypeFilter)
            {
                if (baseType.Value)
                {
                    queryList.Add(baseType.Key);
                }
            }

            LambdaLogger.Log($"Query List: { JsonConvert.SerializeObject(queryList) }");

            if (queryList.Any())
            {
                var objects = queryList.Select(x => (object)x).ToArray();

                var condition = new ScanCondition("BaseType", ScanOperator.Contains, objects);
                conditions.Add(condition);
            }
        }

        private object FloatToObject(float f)
        {
            return (object)f;
        }
    }
}
