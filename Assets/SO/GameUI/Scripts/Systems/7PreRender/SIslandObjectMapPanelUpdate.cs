
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using GS.UI;

using SO.Island;

namespace SO.GameUI
{
    public class SIslandObjectMapPanelUpdate : IEcsRunSystem
    {
        readonly EcsWorldInject world = default;


        readonly EcsPoolInject<CIsland> islandPool = default;


        readonly EcsPoolInject<CObjectDisplayedMapPanels> objectDisplayedMapPanelsPool = default;

        readonly EcsFilterInject<Inc<RObjectMapPanelShow>> objectMapPanelShowRFilter = default;
        readonly EcsPoolInject<RObjectMapPanelShow> objectMapPanelShowRPool = default;

        readonly EcsPoolInject<MF.Map.RProvinceMapPanelSetParent> provinceMapPanelSetParentRPool = default;

        public void Run(IEcsSystems systems)
        {
            //Проверяем, не требуется ли отобразить панели карты островов
            ShowIslandObjectMapPanel();
        }

        void ShowIslandObjectMapPanel()
        {
            //Для каждого запроса отображения панели карты
            foreach(int requestEntity in objectMapPanelShowRFilter.Value)
            {
                //Берём запрос
                ref RObjectMapPanelShow requestComp = ref objectMapPanelShowRPool.Value.Get(requestEntity);

                //Если требуется отобразить панель карты острова
                if (requestComp.objectMapPanelType == GameUIData.islandObjectMapPanelType)
                {
                    //Обновляем панель карты острова
                    IslandObjectMapPanelUpdate(ref requestComp);

                    //Удаляем запрос
                    objectMapPanelShowRPool.Value.Del(requestEntity);
                }
            }
        }

        void IslandObjectMapPanelUpdate(
            ref RObjectMapPanelShow requestComp)
        {
            //Берём остров
            requestComp.objectPE.Unpack(world.Value, out int islandEntity);
            ref CIsland island = ref islandPool.Value.Get(islandEntity);

            //Берём компонент панелей карты
            ref CObjectDisplayedMapPanels islandDisplayedMapPanels = ref objectDisplayedMapPanelsPool.Value.Get(islandEntity);

            //Берём панель карты острова
            UIIslandObjectMapPanel islandMapPanel = islandDisplayedMapPanels.objectMapPanels[requestComp.objectMapPanelType] as UIIslandObjectMapPanel;

            //Обновляем данные панели
            islandMapPanel.selfName.text = island.selfName;

            //Если положение панели карты неправильно
            if(IslandObjectMapPanelPositionCheck(
                islandMapPanel,
                ref island) == false)
            {
                //Запрашиваем изменение родительского объекта панели карты
                MF.Map.ProvinceData.ProvinceMapPanelSetParentRequest(
                    world.Value,
                    provinceMapPanelSetParentRPool.Value,
                    island.selfPE,
                    islandMapPanel.gameObject);
            }
        }

        bool IslandObjectMapPanelPositionCheck(
            UIIslandObjectMapPanel islandMapPanel,
            ref CIsland island)
        {
            //Панель карты должна быть прикреплена к сущности острова 
            return island.selfPE.EqualsTo(islandMapPanel.parentProvincePE);
        }
    }
}
