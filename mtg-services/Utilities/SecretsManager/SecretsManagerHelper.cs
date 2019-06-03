using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.Lambda.Core;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Newtonsoft.Json;

namespace mtg_services.Utilities.SecretsManager
{
    public static class SecretsManagerHelper
    {
        private static AmazonSecretsManagerClient client;

        static SecretsManagerHelper()
        {
            string region = "us-east-1";
            client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region));
        }

        public static string GetS3SecretValue(string type)
        {
            LambdaLogger.Log($"Entering: GetS3SecretValue({ type }");
            string response = string.Empty;

            GetSecretValueRequest request = new GetSecretValueRequest
            {
                SecretId = type == "key" ? "S3_Key" : "S3_Secret",
                VersionStage = "AWSCURRENT"
            };

            GetSecretValueResponse secretResponse = null;

            try
            {
                secretResponse = client.GetSecretValueAsync(request).Result;

                LambdaLogger.Log($"Amazon Secret Value Response: { JsonConvert.SerializeObject(secretResponse) }");
            }
            catch(Exception exp)
            {
                LambdaLogger.Log($"Secret Request Error: { exp }");
                throw;
            }

            if (!string.IsNullOrWhiteSpace(secretResponse.SecretString))
            {
                response = secretResponse.SecretString;

                LambdaLogger.Log($"Secret String Response: { response }");
            }
            else
            {
                StreamReader reader = new StreamReader(secretResponse.SecretBinary);
                response = Encoding.UTF8.GetString(Convert.FromBase64String(reader.ReadToEnd()));

                LambdaLogger.Log($"Secret Binary Response: { response }");
            }

            LambdaLogger.Log($"Leaving: GetS3SecretValue({response}");
            return response;
        }
    }
}
