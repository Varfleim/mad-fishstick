
using Leopotam.EcsLite;

namespace SO.Colonization
{
    public readonly struct RLandColonize
    {
        public RLandColonize(
            EcsPackedEntity newOwnerPE,
            EcsPackedEntity landPE)
        {
            this.newOwnerPE = newOwnerPE;

            this.landPE = landPE;
        }

        public readonly EcsPackedEntity newOwnerPE;

        public readonly EcsPackedEntity landPE;
    }
}
