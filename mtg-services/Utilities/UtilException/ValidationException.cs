﻿using System;
using System.Collections.Generic;
using System.Text;

namespace mtg_services.Utilities.UtilException
{
    public class ValidationException : FormatException
    {
        public ValidationException()
        {
            
        }

        public ValidationException(string message) : base(message)
        {

        }

        public ValidationException(string message, FormatException inner) : base(message, inner)
        {

        }
    }
}
