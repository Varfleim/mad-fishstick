
using System.Collections.Generic;

using UnityEngine;

using Leopotam.EcsLite;

namespace GS.Agent
{
    public class AgentData : MonoBehaviour
    {
        internal Dictionary<Color, EcsPackedEntity> agentUniqueColors = new();
        public static string agentObjectType;
        internal List<Color> agentColors = new();
        internal List<EcsPackedEntity> agentColorPEs = new();
    }
}
