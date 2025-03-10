
namespace GS.UI
{
    public readonly struct RObjectSubpanelTabUpdate
    {
        public RObjectSubpanelTabUpdate(
            bool isSamePanel, 
            bool isSameSubpanel, 
            bool isSameTab, 
            bool isSameObject)
        {
            this.isSamePanel = isSamePanel;
            this.isSameSubpanel = isSameSubpanel;
            this.isSameTab = isSameTab;
            this.isSameObject = isSameObject;
        }

        public readonly bool isSamePanel;
        public readonly bool isSameSubpanel;
        public readonly bool isSameTab;
        public readonly bool isSameObject;
    }
}
