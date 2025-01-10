
using Leopotam.EcsLite;

namespace MF.Input
{
    public readonly struct RMousePositionChange
    {
        public RMousePositionChange(
            bool isMouseOverMap, 
            EcsPackedEntity lastHitProvincePE)
        {
            this.isMouseOverMap = isMouseOverMap;
            
            this.lastHitProvincePE = lastHitProvincePE;
        }

        public readonly bool isMouseOverMap;

        public readonly EcsPackedEntity lastHitProvincePE;
    }
}
