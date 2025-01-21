
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
    }
}
