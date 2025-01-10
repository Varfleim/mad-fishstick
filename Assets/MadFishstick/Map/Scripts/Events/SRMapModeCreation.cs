
using System.Collections.Generic;

using UnityEngine;

namespace MF.Map
{
    public readonly struct SRMapModeCreation
    {
        public SRMapModeCreation(
            string name,
            List<Color> colors,
            bool defaultMapMode)
        {
            this.name = name;

            this.colors = colors;

            this.defaultMapMode = defaultMapMode;
        }

        public readonly string name;

        public readonly List<Color> colors;

        public readonly bool defaultMapMode;
    }
}
