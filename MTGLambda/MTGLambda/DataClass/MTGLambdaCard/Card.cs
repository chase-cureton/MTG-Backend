using MTGLambda.MTGLambda.Helpers.Common;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MTGLambda.MTGLambda.DataClass.MTGLambdaCard
{
    public abstract class Card
    {
        [PrimaryKey]
        public virtual string Name { get; set; }
        public virtual List<string> Colors { get; set; }
        public virtual string ManaCost { get; set; }
        public virtual string CardText { get; set; }
        public virtual string Type { get; set; }

        public virtual List<string> Keywords { get; set; }

        public virtual string DescribeOverview()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
