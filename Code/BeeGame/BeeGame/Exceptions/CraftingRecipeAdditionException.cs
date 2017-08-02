using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeeGame.Exceptions
{
    public class CraftingRecipeAdditionException : Exception
    {
        public CraftingRecipeAdditionException() : base()
        {

        }

        public CraftingRecipeAdditionException(string message) : base(message)
        {

        }

        public CraftingRecipeAdditionException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
