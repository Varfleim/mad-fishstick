
namespace SO.Region
{
    public readonly struct SRRegionsGeneration
    {
        public SRRegionsGeneration(
            int averageProvincesPerRegion)
        {
            this.averageProvincesPerRegion = averageProvincesPerRegion;
        }

        public readonly int averageProvincesPerRegion;
    }
}
