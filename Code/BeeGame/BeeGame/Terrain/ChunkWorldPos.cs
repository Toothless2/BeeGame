using System;
using BeeGame.Core;

namespace BeeGame.Terrain
{
    /// <summary>
    /// Serializable int version of <see cref="THVector3"/>
    /// </summary>
    [Serializable]
    public struct ChunkWorldPos
    {
        /// <summary>
        /// x, y, z values for the vector
        /// </summary>
        public int x, y, z;

        /// <summary>
        /// Constructor so that values can be input on creation of the vector
        /// </summary>
        /// <param name="x">X Value</param>
        /// <param name="y">Y Value</param>
        /// <param name="z">Z Value</param>
        public ChunkWorldPos(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Formats the values nicely incase it is needed
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"({x}, {y}, {z})";
        }

        //*TODO probly add the == and != but for now this is fine
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2231:OverloadOperatorEqualsOnOverridingValueTypeEquals")]
        public override bool Equals(object obj)
        {
            //possibly remove and just check if obj is null
            if (!(obj is ChunkWorldPos))
                return false;

            ChunkWorldPos temp = (ChunkWorldPos)obj;

            //possibly change to hashcode checking
            if (temp.x == x && temp.y == y && temp.z == z)
                return true;

            return false;
        }

        /// <summary>
        /// Makes a unique hascode for the vector
        /// </summary>
        /// <returns>unique int value for the vector</returns>
        /// <remarks>
        /// Possible that 2 defferent values can give the same hashcode but chance of that happening and the vectors needing to be checked against each other is low
        /// </remarks>
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

        /// <summary>
        /// Converts a <see cref="ChunkWorldPos"/> to a <see cref="THVector3"/> without the need for an explicit cast as no data will be lost
        /// </summary>
        /// <param name="pos">this <see cref="ChunkWorldPos"/></param>
        public static implicit operator THVector3(ChunkWorldPos pos)
        {
            return new THVector3(pos.x, pos.y, pos.z);
        }

        /// <summary>
        /// Converts a <see cref="ChunkWorldPos"/> to a <see cref="THVector3"/>
        /// </summary>
        /// <param name="pos">A <see cref="THVector3"/></param>
        /// <remarks>
        /// Operator is explicit as data could be lost, <see cref="THVector3"/> is a float and <see cref="ChunkWorldPos"/> is a int
        /// </remarks>
        public static explicit operator ChunkWorldPos(THVector3 pos)
        {
            return new ChunkWorldPos((int)pos.x, (int)pos.y, (int)pos.z);
        }
    }
}
