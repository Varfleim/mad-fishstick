
using Leopotam.EcsLite;

namespace SO.Island
{
    public struct CIsland
    {
        public CIsland(
            EcsPackedEntity selfPE, string selfName)
        {
            this.selfPE = selfPE;
            this.selfName = selfName;
        }

        public readonly EcsPackedEntity selfPE;
        public readonly string selfName;
    }
}
