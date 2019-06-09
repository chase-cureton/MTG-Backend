using Amazon.DynamoDBv2.DocumentModel;
using MTGLambda.MTGLambda.Helpers.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTGLambda.MTGLambda.DataRepository.Dto
{
    public class UpsertTableItemResponse : Response
    {
        public List<Document> Documents { get; set; }

        public UpsertTableItemResponse()
        {
            Documents = new List<Document>();
        }
    }
}
