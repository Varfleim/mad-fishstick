
using System.Collections.Generic;

using UnityEngine;

using Leopotam.EcsLite;

namespace SO.Region
{
    public class MapModeData : MonoBehaviour
    {
        public string regionMapModeName;
        public EcsPackedEntity regionMapModePE;
        public Color regionMapModeDefaultColor;
        public Dictionary<Color, EcsPackedEntity> regionMapModeUniqueColors = new();
        public List<Color> regionMapModeColors = new();
        public List<EcsPackedEntity> regionMapModePEs = new();
    }
}
