using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeeGame.Exceptipns
{
    public class CraftingRecipieAdditionException : Exception
    {
        public CraftingRecipieAdditionException() : base()
        {

        }

        public CraftingRecipieAdditionException(string message) : base(message)
        {

        }

        public CraftingRecipieAdditionException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
