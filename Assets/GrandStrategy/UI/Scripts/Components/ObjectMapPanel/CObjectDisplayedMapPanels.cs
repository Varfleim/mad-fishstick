
using System.Collections.Generic;

namespace GS.UI
{
    public struct CObjectDisplayedMapPanels
    {
        public CObjectDisplayedMapPanels(
            int a)
        {
            objectMapPanels = new();
        }

        public Dictionary<string, UIAObjectMapPanel> objectMapPanels;
    }
}
