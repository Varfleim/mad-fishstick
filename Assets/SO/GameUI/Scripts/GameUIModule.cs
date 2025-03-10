
using System.Collections.Generic;

using UnityEngine;

using MF;
using GS.UI;

namespace SO.GameUI
{
    [CreateAssetMenu]
    public class GameUIModule : MFModule
    {
        public string islandObjectSubpanelType;
        public string islandOSbpnOverviewTabType;

        public string islandObjectMapPanelType;

        public override void AddSystems(MFStartup startup)
        {
            //Добавляем системы рендеринга
            #region PreRender
            //Обновление подпанели острова
            startup.AddPreRenderSystem(new SIslandObjectSubpanelUpdate());

            //Обновление панелей карты островов
            startup.AddPreRenderSystem(new SIslandObjectMapPanelUpdate());
            #endregion
        }

        public override void InjectData(MFStartup startup)
        {
            //Берём компонент данных UI
            GameUIData gameUIData = startup.GetComponentInChildren<GameUIData>();

            //Переносим в него данные
            GameUIData.islandObjectSubpanelType = islandObjectSubpanelType;
            GameUIData.islandOSbpnOverviewTabType = islandOSbpnOverviewTabType;

            GameUIData.islandObjectMapPanelType = islandObjectMapPanelType;

            //Вводим данные
            startup.InjectData(gameUIData);

            //Заносим подпанели объектов в данные интерфейса
            UIData.objectSubpanels.Add(islandObjectSubpanelType, gameUIData.islandObjectSubpanel);
            gameUIData.islandObjectSubpanel.subpanelTabs.Add(islandOSbpnOverviewTabType, gameUIData.islandOSbpnOverviewTab);

            //Заносим панели карты в данные интерфейса
            UIAObjectMapPanel.objectMapPanelPrefabs.Add(islandObjectMapPanelType, gameUIData.islandObjectMapPanel);
            UIAObjectMapPanel.cachedObjectMapPanels.Add(islandObjectMapPanelType, new List<UIAObjectMapPanel>());
        }
    }
}
