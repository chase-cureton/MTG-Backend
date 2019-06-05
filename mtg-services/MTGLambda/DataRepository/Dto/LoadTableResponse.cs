using Amazon.DynamoDBv2.DocumentModel;
using MTGLambda.MTGLambda.Helpers.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTGLambda.MTGLambda.DataRepository.Dto
{
    public class LoadTableResponse : Response
    {
        public Table ResponseTable { get; set; }
    }
}
