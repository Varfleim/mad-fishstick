
using Leopotam.EcsLite;

namespace SO.Region
{
    public struct CTProvinceRegionNeighbours
    {
        public CTProvinceRegionNeighbours(
            EcsPackedEntity selfPE)
        {
            this.selfPE = selfPE;
            
            //parentRegionPE = new();

            ownedNeighboursCount = 0;
        }

        public readonly EcsPackedEntity selfPE;

        //public EcsPackedEntity parentRegionPE;

        public int ownedNeighboursCount;
    }
}
