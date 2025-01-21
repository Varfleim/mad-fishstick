
namespace MF.Map
{
    public readonly struct SRMapModeCreation
    {
        public SRMapModeCreation(
            string name,
            bool defaultMapMode)
        {
            this.name = name;

            this.defaultMapMode = defaultMapMode;
        }

        public readonly string name;

        public readonly bool defaultMapMode;
    }
}
