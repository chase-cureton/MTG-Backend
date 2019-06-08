using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTGLambda.MTGLambda.DataRepository.Dto
{
    public class UpsertTableItemsRequest
    {
        public string Table { get; set; }
        public List<Document> Documents { get; set; }

        public UpsertTableItemsRequest()
        {
            Documents = new List<Document>();
        }
    }
}
