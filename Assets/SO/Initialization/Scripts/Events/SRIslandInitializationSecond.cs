
using Leopotam.EcsLite;

namespace SO.Initialization
{
    public readonly struct SRIslandInitializationSecond
    {
        public SRIslandInitializationSecond(
            EcsPackedEntity ownerAgentPE)
        {
            this.ownerAgentPE = ownerAgentPE;
        }

        public readonly EcsPackedEntity ownerAgentPE;
    }
}
