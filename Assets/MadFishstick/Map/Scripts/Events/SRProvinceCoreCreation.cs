
using Leopotam.EcsLite;

namespace MF.Map
{
    public readonly struct SRProvinceCoreCreation
    {
        public SRProvinceCoreCreation(
            EcsPackedEntity parentMapPE,
            EcsPackedEntity[] neighbourProvincePEs)
        {
            this.parentMapPE = parentMapPE;

            this.neighbourProvincePEs = neighbourProvincePEs;
        }

        public readonly EcsPackedEntity parentMapPE;

        public readonly EcsPackedEntity[] neighbourProvincePEs;
    }
}
