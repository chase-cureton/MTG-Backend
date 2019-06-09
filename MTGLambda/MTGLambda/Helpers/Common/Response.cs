using System;
using System.Collections.Generic;
using System.Text;

namespace MTGLambda.MTGLambda.Helpers.Common
{
    public abstract class Response
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }
}
