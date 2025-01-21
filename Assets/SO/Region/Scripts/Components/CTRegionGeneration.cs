
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
            outerProvincePEs = new();

            provinceWithoutFreeNeighboursCount = 0;
        }

        public List<EcsPackedEntity> innerProvincePEs;
        public List<EcsPackedEntity> outerProvincePEs;
        public int ProvinceTotalCount
        {
            get
            {
                return innerProvincePEs.Count + outerProvincePEs.Count;
            }
        }

        public int provinceWithoutFreeNeighboursCount;

        public List<EcsPackedEntity> GetAllProvinces()
        {
            List<EcsPackedEntity> allProvincePEs = new();

            allProvincePEs.AddRange(innerProvincePEs);
            allProvincePEs.AddRange(outerProvincePEs);

            return allProvincePEs;
        }
    }
}
