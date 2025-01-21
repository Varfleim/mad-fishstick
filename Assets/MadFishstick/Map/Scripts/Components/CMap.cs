
using Leopotam.EcsLite;

namespace MF.Map
{
    public struct CMap
    {
        public CMap(
            EcsPackedEntity selfPE, int selfIndex, string selfName)
        {
            this.selfPE = selfPE;
            this.selfIndex = selfIndex;
            this.selfName = selfName;
            
            provincePEs = new EcsPackedEntity[0];
        }

        public readonly EcsPackedEntity selfPE;
        public readonly int selfIndex;
        public readonly string selfName;

        public EcsPackedEntity[] provincePEs;

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
    }
}
