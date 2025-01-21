
using Leopotam.EcsLite;

namespace SO.Region
{
    public struct CTProvinceRegionOwner
    {
        public CTProvinceRegionOwner(
            EcsPackedEntity parentRegionPE)
        {
            this.parentRegionPE = parentRegionPE;
        }

        public EcsPackedEntity parentRegionPE;
    }
}
