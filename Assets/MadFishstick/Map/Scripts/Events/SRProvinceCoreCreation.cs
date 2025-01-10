
using Leopotam.EcsLite;

namespace MF.Map
{
    public readonly struct SRProvinceCoreCreation
    {
        public SRProvinceCoreCreation(
            int parentMapIndex,
            EcsPackedEntity[] neighbourProvincePEs)
        {
            this.parentMapIndex = parentMapIndex;

            this.neighbourProvincePEs = neighbourProvincePEs;
        }

        public readonly int parentMapIndex;

        public readonly EcsPackedEntity[] neighbourProvincePEs;
    }
}
