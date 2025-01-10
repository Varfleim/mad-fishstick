
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using MF.Map;

namespace HS
{
    public class SDefaultMapModeRender : IEcsRunSystem
    {
        readonly EcsWorldInject world = default;

        readonly EcsFilterInject<Inc<CMap, CActiveMap>> activeMapFilter = default;
        readonly EcsPoolInject<CMap> mapPool = default;

        public void Run(IEcsSystems systems)
        {
            //Для каждой активной карты
            foreach (int activeMapEntity in activeMapFilter.Value)
            {
                //Берём карту
                ref CMap activeMap = ref mapPool.Value.Get(activeMapEntity);

                //Создаём запросы изменения визуализации для каждой провинции
                SetMapRenderValuesRequests(ref activeMap);
            }
        }

        readonly EcsPoolInject<SRSetMapRenderValues> setMapRenderValuesSelfRequestPool = default;
        void SetMapRenderValuesRequests(
            ref CMap map)
        {
            //Для каждой провинции карты
            for(int a = 0; a < map.provincePEs.Length; a++)
            {
                //Берём сущность провинции
                map.provincePEs[a].Unpack(world.Value, out int provinceEntity);

                //Создаём запрос изменения визуализации для неё
                MF.Map.MapModeData.SetMapRenderValuesRequestCreation(
                    setMapRenderValuesSelfRequestPool.Value,
                    provinceEntity);
            }
        }
    }
}
