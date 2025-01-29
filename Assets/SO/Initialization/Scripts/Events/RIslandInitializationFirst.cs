
using Leopotam.EcsLite;

namespace SO.Initialization
{
    public readonly struct RIslandInitializationFirst
    {
        public RIslandInitializationFirst(
            EcsPackedEntity parentRegionPE,
            EcsPackedEntity ownerAgentPE)
        {
            this.parentRegionPE = parentRegionPE;

            this.ownerAgentPE = ownerAgentPE;
        }

        public readonly EcsPackedEntity parentRegionPE;

        public readonly EcsPackedEntity ownerAgentPE;
    }
}
