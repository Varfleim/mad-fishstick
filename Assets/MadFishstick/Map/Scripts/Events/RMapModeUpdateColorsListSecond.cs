
using System.Collections.Generic;

using UnityEngine;

using Leopotam.EcsLite;

namespace MF.Map
{
    public readonly struct RMapModeUpdateColorsListSecond
    {
        public RMapModeUpdateColorsListSecond(
            EcsPackedEntity mapModePE, 
            List<Color> mapModeColors, Color defaultColor)
        {
            this.mapModePE = mapModePE;
            
            this.mapModeColors = mapModeColors;

            this.defaultColor = defaultColor;
        }

        public readonly EcsPackedEntity mapModePE;

        public readonly List<Color> mapModeColors;

        public readonly Color defaultColor;
    }
}
