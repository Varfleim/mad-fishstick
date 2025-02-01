
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
            firstOuterProvinceIndex = 0;

            neighbourRegionPEs = new EcsPackedEntity[0];
            neighbourProvincePEs = new EcsPackedEntity[0];
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
        public int firstOuterProvinceIndex;

        public EcsPackedEntity GetProvince(
            int provinceIndex)
        {
            return provincePEs[provinceIndex];
        }

        /// <summary>
        /// Ќельз€ использовать в многопоточных системах
        /// </summary>
        /// <returns></returns>
        public EcsPackedEntity GetProvinceRandom()
        {
            return GetProvince(UnityEngine.Random.Range(0, provincePEs.Length));
        }

        public EcsPackedEntity[] neighbourRegionPEs;
        public EcsPackedEntity[] neighbourProvincePEs;
    }
}
