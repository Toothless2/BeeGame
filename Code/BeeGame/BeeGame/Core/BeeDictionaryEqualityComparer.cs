using System.Collections.Generic;
using BeeGame.Enums;
using System.Linq;

namespace BeeGame.Core
{
    public class BeeDictionaryEqualityComparer : IEqualityComparer<BeeSpecies[]>
    {
        public bool Equals(BeeSpecies[] x, BeeSpecies[] y)
        {
            if(x.Contains(y[0]) && x.Contains(y[1]))
            {
                return true;
            }

            if(x.Length != y.Length)
            {
                return false;
            }
            
            if(x.Intersect(y).Count() == x.Length)
            {
                return true;
            }

            return false;
        }

        public int GetHashCode(BeeSpecies[] obj)
        {
            unchecked
            {
                int hashcode = 13;

                for(int i = 0; i < obj.Length; i++)
                {
                    hashcode += (int)obj[i];
                }

                return hashcode;
            }
        }
    }
}