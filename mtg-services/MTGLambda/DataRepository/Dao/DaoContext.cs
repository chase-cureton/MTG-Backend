using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.Core;
using MTGLambda.MTGLambda.DataRepository.Dto;
using MTGLambda.MTGLambda.Helpers.DynamoDb;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        public LoadTableItemsResponse LoadTableItems(LoadTableItemsRequest request)
        {
            var response = new LoadTableItemsResponse
            {
                IsSuccess = true
            };

            try
            {
                //Use request to hit dynamodb
                //If has an id, then can use dynamodb GetItem, else need to query based on filter
            }
            catch(Exception exp)
            {
                LambdaLogger.Log($"Error: { exp }");
            }

            return response;
        }

        /// <summary>
        /// Inserts item into table from DynamoDb
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public InsertTableItemResponse InsertTableItem(InsertTableItemRequest request)
        {
            LambdaLogger.Log($"Entering: InsertTableItem({ JsonConvert.SerializeObject(request) }");

            var response = new InsertTableItemResponse
            {
                IsSuccess = true
            };
            
            try
            {
                //Need dynamo db helper method to write
            }
            catch(Exception exp)
            {
                LambdaLogger.Log($"Error: { exp }");
            }

            LambdaLogger.Log($"Leaving: InsertTableItem({ JsonConvert.SerializeObject(response) }");
            return response;
        }
    }
}
