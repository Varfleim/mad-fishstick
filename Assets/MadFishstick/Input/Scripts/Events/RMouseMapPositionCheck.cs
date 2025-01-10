
using Leopotam.EcsLite;

namespace MF.Input
{
    public readonly struct RMouseMapPositionCheck
    {
        public RMouseMapPositionCheck(
            EcsPackedEntity currentProvincePE)
        {
            this.currentProvincePE = currentProvincePE;
        }

        public readonly EcsPackedEntity currentProvincePE;
    }
}
