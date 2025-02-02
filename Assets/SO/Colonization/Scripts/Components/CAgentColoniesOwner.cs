
using System.Collections.Generic;

using Leopotam.EcsLite;

namespace SO.Colonization
{
    public struct CAgentColoniesOwner
    {
        public CAgentColoniesOwner(
            EcsPackedEntity selfPE)
        {
            this.selfPE = selfPE;

            ownedColonyPEs = new();
        }

        public readonly EcsPackedEntity selfPE;

        public readonly HashSet<EcsPackedEntity> ownedColonyPEs;
    }
}
