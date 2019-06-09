using Amazon;
using Amazon.Lambda.Core;
using Amazon.S3;
using Amazon.S3.Model;
using MTGLambda.MTGLambda.Helpers.S3.Constants;
using MTGLambda.MTGLambda.Helpers.S3.Dto;
using MTGLambda.MTGLambda.Helpers.SecretsManager;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MTGLambda.MTGLambda.Helpers.S3
{
    public static class S3Helper
    {
        private static AmazonS3Client S3Client;

        static S3Helper()
        {
            //S3Client = new AmazonS3Client(S3Constants.S3_KEY, S3Constants.S3_SECRET, RegionEndpoint.USEast1);

            string S3Key = SecretsManagerHelper.GetS3SecretValue("key");
            string S3Secret = SecretsManagerHelper.GetS3SecretValue("secret");

            S3Client = new AmazonS3Client(S3Key, S3Secret, RegionEndpoint.USEast1);
        }

        public static async Task<S3CreateFileResponse> CreateFile(S3CreateFileRequest request)
        {
            LambdaLogger.Log($"Entering: CreateFolder({JsonConvert.SerializeObject(request)})");

            S3CreateFileResponse response = new S3CreateFileResponse
            {
                S3FilePath = request.FilePath,
                IsSuccess = true
            };

            try
            {
                PutObjectRequest S3Request = new PutObjectRequest
                {
                    BucketName = S3Constants.BUCKET_NAME,
                    Key = request.FilePath,
                    ContentBody = request.Content
                };

                PutObjectResponse S3Response = await S3Client.PutObjectAsync(S3Request);

                if (S3Response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                {
                    response.IsSuccess = false;
                    response.ErrorMessage = string.Format("Status Code: {0}", S3Response.HttpStatusCode);
                }
            }
            catch(Exception exp)
            {
                LambdaLogger.Log($"Error: {exp}");

                response.IsSuccess = false;
                response.ErrorMessage = string.Format("Error: {0}", exp.Message);
            }

            LambdaLogger.Log($"Leaving: CreateFolder({JsonConvert.SerializeObject(response)})");

            return response;
        }

        public static async Task<S3GetFileResponse> GetFile(S3GetFileRequest request)
        {
            LambdaLogger.Log($"Entering: GetFile({JsonConvert.SerializeObject(request)}");

            S3GetFileResponse response = new S3GetFileResponse
            {
                FilePath = request.FilePath,
                IsSuccess = true,
                FileContent = string.Empty
            };

            try
            {
                GetObjectRequest S3Request = new GetObjectRequest
                {
                    BucketName = S3Constants.BUCKET_NAME,
                    Key = request.FilePath
                };

                GetObjectResponse S3Response = await S3Client.GetObjectAsync(S3Request);

                if (S3Response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                {
                    response.IsSuccess = false;
                    response.ErrorMessage = string.Format("Status Code: {0}", S3Response.HttpStatusCode);
                }
                else
                {
                    response.FilePath = S3Response.Key;
                    using (StreamReader reader = new StreamReader(S3Response.ResponseStream))
                    {
                        response.FileContent = reader.ReadToEnd();
                    }
                }
            }
            catch(Exception exp)
            {
                LambdaLogger.Log($"Error: {exp}");

                response.IsSuccess = false;
                response.ErrorMessage = string.Format("Error: {0}", exp.Message);
            }

            LambdaLogger.Log($"Leaving: GetFile({JsonConvert.SerializeObject(response)})");

            return response;
        }
    }
}
