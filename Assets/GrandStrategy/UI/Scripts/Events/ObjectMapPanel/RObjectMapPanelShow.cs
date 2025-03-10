
using Leopotam.EcsLite;

namespace GS.UI
{
    public readonly struct RObjectMapPanelShow
    {
        public RObjectMapPanelShow(
            string objectMapPanelType, 
            EcsPackedEntity objectPE)
        {
            this.objectMapPanelType = objectMapPanelType;
            
            this.objectPE = objectPE;
        }

        public readonly string objectMapPanelType;

        public readonly EcsPackedEntity objectPE;
    }
}
