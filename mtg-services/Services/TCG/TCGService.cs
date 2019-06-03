﻿using Amazon.Lambda.Core;
using mtg_services.Models.S3;
using mtg_services.Models.TCG;
using mtg_services.Utilities.Constants;
using mtg_services.Utilities.S3;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace mtg_services.Services.TCG
{
    public class TCGService
    {
        private static HttpClient httpClient = new HttpClient();

        #region Authorization
        public async Task<string> GetToken()
        {
            LambdaLogger.Log($"Entering: GetToken()");

            string response = string.Empty;

            try
            {
                S3GetFileResponse S3Response = await S3Helper.GetFile(new S3GetFileRequest
                {
                    FilePath = MTGServiceConstants.AUTHENTICATE_JSON_FILEPATH
                });

                if (S3Response.IsSuccess)
                {
                    if (string.IsNullOrEmpty(S3Response.FileContent))
                        throw new Exception("S3 Response content is null or empty! ya bish!");

                    AuthenticateDto authenticateDto = JsonConvert.DeserializeObject<AuthenticateDto>(S3Response.FileContent);

                    if (authenticateDto == null)
                        throw new Exception("S3 Authenticate response content could not be deserialized!");

                    response = authenticateDto.AccessToken;
                }
            }
            catch(Exception exp)
            {
                LambdaLogger.Log($"Error: {exp}");
                throw;
            }

            LambdaLogger.Log($"Leaving: GetToken({response})");

            return response;
        }

        public async Task<AuthenticateDto> Authenticate()
        {
            LambdaLogger.Log($"Entering: Authenticate()");

            AuthenticateDto response = null;

            try
            {
                string url = $"{TCGConstants.BASE_URL}/token";

                StringContent content = new StringContent(TCGConstants.AUTH_STRING);

                using (var httpClient = new HttpClient())
                {
                    HttpResponseMessage httpResponse = await httpClient.PostAsync(url, content);

                    LambdaLogger.Log($"Http Response: {JsonConvert.SerializeObject(httpResponse)}");

                    if (httpResponse != null)
                    {
                        if (httpResponse.IsSuccessStatusCode)
                        {
                            response = JsonConvert.DeserializeObject<AuthenticateDto>(await httpResponse.Content.ReadAsStringAsync());

                            LambdaLogger.Log($"Authentication Response: {JsonConvert.SerializeObject(response)}");

                            if (response != null)
                            {
                                await S3Helper.CreateFile(new S3CreateFileRequest
                                {
                                    FilePath = MTGServiceConstants.AUTHENTICATE_JSON_FILEPATH,
                                    Content = JsonConvert.SerializeObject(response)
                                });
                            }
                        }
                    }
                };
            }
            catch (Exception exp)
            {
                LambdaLogger.Log($"Exception: {exp}");
                throw;
            }

            LambdaLogger.Log($"Leaving: {JsonConvert.SerializeObject(response)}");

            return response;
        }
        #endregion

        #region Search
        /// <summary>
        /// Combines searching for products and details for the products
        /// returned from the search.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<TCGProductResponse> Search(TCGSearchRequest request)
        {
            LambdaLogger.Log($"Entering: Search({ JsonConvert.SerializeObject(request) })");

            TCGProductResponse response = new TCGProductResponse
            {
                success = false,
                errors = new List<string> { { "Default Error: Not set from TCG responses." } },
                results = new List<TCGProduct>()
            };

            try
            {
                TCGSearchResponse searchResponse = await SearchCategoryProducts(request);

                if (searchResponse == null)
                    throw new Exception("TCG Search response was null...");

                if (searchResponse.results.Count == 0)
                    throw new Exception("TCG Search response contains no results...");

                TCGProductRequest productRequest = new TCGProductRequest
                {
                    productIds = searchResponse.results
                };

                response = await GetProductDetails(productRequest);
            }
            catch(Exception exp)
            {
                LambdaLogger.Log($"Error: {exp}");
                throw;
            }

            LambdaLogger.Log($"Leaving: Search({ JsonConvert.SerializeObject(response) })");

            return response;
        }

        /// <summary>
        /// Searches for cards based on given filters
        /// </summary>
        /// <param name="request"></param>
        /// returns => results: productIds
        public async Task<TCGSearchResponse> SearchCategoryProducts(TCGSearchRequest request)
        {
            LambdaLogger.Log($"Entering: SearchCategoryProducts({ JsonConvert.SerializeObject(request) })");

            TCGSearchResponse response = null;

            //Need to validate TCGSearchRequest
            try
            {
                string url = $"{TCGConstants.SEARCH_URL}";

                StringContent content = new StringContent(JsonConvert.SerializeObject(request));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                using (HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Post, url))
                {
                    httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken());

                    httpRequest.Content = content;

                    LambdaLogger.Log($"Http Request: {JsonConvert.SerializeObject(httpRequest)}");

                    HttpResponseMessage httpResponse = await httpClient.SendAsync(httpRequest);

                    LambdaLogger.Log($"Http Response: {JsonConvert.SerializeObject(httpResponse)}");

                    if (httpResponse != null)
                    {
                        if (httpResponse.IsSuccessStatusCode)
                        {
                            string httpResponseContent = await httpResponse.Content.ReadAsStringAsync();

                            LambdaLogger.Log($"Http Response Content: {httpResponseContent}");

                            response = JsonConvert.DeserializeObject<TCGSearchResponse>(httpResponseContent);

                            LambdaLogger.Log($"SearchCategoryProducts Response: {JsonConvert.SerializeObject(response)}");

                            if (!response.success)
                                throw new Exception($"TCG Error(s): { string.Join(" - ", response.errors) }");
                        }
                    }
                }
            }
            catch(Exception exp)
            {
                LambdaLogger.Log($"Error: {exp}");
                throw;
            }

            LambdaLogger.Log($"Leaving: SearchCategoryProducts({ JsonConvert.SerializeObject(response) })");

            return response;
        }

        /// <summary>
        /// Gets card details from given productIds
        /// </summary>
        /// <param name="request"></param>
        /// returns => results: List of card details
        public async Task<TCGProductResponse> GetProductDetails(TCGProductRequest request)
        {
            LambdaLogger.Log($"Entering: GetProductDetails({ JsonConvert.SerializeObject(request) })");

            TCGProductResponse response = null;
            try
            {
                //Need to validate the product request here as well!!!
                if (request == null)
                    throw new Exception("Product Details request is null...");

                if (request.productIds.Count == 0)
                    throw new Exception("Product Details request contains no product ids.");

                string url = string.Format(TCGConstants.GET_PRODUCT_URL_TEMPLATE, string.Join(',', request.productIds));

                using (HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Get, url))
                {
                    httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken());

                    LambdaLogger.Log($"Http Request: {JsonConvert.SerializeObject(httpRequest)}");

                    HttpResponseMessage httpResponse = await httpClient.SendAsync(httpRequest);

                    LambdaLogger.Log($"Http Response: {JsonConvert.SerializeObject(httpResponse)}");

                    if (httpResponse != null)
                    {
                        if (httpResponse.IsSuccessStatusCode)
                        {
                            string httpResponseContent = await httpResponse.Content.ReadAsStringAsync();

                            LambdaLogger.Log($"Http Response Content: {httpResponseContent}");

                            response = JsonConvert.DeserializeObject<TCGProductResponse>(httpResponseContent);

                            LambdaLogger.Log($"SearchCategoryProducts Response: {JsonConvert.SerializeObject(response)}");

                            if (!response.success)
                                throw new Exception($"TCG Error(s): { string.Join(" - ", response.errors) }");
                        }
                    }
                }
            }
            catch(Exception exp)
            {
                LambdaLogger.Log($"Error: {exp}");
                throw;
            }

             LambdaLogger.Log($"Leaving: GetProductDetails({ JsonConvert.SerializeObject(request) })");

            return response;
        }
        #endregion
    }
}