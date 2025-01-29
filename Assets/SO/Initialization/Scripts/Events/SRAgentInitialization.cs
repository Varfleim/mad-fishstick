
using Leopotam.EcsLite;

namespace SO.Initialization
{
    internal struct SRAgentInitialization
    {
        public SRAgentInitialization(
            string agentName,
            EcsPackedEntity parentMapPE = new())
        {
            this.agentName = agentName;

            this.parentMapPE = parentMapPE;
        }

        public readonly string agentName;

        public readonly EcsPackedEntity parentMapPE;
    }
}
