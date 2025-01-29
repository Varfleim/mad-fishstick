
using System.Collections.Generic;

using Leopotam.EcsLite;

namespace SO.LandOwnership
{
    public struct CAgentLandOwner
    {
        public CAgentLandOwner(
            EcsPackedEntity selfPE)
        {
            this.selfPE = selfPE;

            ownedLandPEs = new();
        }

        public readonly EcsPackedEntity selfPE;

        public readonly HashSet<EcsPackedEntity> ownedLandPEs;
    }
}
