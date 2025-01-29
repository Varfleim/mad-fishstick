
using Leopotam.EcsLite;

namespace SO.LandOwnership
{
    public readonly struct RLandChangeOwner
    {
        public RLandChangeOwner(
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
