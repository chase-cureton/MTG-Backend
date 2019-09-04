using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Core;
using MTGLambda.MTGLambda.Helpers.DynamoDb.Dto;
using MTGLambda.MTGLambda.Helpers.SecretsManager;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTGLambda.MTGLambda.Helpers.DynamoDb
{
    public static class DynamoDbHelper
    {
        private static AmazonDynamoDBClient _client;
        private static DynamoDBContext _context;

        static DynamoDbHelper()
        {
            _client = new AmazonDynamoDBClient(SecretsManagerHelper.GetS3SecretValue("key"), 
                                              SecretsManagerHelper.GetS3SecretValue("secret"), 
                                              RegionEndpoint.USEast1);

            _context = new DynamoDBContext(_client);
        }

        public static AmazonDynamoDBClient GetClient()
        {
            return _client;
        }

        /// <summary>
        /// Retrieves table(sql table equivalent) from DynamoDB
        /// - Search by name
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static Table GetTable(string tableName)
        {
            LambdaLogger.Log($"Entering: GetTable({ tableName })");
            Table response = null;

            try
            {
                response = Table.LoadTable(_client, tableName);
            }
            catch(Exception exp)
            {
                LambdaLogger.Log($"Error: { exp }");
                throw;
            }

            LambdaLogger.Log($"Leaving: GetTable({ JsonConvert.SerializeObject(response) })");
            return response;
        }

        /// <summary>
        /// Retrieves document(sql record equivalent) from table
        /// - Search by id
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="documentId"></param>
        /// <returns></returns>
        public static Document GetDocument(string tableName, string documentId)
        {
            LambdaLogger.Log($"Entering: GetDocument(Table: {tableName}, DocumentId: {documentId})");

            Document response = null;

            try
            {
                Table documentTable = GetTable(tableName);

                if (documentTable == null)
                    throw new Exception($"Could not find table: { tableName }");

                response = documentTable.GetItemAsync(documentId).Result;
            }
            catch (Exception exp)
            {
                LambdaLogger.Log($"Error: { exp }");
                throw;
            }

            LambdaLogger.Log($"Leaving: GetDocument({ JsonConvert.SerializeObject(response) })");
            return response;
        }

        //TODO: Explore Query & Scan
        /// <summary>
        /// keyFilters format:
        /// ['=', 'like', 'in']
        /// if ('=') then assert list.count = 1
        /// if ('in') then assert list.count >= 1
        /// - Read as, return documents with property that is equal to a value in the list
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="keyFilters"></param>
        /// <returns></returns>
        public static IEnumerable<Document> SearchDocuments(SearchDocumentsRequest request)
        {
            LambdaLogger.Log($"Entering: SearchDocuments({ JsonConvert.SerializeObject(request) }");

            var documents = new List<Document>();

            //TODO: Change tableName and keyFilters to be in request
            //Validation of the request
            try
            {
                var expressionFilterObject = BuildExpressionFilterObject(request.KeyFilters);
                var keyConditionExpression = BuildKeyConditionExpression(expressionFilterObject.ExpressionGeneratorList);

                QueryRequest queryRequest = new QueryRequest
                {
                    TableName = request.TableName,
                    ExpressionAttributeNames = expressionFilterObject.ExpressionAttributeNames,
                    ExpressionAttributeValues = expressionFilterObject.ExpressionAttributeValues,
                    KeyConditionExpression = keyConditionExpression
                };

                Task<QueryResponse> queryTask = _client.QueryAsync(queryRequest);

                LambdaLogger.Log($"About to run Query: { JsonConvert.SerializeObject(queryRequest) }");

                var queryResponse = queryTask.Result;

                LambdaLogger.Log($"Query Response: { JsonConvert.SerializeObject(queryResponse) }");

                //TODO: Build out response
            }
            catch(Exception exp)
            {
                LambdaLogger.Log($"Error: { exp }");
                throw;
            }

            LambdaLogger.Log($"Leaving: SearchDocuments({ JsonConvert.SerializeObject(documents) }");
            return documents;
        }

        //TODO: Insert & Update

        /// <summary>
        /// Writes a list of documents to DynamoDb table
        /// - TODO: Create BatchWriteDocumentsRequest
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="documents"></param>
        public static void BatchWriteDocuments(string tableName, List<Document> documents)
        {
            try
            {
                var table = GetTable(tableName);
                var tableBatchWrite = table.CreateBatchWrite();

                foreach(var document in documents)
                {
                    tableBatchWrite.AddDocumentToPut(document);
                }

                tableBatchWrite.ExecuteAsync();
            }
            catch(Exception exp)
            {
                LambdaLogger.Log($"Error: { exp }");
            }
        }

        /// <summary>
        /// Load record given primary key and hash (sort) key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="primaryKey"></param>
        /// <param name="hashKey"></param>
        /// <returns></returns>
        public static T Load<T>(string primaryKey, string hashKey)
        {
            return _context.LoadAsync<T>(primaryKey, hashKey).Result;
        }

        /// <summary>
        /// Save for individual item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public static void Save<T>(T t)
        {
            var batch = _context.CreateBatchWrite<T>();
            batch.AddPutItem(t);

            _context.ExecuteBatchWriteAsync(new BatchWrite[] { batch });
            //batch.ExecuteAsync();
        }

        /// <summary>
        /// Save for multiple items
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void Save<T>(IEnumerable<T> list)
        {
            //Adds list of items to put request
            var batch = _context.CreateBatchWrite<T>();
            batch.AddPutItems(list.ToList());

            _context.ExecuteBatchWriteAsync(new BatchWrite[] { batch });
            //batch.ExecuteAsync();
        }

        public static async Task SaveAsync<T>(IEnumerable<T> list)
        {
            var batch = _context.CreateBatchWrite<T>();
            batch.AddPutItems(list.ToList());

            await _context.ExecuteBatchWriteAsync(new BatchWrite[] { batch });
        }

        public static async Task DeleteAsync<T>(IEnumerable<T> list)
        {
            foreach(var item in list)
            {
                await _context.DeleteAsync<T>(item);
            }
        }

        /// <summary>
        /// Scans table and returns results based on conditions
        /// Note: Only to be used for small record size tables
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public static IEnumerable<T> Scan<T>(List<ScanCondition> conditions)
        {
            List<T> response = new List<T>();

            var searchEngine = _context.ScanAsync<T>(conditions);
            var items = searchEngine.GetNextSetAsync();

            response = items.Result;

            while (!searchEngine.IsDone)
            {
                items = searchEngine.GetNextSetAsync();

                response.AddRange(items.Result);
            }

            return response;
        }

        //TODO: Query

        #region Helper methods
        /// <summary>
        /// Builds each of the main QueryRequest components
        /// - Attribute name map
        /// - Query value map
        /// - Expression
        /// </summary>
        /// <param name="keyFilters"></param>
        /// <returns></returns>
        private static ExpressionFilterObject BuildExpressionFilterObject(Dictionary<string, List<string>> keyFilters)
        {
            ExpressionFilterObject response = new ExpressionFilterObject();

            try
            {
                int i = 0;
                int j = 0;
                foreach (var keyFilter in keyFilters)
                {
                    var attrName = $"#{i}";
                    var queryOperator = string.Empty;

                    response.ExpressionAttributeNames.Add(attrName, keyFilter.Key);

                    switch (keyFilter.Value[0])
                    {
                        case "=":
                            queryOperator = "=";
                            break;
                        case "like":
                            queryOperator = "like";
                            throw new NotImplementedException();
                        case "in":
                            queryOperator = "in";
                            break;
                        default:
                            throw new NotImplementedException();
                    }

                    //Iterates the rest of the list to get compare values
                    foreach (var filterValue in keyFilter.Value.GetRange(1, keyFilter.Value.Count - 1))
                    {
                        //i: alias for property being queried
                        //q{i}_{j}: alias for expression denoting property value and compare value

                        var attrValue = $":q{i}_{j}";

                        response.ExpressionAttributeValues.Add(attrValue, new AttributeValue(keyFilter.Value));

                        response.ExpressionGeneratorList.Add(new Dictionary<string, string>() { { attrName, $"= { attrValue }" } });

                        j++;
                    }

                    i++;
                }
            }
            catch (Exception exp)
            {
                throw;
            }

            return response;
        }

        /// <summary>
        /// Builds expression string from list of expressions
        /// </summary>
        /// <param name="expressionGeneratorList"></param>
        /// <returns></returns>
        private static string BuildKeyConditionExpression(List<Dictionary<string, string>> expressionGeneratorList)
        {
            string response = string.Empty;

            StringBuilder stringBuilder = new StringBuilder();

            string currentKey = string.Empty, previousKey = string.Empty;
            foreach (var expression in expressionGeneratorList)
            {
                foreach (var expressionEntry in expression) //1 Entry dictionary, done for access to key and value (change this later)
                {
                    currentKey = expressionEntry.Key;
                    if (currentKey == previousKey && !string.IsNullOrEmpty(currentKey))
                    {
                        stringBuilder.Append(" or ");
                    }
                    else if (currentKey != previousKey && !string.IsNullOrEmpty(previousKey))
                    {
                        stringBuilder.Append(" and ");
                    }
                    stringBuilder.AppendFormat("{0} {1}", expressionEntry.Key, expressionEntry.Value);
                }
                previousKey = currentKey;
            }

            response = stringBuilder.ToString();

            LambdaLogger.Log($"KeyConditionExpression: { JsonConvert.SerializeObject(response) }");

            return response;
        }
        #endregion
    }
}
