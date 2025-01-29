
using Leopotam.EcsLite;

namespace SO.LandOwnership
{
    public struct CLandOwned
    {
        public CLandOwned(
            EcsPackedEntity selfPE)
        {
            this.selfPE = selfPE;

            ownerPE = new();
        }

        public readonly EcsPackedEntity selfPE;

        public EcsPackedEntity ownerPE;
    }
}
