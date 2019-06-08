using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTGLambda.MTGLambda.Helpers.DynamoDb.Dto
{
    public class ExpressionFilterObject
    {
        public Dictionary<string, string> ExpressionAttributeNames { get; set; }
        public Dictionary<string, AttributeValue> ExpressionAttributeValues { get; set; }
        public List<Dictionary<string, string>> ExpressionGeneratorList { get; set; }

        public ExpressionFilterObject()
        {
            ExpressionAttributeNames = new Dictionary<string, string>();
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>();
            ExpressionGeneratorList = new List<Dictionary<string, string>>();
        }
    }
}
