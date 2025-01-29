
using Leopotam.EcsLite;

namespace SO.Initialization
{
    public readonly struct RRegionInitializationFirst
    {
        public RRegionInitializationFirst(
            EcsPackedEntity parentMapPE,
            EcsPackedEntity ownerAgentPE)
        {
            this.parentMapPE = parentMapPE;

            this.ownerAgentPE = ownerAgentPE;
        }

        public readonly EcsPackedEntity parentMapPE;

        public readonly EcsPackedEntity ownerAgentPE;
    }
}
