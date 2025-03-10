
using System.Collections.Generic;

using UnityEngine;

namespace GS.UI
{
    public abstract class UIAObjectSubpanel : MonoBehaviour
    {
        public Dictionary<string, UIAObjectSubpanelTab> subpanelTabs = new();

        public TabGroup tabGroup;

        public UIAObjectSubpanelTab activeTab;

        public void HideActiveTab()
        {
            //Скрываем активную вкладку
            activeTab.gameObject.SetActive(false);

            //Очищаем сущность активного объекта
            activeTab.objectPE = new();

            //Указываем, что активной вкладки нет
            activeTab = null;
        }
    }
}
