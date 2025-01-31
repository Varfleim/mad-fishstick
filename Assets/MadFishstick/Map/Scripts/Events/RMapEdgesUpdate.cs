
namespace MF.Map
{
    public readonly struct RMapEdgesUpdate
    {
        public RMapEdgesUpdate(
            bool isThinUpdated, bool isThickUpdated, bool isCurrentHighlightUpdated)
        {
            this.isThinUpdated = isThinUpdated;
            this.isThickUpdated = isThickUpdated;
            this.isCurrentHighlightUpdated = isCurrentHighlightUpdated;
        }

        public readonly bool isThinUpdated;
        public readonly bool isThickUpdated;
        public readonly bool isCurrentHighlightUpdated;
    }
}
