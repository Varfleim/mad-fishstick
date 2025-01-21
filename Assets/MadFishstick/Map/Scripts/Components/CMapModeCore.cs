
using System.Collections.Generic;

using UnityEngine;

using Leopotam.EcsLite;

namespace MF.Map
{
    public struct CMapModeCore
    {
        public CMapModeCore(
            EcsPackedEntity selfPE, string selfName)
        {
            this.selfPE = selfPE;
            this.selfName = selfName;

            this.colors = new();
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
