
namespace MF.Input
{
    public readonly struct RCameraMoving
    {
        public RCameraMoving(
            bool isHorizontal, bool isVertical, bool isZoom,
            float value)
        {
            this.isHorizontal = isHorizontal;
            this.isVertical = isVertical;
            this.isZoom = isZoom;

            this.value = value;
        }

        public readonly bool isHorizontal;
        public readonly bool isVertical;
        public readonly bool isZoom;

        public readonly float value;
    }
}
