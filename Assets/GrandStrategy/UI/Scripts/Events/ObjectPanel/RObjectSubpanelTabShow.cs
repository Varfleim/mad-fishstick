
using Leopotam.EcsLite;

namespace GS.UI
{
    public readonly struct RObjectSubpanelTabShow
    {
        public RObjectSubpanelTabShow(
            string objectSubpanelType, string objectSubpanelTabType,
            EcsPackedEntity objectPE)
        {
            this.objectSubpanelType = objectSubpanelType;
            this.objectSubpanelTabType = objectSubpanelTabType;


            this.objectPE = objectPE;
        }

        public readonly string objectSubpanelType;
        public readonly string objectSubpanelTabType;

        public readonly EcsPackedEntity objectPE;
    }
}
