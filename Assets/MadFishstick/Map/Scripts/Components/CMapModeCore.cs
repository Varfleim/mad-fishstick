
using System.Collections.Generic;

using UnityEngine;

using Leopotam.EcsLite;

namespace MF.Map
{
    public struct CMapModeCore
    {
        public CMapModeCore(
            EcsPackedEntity selfPE, string selfName, 
            List<Color> colors)
        {
            this.selfPE = selfPE;
            this.selfName = selfName;

            this.colors = colors;
        }

        public readonly EcsPackedEntity selfPE;
        public readonly string selfName;

        public readonly List<Color> colors;

        public Color GetProvinceColor(
            ref CProvinceRender pR)
        {
            return colors[pR.ProvinceColorIndex];
        }
    }
}
