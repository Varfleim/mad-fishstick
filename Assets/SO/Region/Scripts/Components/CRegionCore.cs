
using Leopotam.EcsLite;

namespace SO.Region
{
    public struct CRegionCore
    {
        public CRegionCore(
            EcsPackedEntity selfPE,
            EcsPackedEntity parentMapPE)
        {
            this.selfPE = selfPE;

            this.parentMapPE = parentMapPE;

            colorIndex = -1;

            provincePEs = new EcsPackedEntity[0];
        }

        public readonly EcsPackedEntity selfPE;

        public readonly EcsPackedEntity parentMapPE;

        public int ColorIndex
        {
            get
            {
                return colorIndex;
            }
        }
        int colorIndex;

        public void SetColorIndex(
            int value)
        {
            colorIndex = value;
        }

        public EcsPackedEntity[] provincePEs;
    }
}
