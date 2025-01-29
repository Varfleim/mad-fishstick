
namespace SO.Initialization
{
    internal struct SRMapInitialization
    {
        public SRMapInitialization(
            string mapName,
            int hexasphereSubdivisions,
            int averageProvincesPerRegion)
        {
            this.mapName = mapName;

            this.hexasphereSubdivisions = hexasphereSubdivisions;

            this.averageProvincesPerRegion = averageProvincesPerRegion;
        }

        public readonly string mapName;

        public readonly int hexasphereSubdivisions;

        public readonly int averageProvincesPerRegion;
    }
}
