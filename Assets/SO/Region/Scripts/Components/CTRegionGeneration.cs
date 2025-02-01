
using System.Collections.Generic;

using Leopotam.EcsLite;

namespace SO.Region
{
    public struct CTRegionGeneration
    {
        public CTRegionGeneration(
            int a)
        {
            innerProvincePEs = new();
            outerProvinceWithFreeNeighboursPEs = new();
            outerProvinceWithoutFreeNeighboursPEs = new();

            provinceWithoutFreeNeighboursCount = 0;

            tempNeighbourRegionPEs = new();
            tempNeighbourProvincePEs = new();
        }

        public List<EcsPackedEntity> innerProvincePEs;
        public List<EcsPackedEntity> outerProvinceWithFreeNeighboursPEs;
        public List<EcsPackedEntity> outerProvinceWithoutFreeNeighboursPEs;

        public int ProvinceTotalCount
        {
            get
            {
                return innerProvincePEs.Count + outerProvinceWithFreeNeighboursPEs.Count + outerProvinceWithoutFreeNeighboursPEs.Count;
            }
        }

        public int provinceWithoutFreeNeighboursCount;

        public List<EcsPackedEntity> GetAllProvinces()
        {
            List<EcsPackedEntity> allProvincePEs = new();

            allProvincePEs.AddRange(innerProvincePEs);
            allProvincePEs.AddRange(outerProvinceWithFreeNeighboursPEs);
            allProvincePEs.AddRange(outerProvinceWithoutFreeNeighboursPEs);

            return allProvincePEs;
        }

        public HashSet<EcsPackedEntity> tempNeighbourRegionPEs;
        public HashSet<EcsPackedEntity> tempNeighbourProvincePEs;
    }
}
