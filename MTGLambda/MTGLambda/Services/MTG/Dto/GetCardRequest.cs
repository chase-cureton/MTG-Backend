using System;
using System.Collections.Generic;
using System.Text;

namespace MTGLambda.MTGLambda.Services.MTG.Dto
{
    public class GetCardRequest
    {
        public string NameFilter { get; set; }

        public string TextFilter { get; set; }

        public string KeywordsFilter { get; set; }

        public Dictionary<int, bool> ManaCostFilter { get; set; }

        public Dictionary<string, bool> BaseTypeFilter { get; set; }

        /// <summary>
        /// Nullable bool gives 3 options
        /// Null: Don't care about the color
        /// False: !Contains color
        /// True: Contains color
        /// </summary>
        public Dictionary<string, bool?> ColorFilter { get; set; }

        public GetCardRequest()
        {
            ManaCostFilter = new Dictionary<int, bool>();
            BaseTypeFilter = new Dictionary<string, bool>();
            ColorFilter = new Dictionary<string, bool?>();
        }
    }
}
