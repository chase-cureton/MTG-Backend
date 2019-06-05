using Amazon.DynamoDBv2.DocumentModel;
using MTGLambda.MTGLambda.Helpers.Common;
using System.Collections.Generic;

namespace MTGLambda.MTGLambda.DataRepository.Dao
{
    public class LoadTableItemsResponse : Response
    {
        public List<Document> TableItems { get; set; }
    }
}