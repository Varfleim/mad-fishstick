
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using MF.Map;

using GS.UI;

using SO.GameUI;
using SO.Island;
using SO.Colonization;
using SO.LandOwnership;

namespace SO.MapMode
{
    public class SPoliticalMapModePostThreads : IEcsRunSystem
    {
        readonly EcsWorldInject world = default;

        readonly EcsFilterInject<Inc<CIsland, CLandOwned>> islandWithOwnerFilter = default;
        readonly EcsFilterInject<Inc<CIsland>, Exc<CLandOwned, CLandColony>> islandWithoutOwnerFilter = default;
        readonly EcsPoolInject<CIsland> islandPool = default;


        readonly EcsPoolInject<SRUpdateProvinceRender> setMapRenderValuesSRPool = default;

        readonly EcsPoolInject<RObjectMapPanelShow> objectMapPanelShowRPool = default;

        public void Run(IEcsSystems systems)
        {
            //Для каждого острова с владельцем
            foreach(int islandEntity in islandWithOwnerFilter.Value)
            {
                //Берём острова
                ref CIsland island = ref islandPool.Value.Get(islandEntity);

                //Создаём запрос отображения панели карты острова
                UIData.ShowObjectMapPanelRequest(
                    world.Value,
                    objectMapPanelShowRPool.Value,
                    GameUIData.islandObjectMapPanelType,
                    island.selfPE);
            }

            //Для каждого острова без владельца
            foreach (int islandEntity in islandWithoutOwnerFilter.Value)
            {
                //Создаём запрос обновления визуализации провинции, отображающий отсутствие владельца
                MF.Map.MapModeData.UpdateProvinceRenderRequestFull(
                    setMapRenderValuesSRPool.Value,
                    islandEntity,
                    new(),
                    0.01f,
                    0);
            }
        }
    }
}
