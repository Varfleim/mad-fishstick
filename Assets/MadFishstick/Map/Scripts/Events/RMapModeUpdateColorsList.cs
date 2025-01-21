
using System.Collections.Generic;

using UnityEngine;

using Leopotam.EcsLite;

namespace MF.Map
{
    public readonly struct RMapModeUpdateColorsList
    {
        public RMapModeUpdateColorsList(
            EcsPackedEntity mapModePE, 
            List<Color> mapModeColors)
        {
            this.mapModePE = mapModePE;
            
            this.mapModeColors = mapModeColors;
        }

        public readonly EcsPackedEntity mapModePE;

        public readonly List<Color> mapModeColors;
    }
}
