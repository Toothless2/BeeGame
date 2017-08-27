using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeeGame.Items
{
    public class Honey : Item
    {
        public new static int ID => 14;

        public override string ToString()
        {
            return $"{itemName} \\ {GetItemID()}";
        }

        public override string GetItemID()
        {
            return GetHashCode().ToString(); ;
        }

        public override int GetHashCode()
        {
            return ID;
        }
    }
}
