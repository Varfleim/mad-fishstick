
namespace MF.Map
{
    public readonly struct RMapProvincesUpdate
    {
        public RMapProvincesUpdate(
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
