using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeeGame.Exceptipns
{
    public class InputException : Exception
    {
        public InputException() : base()
        {

        }

        public InputException(string message) : base(message)
        {

        }

        public InputException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
