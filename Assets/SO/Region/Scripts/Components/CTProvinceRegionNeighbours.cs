
using System.Collections.Generic;

using Leopotam.EcsLite;

namespace SO.Region
{
    public struct CTProvinceRegionNeighbours
    {
        public CTProvinceRegionNeighbours(
            EcsPackedEntity selfPE)
        {
            this.selfPE = selfPE;

            ownedNeighboursCount = 0;
        }

        public readonly EcsPackedEntity selfPE;

        public int ownedNeighboursCount;
    }
}
