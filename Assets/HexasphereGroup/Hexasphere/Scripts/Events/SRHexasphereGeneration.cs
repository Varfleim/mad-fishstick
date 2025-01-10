namespace HS
{
    public readonly struct SRHexasphereGeneration
    {
        public SRHexasphereGeneration(
            int mapIndex,
            int subdivisions)
        {
            this.mapIndex = mapIndex;

            this.subdivisions = subdivisions;
        }

        public readonly int mapIndex;

        public readonly int subdivisions;
    }
}
