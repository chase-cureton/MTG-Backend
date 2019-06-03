using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using HtmlAgilityPack;
using Newtonsoft.Json;
using mtg_services.Models.TCG;
using System.Net.Http;
using mtg_services.Services.TCG;
using mtg_services.Models.MTG.Deck;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace mtg_services
{
    public class Functions
    {
        
        /// <summary>
        /// Default constructor that Lambda will invoke.
        /// </summary>
        public  APIGatewayProxyResponse GetPriceAPI(APIGatewayProxyRequest request, ILambdaContext context)
        {
            LambdaLogger.Log(string.Format("Entering: {0}", JsonConvert.SerializeObject(request)));

            try
            {
                HtmlWeb web = new HtmlWeb();

                string set = request.PathParameters["set-name"];
                string card = request.PathParameters["card-name"];

                string url = string.Format($"https://shop.tcgplayer.com/magic/{0}/{1}", set, card);
                var htmlDoc = web.Load(url);

                //LambdaLogger.Log(string.Format("Http Response: {0}", JsonConvert.SerializeObject(htmlDoc)));

                var prices = htmlDoc.DocumentNode
                                    .Descendants()
                                    .Where(x => x.Attributes.Contains("class") && 
                                                (x.Attributes["class"].Value.Contains("product-offer__price") ||
                                                 x.Attributes["class"].Value.Contains("seller-spotlight__price")));

                var response = new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Body = JsonConvert.SerializeObject(prices),
                    Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
                };

                LambdaLogger.Log(string.Format("Leaving: {0}", JsonConvert.SerializeObject(request)));

                return response;
            }
            catch(Exception exp)
            {
                LambdaLogger.Log(string.Format("Exception occurred: {0}", exp));

                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Body = "y u do dis?",
                    Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
                };
            }
        }

        /// <summary>
        /// A Lambda function to respond to HTTP Get methods from API Gateway
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The list of blogs</returns>
        public APIGatewayProxyResponse Get(APIGatewayProxyRequest request, ILambdaContext context)
        {
            context.Logger.LogLine("Get Request\n");

            var response = new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = "Hello AWS Serverless",
                Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
            };

            return response;
        }

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
                    artifacts = 10,
                    creatures = 24,
                    deckId = 1,
                    deckName = "The Harbinger (Deck #1)",
                    enchantments = 4,
                    instants = 12,
                    sorceries = 8,
                    planeswalkers = 3,
                    lands = 38
                });

                responseContent.Add(new DeckOverviewDto()
                {
                    artifacts = 6,
                    creatures = 30,
                    deckId = 2,
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
    }
}
