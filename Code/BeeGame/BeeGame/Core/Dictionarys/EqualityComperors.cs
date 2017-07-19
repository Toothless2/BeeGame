using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeeGame.Core.Enums;

namespace BeeGame.Core.Dictionarys
{
    public class BeeCombinationDictionaryEqualityComparer : IEqualityComparer<BeeSpecies[]>
    {
        public bool Equals(BeeSpecies[] x, BeeSpecies[] y)
        {
            if (x.Contains(y[0]) && x.Contains(y[1]))
            {
                //* if the x length is greater than 2 this means that the combination can have duplicate bees for a product
                if (x.Length > 2)
                    return true;

                //* if 1 means both y elements are the same so no combination has been found 
                if(y.Intersect(x).Count() <= 1)
                    return false;

                return true;
            }

            return false;
        }

        public int GetHashCode(BeeSpecies[] obj)
        {
            unchecked
            {
                int hashcode = 13;

                for (int i = 0; i < obj.Length; i++)
                {
                    hashcode += (int)obj[i];
                }

                return hashcode;
            }
        }
    }
}
