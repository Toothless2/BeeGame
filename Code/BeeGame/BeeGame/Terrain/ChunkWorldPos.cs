using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeeGame.Terrain
{
    [Serializable]
    public struct ChunkWorldPos
    {
        public int x, y, z;

        public ChunkWorldPos(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override string ToString()
        {
            return $"({x}, {y}, {z})";
        }

        //TODO probly add the == and != but for now this is fine
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2231:OverloadOperatorEqualsOnOverridingValueTypeEquals")]
        public override bool Equals(object obj)
        {
            if (!(obj is ChunkWorldPos))
                return false;
            ChunkWorldPos temp = (ChunkWorldPos)obj;

            if (temp.x == x && temp.y == y && temp.z == z)
                return true;

            //if (obj.GetHashCode() == GetHashCode())
            //    return true; 

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashcode = 47;

                hashcode *= 227 + x.GetHashCode();
                hashcode *= 227 + y.GetHashCode();
                hashcode *= 227 + z.GetHashCode();

                return hashcode;
            }
        }
    }
}
