using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTGLambda.MTGLambda.DataClass.MTGLambdaCard
{
    public abstract class Card
    {
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
