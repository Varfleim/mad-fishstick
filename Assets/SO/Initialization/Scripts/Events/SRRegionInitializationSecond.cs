
using Leopotam.EcsLite;

namespace SO.Initialization
{
    public readonly struct SRRegionInitializationSecond
    {
        public SRRegionInitializationSecond(
            EcsPackedEntity ownerAgentPE)
        {
            this.ownerAgentPE = ownerAgentPE;
        }

        public readonly EcsPackedEntity ownerAgentPE;
    }
}
