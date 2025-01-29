
using System.Collections.Generic;

using UnityEngine;

namespace MF.Map
{
    public readonly struct RMapModeUpdateColorsListFirst
    {
        public RMapModeUpdateColorsListFirst(
            string coloredObjectType, 
            List<Color> objectColors)
        {
            this.coloredObjectType = coloredObjectType;
            
            this.objectColors = objectColors;
        }

        public readonly string coloredObjectType;

        public readonly List<Color> objectColors;
    }
}
