using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTGLambda.MTGLambda.DataRepository.Dto
{
    public class LoadTableItemsRequest
    {
        public string Table { get; set; }
        public Type Type { get; set; }
        public string Filter { get; set; }
        public List<ScanCondition> Conditions { get; set; }
        public string OrderByExpression { get; set; }
        public int RecordCap { get; set; }
        public List<string> Properties { get; set; } //Will implement return of specific properties later

        public LoadTableItemsRequest()
        {
            Conditions = new List<ScanCondition>();
            Properties = new List<string>();
        }
    }
}
