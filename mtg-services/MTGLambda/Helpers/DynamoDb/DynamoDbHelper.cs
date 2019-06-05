using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.Core;
using MTGLambda.MTGLambda.Helpers.SecretsManager;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTGLambda.MTGLambda.Helpers.DynamoDb
{
    public static class DynamoDbHelper
    {
        private static AmazonDynamoDBClient _client;

        static DynamoDbHelper()
        {
            _client = new AmazonDynamoDBClient(SecretsManagerHelper.GetS3SecretValue("key"), 
                                              SecretsManagerHelper.GetS3SecretValue("secret"), 
                                              RegionEndpoint.USEast1);
        }

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

        //TODO: Explore Query & Search
        //To be used in base DaoContext
    }
}
