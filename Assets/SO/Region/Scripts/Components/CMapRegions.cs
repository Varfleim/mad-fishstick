
using Leopotam.EcsLite;

namespace SO.Region
{
    public struct CMapRegions
    {
        public CMapRegions(int a)
        {
            regionPEs = new EcsPackedEntity[0];
        }

        public EcsPackedEntity[] regionPEs;

        public EcsPackedEntity GetRegion(
            int regionIndex)
        {
            return regionPEs[regionIndex];
        }

        /// <summary>
        /// Ќельз€ использовать в многопоточных системах
        /// </summary>
        /// <returns></returns>
        public EcsPackedEntity GetRegionRandom()
        {
            return GetRegion(UnityEngine.Random.Range(0, regionPEs.Length));
        }
    }
}
