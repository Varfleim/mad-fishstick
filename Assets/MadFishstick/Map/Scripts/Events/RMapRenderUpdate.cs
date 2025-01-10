
namespace MF.Map
{
    public readonly struct RMapRenderUpdate
    {
        public RMapRenderUpdate(
            bool isMaterialUpdated, bool isHeightUpdated, bool isColorUpdated)
        {
            this.isMaterialUpdated = isMaterialUpdated;
            this.isHeightUpdated = isHeightUpdated;
            this.isColorUpdated = isColorUpdated;
        }

        public readonly bool isMaterialUpdated;
        public readonly bool isHeightUpdated ;
        public readonly bool isColorUpdated;
    }
}
