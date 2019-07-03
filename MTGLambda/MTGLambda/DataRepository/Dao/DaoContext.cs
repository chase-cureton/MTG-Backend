using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.Core;
using MTGLambda.MTGLambda.DataRepository.Dto;
using MTGLambda.MTGLambda.Helpers.DynamoDb;
using MTGLambda.MTGLambda.Helpers.DynamoDb.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTGLambda.MTGLambda.DataRepository.Dao
{
    public class DaoContext
    {
        /// <summary>
        /// Loads table from DynamoDb
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public LoadTableResponse LoadTable(LoadTableRequest request)
        {
            LambdaLogger.Log($"Entering: LoadTable({ JsonConvert.SerializeObject(request) })");

            var response = new LoadTableResponse
            {
                IsSuccess = true
            };

            try
            {
                response.ResponseTable = DynamoDbHelper.GetTable(request.TableName);
            }
            catch(Exception exp)
            {
                LambdaLogger.Log($"Error: { exp }");

                response.IsSuccess = false;
                response.ErrorMessage = "Error while loading table.";

                LambdaLogger.Log($"Leaving: LoadTable({ JsonConvert.SerializeObject(response) })");
                return response;
            }

            LambdaLogger.Log($"Leaving: LoadTable({ JsonConvert.SerializeObject(response) })");
            return response;
        }

        /// <summary>
        /// Loads table items from DynamoDb
        /// - Filter property is used to query against DynamoDb table
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public IEnumerable<T> LoadTableItems<T>(LoadTableItemsRequest request)
        {
            LambdaLogger.Log($"Entering: LoadTableItems({ JsonConvert.SerializeObject(request) }");

            var response = new List<T>();

            try
            {
                SearchDocumentsRequest searchDocumentsRequest = CreateSearchDocumentsRequest(request);

                //var tableItems = DynamoDbHelper.SearchDocuments(searchDocumentsRequest); 

                //TODOING: build conditions from request filter

                var tableItems = DynamoDbHelper.Scan<T>(request.Conditions);

                //TODO: Handle order by expression and record cap here

                //Use request to hit dynamodb
                //If has an id, then can use dynamodb GetItem, else need to query based on filter
                response = tableItems.ToList();
            }
            catch (Exception exp)
            {
                LambdaLogger.Log($"Error: { exp }");

                LambdaLogger.Log($"Leaving: LoadTableItems({ JsonConvert.SerializeObject(response) }");
                return response;
            }

            LambdaLogger.Log($"Leaving: LoadTableItems({ JsonConvert.SerializeObject(response) }");
            return response;
        }

        /// <summary>
        /// Inserts item into table from DynamoDb
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public UpsertTableItemResponse UpsertTableItems(UpsertTableItemsRequest request)
        {
            LambdaLogger.Log($"Entering: UpsertTableItems({ JsonConvert.SerializeObject(request) }");

            var response = new UpsertTableItemResponse
            {
                IsSuccess = true
            };
            
            try
            {
                //First scan table to see which documents already exist


                //Need dynamo db helper method to write
                DynamoDbHelper.BatchWriteDocuments(request.Table, request.Documents);

                //Because the batch save won't return because I don't wanna await that call
                response.Documents = request.Documents;
            }
            catch(Exception exp)
            {
                LambdaLogger.Log($"Error: { exp }");

                response.IsSuccess = false;
                response.ErrorMessage = "Error while inserting table item.";

                LambdaLogger.Log($"Leaving: UpsertTableItems({ JsonConvert.SerializeObject(response) }");
                return response;
            }

            LambdaLogger.Log($"Leaving: UpsertTableItems({ JsonConvert.SerializeObject(response) }");
            return response;
        }

        #region Helper methods

        private static SearchDocumentsRequest CreateSearchDocumentsRequest(LoadTableItemsRequest request)
        {
            LambdaLogger.Log($"Entering: CreateSearchDocumentsRequest({ JsonConvert.SerializeObject(request) })");

            SearchDocumentsRequest searchDocumentsRequest = new SearchDocumentsRequest
            {
                TableName = request.Table
            };

            try
            {
                //Enter the Regex
                // - goal here is to map request.Filter to Dictionary<string, List<string>>
                request.Filter = request.Filter.ToLower();

                List<string> tokens = request.Filter
                                             .Split(new char[] { ' ', ',' })
                                             .ToList();

                //Example Case #2
                //tokens = ["attr1", "=", "value1", "and", "attr2", "=", "value2"]
                //keyFilters = [{"attr1", ["=", "value1"]}, {"attr2", ["=", "value2"]}]
                if (tokens.Contains("&&"))
                {
                    var subFilters = request.Filter.Split("&&");

                    foreach (var subFilter in subFilters)
                    {
                        var subTokens = subFilter.Split(new char[] { ' ', ',' })
                                                 .ToList();

                        //TODO: Will check later for ORs ("||") here
                        ParseBaseQuery(searchDocumentsRequest, subTokens);
                    }
                }
            } catch (Exception exp) {
                LambdaLogger.Log($"Error: { exp }");

                LambdaLogger.Log($"Leaving: CreateSearchDocumentsRequest({ JsonConvert.SerializeObject(searchDocumentsRequest) })");
                throw exp;
            }

            LambdaLogger.Log($"Leaving: CreateSearchDocumentsRequest({ JsonConvert.SerializeObject(searchDocumentsRequest) })");
            return searchDocumentsRequest;
        }

        private static void ParseBaseQuery(SearchDocumentsRequest searchDocumentsRequest, List<string> tokens)
        {
            if (tokens.Contains("="))
            {
                //Example Case #1
                //tokens = ["a", "=", "b"]
                //keyFilter = {"a", ["=", "b"]}
                searchDocumentsRequest.KeyFilters.Add(tokens[0], new List<string> { "=", tokens[2] });
            }
            else if (tokens.Contains("in"))
            {
                //Example Case #3
                //tokens = ["attr1", "in", "(", "value1", "value2", ")"]
                //keyFilter = {"attr1", ["in", "value1", "value2"]}

                tokens.RemoveAll(x => x == "(" || x == ")" || x == "in");

                var queryValues = new List<string> { "in" };

                foreach (var token in tokens.GetRange(1, tokens.Count))
                {
                    queryValues.Add(token);
                }

                searchDocumentsRequest.KeyFilters.Add(tokens[0], queryValues);
            }
            else if (tokens.Contains("like"))
            {
                throw new NotImplementedException("Operator: like");
            }
            else if (tokens.Contains("between"))
            {
                throw new NotImplementedException("Operator: between");
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
