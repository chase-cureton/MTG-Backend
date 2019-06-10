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

namespace MTGLambda.MTGLambda.Helpers.SecretsManager
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
            LambdaLogger.Log($"Entering: GetS3SecretValue({ type })");
            string response = string.Empty;

            GetSecretValueRequest request = new GetSecretValueRequest
            {
                SecretId = "arn:aws:secretsmanager:us-east-1:542966453150:secret:AWS_Access-nBXyBQ",
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
                LambdaLogger.Log($"Secret String Response: { secretResponse.SecretString }");

                var secret = JsonConvert.DeserializeObject<Dictionary<string, string>>(secretResponse.SecretString);

                response = secret[type];
            }
            else
            {
                StreamReader reader = new StreamReader(secretResponse.SecretBinary);
                var secretString = Encoding.UTF8.GetString(Convert.FromBase64String(reader.ReadToEnd()));

                var secret = JsonConvert.DeserializeObject<Dictionary<string, string>>(secretString);

                response = secret[type];

                LambdaLogger.Log($"Secret Binary Response: { response }");
            }

            LambdaLogger.Log($"Leaving: GetS3SecretValue({response})");
            return response;
        }
    }
}
