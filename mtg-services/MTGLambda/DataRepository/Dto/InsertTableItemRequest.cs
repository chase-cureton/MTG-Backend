using System;
using System.Collections.Generic;
using System.Text;

namespace MTGLambda.MTGLambda.DataRepository.Dto
{
    public class InsertTableItemRequest
    {
        public string Table { get; set; }
        public string Filter { get; set; }
        public string OrderByExpression { get; set; }
        public int RecordCap { get; set; }

        public string Content { get; set; }
    }
}
