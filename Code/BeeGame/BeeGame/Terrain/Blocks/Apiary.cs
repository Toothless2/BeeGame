using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BeeGame.Terrain.Blocks
{
    public class Apiary : Block
    {
        public Apiary() : base() { }

        public Apiary(SerializationInfo info, StreamingContext context)
        {
            //use info.getvalue("valuename", typeof(valueType))
            UnityEngine.MonoBehaviour.print("hi");
        }
    }
}
