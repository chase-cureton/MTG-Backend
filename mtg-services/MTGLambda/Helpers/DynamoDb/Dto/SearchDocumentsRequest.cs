using System;
using System.Collections.Generic;
using System.Text;

namespace MTGLambda.MTGLambda.Helpers.DynamoDb.Dto
{
    public class SearchDocumentsRequest
    {
        public string TableName { get; set; }
        public Dictionary<string, List<string>> KeyFilters { get; set; }

        public SearchDocumentsRequest()
        {
            KeyFilters = new Dictionary<string, List<string>>();
        }
    }
}
