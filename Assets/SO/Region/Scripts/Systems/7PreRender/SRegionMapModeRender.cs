using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using MF.Map;

namespace SO.Region
{
    public class SRegionMapModeRender : IEcsRunSystem
    {
        readonly EcsWorldInject world = default;


        readonly EcsFilterInject<Inc<CMap, CActiveMap>> activeMapFilter = default;
        readonly EcsPoolInject<CMapRegions> mapRegionsPool = default;

        public void Run(IEcsSystems systems)
        {
            //Для каждой активной карты
            foreach(int activeMapEntity in activeMapFilter.Value)
            {
                //Берём компонент регионов карты
                ref CMapRegions activeMapR = ref mapRegionsPool.Value.Get(activeMapEntity);

                //Создаём запросы изменения визуализации для каждого региона
                SetMapRenderValuesRequests(ref activeMapR);
            }
        }

        readonly EcsPoolInject<SRSetMapRenderValues> setMapRenderValuesSelfRequestsPool = default;
        void SetMapRenderValuesRequests(
            ref CMapRegions mapR)
        {
            //Для каждого региона карты
            for(int a = 0; a < mapR.regionPEs.Length; a++)
            {
                //Берём сущность региона
                mapR.regionPEs[a].Unpack(world.Value, out int regionEntity);

                //Создаём запрос изменения визуализации для неё
                MF.Map.MapModeData.SetMapRenderValuesRequestCreation(
                    setMapRenderValuesSelfRequestsPool.Value,
                    regionEntity);
            }
        }
    }
}
