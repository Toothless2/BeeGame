using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeeGame.Items
{
    /// <summary>
    /// Does this need to exist?
    /// </summary>
    [Serializable]
    public abstract class AbstractItem
    {
        public abstract string GetItemName();
        public abstract string GetItemID();
        public abstract override int GetHashCode();
    }
}
