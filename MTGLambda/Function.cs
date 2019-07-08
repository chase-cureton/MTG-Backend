using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using MTGLambda.MTGLambda.Services.TCG.Dto;
using MTGLambda.MTGLambda.Services.TCG;
using MTGLambda.MTGLambda.DataClass.MTGLambdaDeck;
using MTGLambda.MTGLambda.Services.MTG;
using MTGLambda.MTGLambda.Services;
using MTGLambda.MTGLambda.DataClass.MTGLambdaCard;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace MTGLambda
{
    public class Functions
    {
        /// <summary>
        /// A wrapper function for TCG's search API.
        /// - Will return list of card details instead of ids based on search criteria
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<APIGatewayProxyResponse> Search(APIGatewayProxyRequest request, ILambdaContext context)
        {
            LambdaLogger.Log($"Entering: Search({JsonConvert.SerializeObject(request)})");

            APIGatewayProxyResponse response = new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json"} }
            };

            try
            {
                try
                {
                    TCGSearchRequest searchRequest = JsonConvert.DeserializeObject<TCGSearchRequest>(request.Body);

                    if (searchRequest == null)
                        throw new Exception($"Request Body sucks...look at this: {request.Body} ...does that look right to you?");

                    LambdaLogger.Log($"Search Response: { JsonConvert.SerializeObject(searchRequest) }");

                    //TODO: Come up with response
                    //- TCGResponse will contain ids for cards
                    //- Need to use TCGResponse ids to retrieve actual card information
                    //- Return list of this card information
                    TCGProductResponse searchResponse = await new TCGService().Search(searchRequest);

                    LambdaLogger.Log($"Search Response: { JsonConvert.SerializeObject(searchResponse) }");

                    if (searchResponse == null)
                        throw new Exception($"TCG Response is null...WTF, like why?");

                    response.Body = JsonConvert.SerializeObject(searchResponse);
                }
                catch(JsonSerializationException jsonExp)
                {
                    LambdaLogger.Log($"Deserialization Exception: {jsonExp}");

                    throw new Exception($"Request Body stinks...look at this: {request.Body} ...does that look right to you?");
                }

            }
            catch(Exception exp)
            {
                LambdaLogger.Log($"Exception: {exp}");

                Dictionary<string, string> body = new Dictionary<string, string>()
                {
                    { "Message", "rly dude/lady?lady/dude? y u breaking things?" },
                    { "Error1", "It's probably your fault" },
                    { "Error2", $"It might be this -> { exp.Message }" }
                };

                response = new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } },
                    Body = JsonConvert.SerializeObject(body)
                };
            }

            LambdaLogger.Log($"Leaving: Search({ JsonConvert.SerializeObject(response) })");

            return response;
        }

        /// <summary>
        /// Search card function for dynamo db
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public APIGatewayProxyResponse SearchCards(APIGatewayProxyRequest request, ILambdaContext context)
        {
            LambdaLogger.Log($"Entering: SearchCards({ JsonConvert.SerializeObject(request) })");

            var response = new APIGatewayProxyResponse()
            {
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" },
                    { "Access-Control-Allow-Origin", "*" }
                }
            };

            try
            {
                var requestParams = JsonConvert.DeserializeObject<Dictionary<string, string>>(request.Body);

                var MTGService = ServiceFactory.GetService<MTGService>();

                LambdaLogger.Log($"About to search for card...{requestParams["Name"]}");

                var cards = MTGService.GetCardsFromName(requestParams["Name"]);

                LambdaLogger.Log($"Cards retrieved => cards: { JsonConvert.SerializeObject(cards) }");

                response.Body = JsonConvert.SerializeObject(cards);
                response.StatusCode = (int)HttpStatusCode.OK;
            }
            catch(Exception exp)
            {
                LambdaLogger.Log($"Error: { exp }");

                Dictionary<string, string> body = new Dictionary<string, string>()
                {
                    { "Message", "y u breaking my stuff?" },
                    { "Error", $"This is all you get, here's your peasant error: { exp.Message }" }
                };

                var errorResponse = new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Headers = new Dictionary<string, string> {
                        { "Content-Type", "application/json" },
                        { "Access-Control-Allow-Origin", "*" }
                    },
                    Body = JsonConvert.SerializeObject(body)
                };

                LambdaLogger.Log($"Leaving: SearchCards({ JsonConvert.SerializeObject(errorResponse) })");
                return errorResponse;
            }

            LambdaLogger.Log($"Leaving: SearchCards({ JsonConvert.SerializeObject(response) })");
            return response;
        }

        /// <summary>
        /// Function to return user's deck overview
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<APIGatewayProxyResponse> GetUserDecksOverview(APIGatewayProxyRequest request, ILambdaContext context)
        {
            LambdaLogger.Log($"Entering: GetUserDecksOverview({JsonConvert.SerializeObject(request)}");

            var response = new APIGatewayProxyResponse()
            {
                Headers = new Dictionary<string, string> {
                    { "Content-Type", "application/json" },
                    { "Access-Control-Allow-Origin", "*" }
                }
            };
   
            try
            {
                List<DeckOverviewDto> responseContent = new List<DeckOverviewDto>();

                responseContent.Add(new DeckOverviewDto()
                {
                    id = 1,
                    artifacts = 10,
                    creatures = 24,
                    deckName = "The Harbinger (Deck #1)",
                    enchantments = 4,
                    instants = 12,
                    sorceries = 8,
                    planeswalkers = 3,
                    lands = 38
                });

                responseContent.Add(new DeckOverviewDto()
                {
                    id = 2,
                    artifacts = 6,
                    creatures = 30,
                    deckName = "The Weeper (Deck #2)",
                    enchantments = 4,
                    instants = 9,
                    sorceries = 6,
                    planeswalkers = 4,
                    lands = 36
                });

                response.StatusCode = (int)HttpStatusCode.OK;
                response.Body = JsonConvert.SerializeObject(responseContent);

                LambdaLogger.Log($"Response: { JsonConvert.SerializeObject(response) }");
            }
            catch(Exception exp)
            {
                LambdaLogger.Log($"Exception: {exp}");

                Dictionary<string, string> body = new Dictionary<string, string>()
                {
                    { "Message", "y u breaking my stuff?" },
                    { "Error", $"This is all you get, here's your peasant error: {exp.Message}" }
                };

                var errorResponse = new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Headers = new Dictionary<string, string> {
                        { "Content-Type", "application/json" },
                        { "Access-Control-Allow-Origin", "*" }
                    },
                    Body = JsonConvert.SerializeObject(body)
                };

                LambdaLogger.Log($"Leaving: GetUserDecksOverview({JsonConvert.SerializeObject(errorResponse)}");

                return errorResponse;
            }

            LambdaLogger.Log($"Leaving: GetUserDecksOverview({JsonConvert.SerializeObject(response)}");
            return response;
        }

        /// <summary>
        /// Imports cards into db
        /// - Request Parameters:
        /// - Page or (PageStart and PageEnd)
        /// - PageSize
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<APIGatewayProxyResponse> ImportCards(APIGatewayProxyRequest request, ILambdaContext context)
        {
            LambdaLogger.Log($"Entering: ImportCards({ JsonConvert.SerializeObject(request) })");

            var response = new APIGatewayProxyResponse()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" },
                    //{ "Access-Control-Allow-Origin", "*" }
                }
            };

            try
            {
                var input = JsonConvert.DeserializeObject<Dictionary<string, int>>(request.Body);

                var importService = ServiceFactory.GetService<ImportService>();

                if (input.ContainsKey("Page"))
                {
                    response.Body = $"Importing page: { input["Page"] } and page size: { input["PageSize"]}";
                    importService.ImportCards(input["Page"], input["PageSize"]);
                }
                else if (input.ContainsKey("PageStart"))
                {
                    response.Body = $"Importing page start/end: { input["PageStart"] } / { input["PageEnd"] } and page size: { input["PageSize"]}";
                    importService.ImportCards(input["PageStart"], input["PageEnd"], input["PageSize"]);
                }
            }
            catch(Exception exp)
            {
                LambdaLogger.Log($"Error: { exp }");

                Dictionary<string, string> body = new Dictionary<string, string>()
                {
                    { "Message", "y u breaking my stuff?" },
                    { "Error", $"This is all you get, here's your peasant error: { exp.Message }" }
                };

                var errorResponse = new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Headers = new Dictionary<string, string> {
                        { "Content-Type", "application/json" },
                    },
                    Body = JsonConvert.SerializeObject(body)
                };

                LambdaLogger.Log($"Leaving: ImportCards({ JsonConvert.SerializeObject(errorResponse) })");
                return errorResponse;

            }

            LambdaLogger.Log($"Leaving: ImportCards({ JsonConvert.SerializeObject(response) })");
            return response;
        }

        /// <summary>
        /// Cross origin support / set this up as preflight options method
        /// for each method that returns to a browser
        /// </summary>
        /// <param name="apiGatewayProxyRequest"></param>
        /// <param name="lambdaContext"></param>
        /// <returns></returns>
        public async Task<APIGatewayProxyResponse> Cors(APIGatewayProxyRequest apiGatewayProxyRequest, ILambdaContext lambdaContext)
        {
            return new APIGatewayProxyResponse
            {
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" },
                                                           { "Access-Control-Allow-Origin", "*" },
                                                           { "Access-Control-Allow-Methods", "GET,PUT,POST,DELETE,OPTIONS" },
                                                           { "Access-Control-Allow-Headers", "*"} },
                StatusCode = (int)HttpStatusCode.OK,
                Body = string.Empty
            };
        }
    }
}
