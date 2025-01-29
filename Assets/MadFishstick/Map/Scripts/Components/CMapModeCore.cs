
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

            colors = new();
            defaultColor = new Color();
        }

        public readonly EcsPackedEntity selfPE;
        public readonly string selfName;

        public readonly List<Color> colors;
        public Color defaultColor;

        public Color GetProvinceColor(
            ref CProvinceRender pR)
        {
            if(pR.ProvinceColorIndex != -1)
            {
                return colors[pR.ProvinceColorIndex];
            }
            else
            {
                return defaultColor;
            }
        }
    }
}
