
using Leopotam.EcsLite;

namespace MF.Map
{
    public readonly struct SRSetMapRenderValues
    {
        public SRSetMapRenderValues(
            EcsPackedEntity displayedObjectPE,
            float height, 
            int colorIndex)
        {
            this.displayedObjectPE = displayedObjectPE;

            this.height = height;
            
            this.colorIndex = colorIndex;
        }

        public readonly EcsPackedEntity displayedObjectPE;

        public readonly float height;

        public readonly int colorIndex;
    }
}
