
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using MF.Map;

using SO.Island;
using SO.LandOwnership;

namespace SO.MapMode
{
    public class SPoliticalMapModePostThreads : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<CIsland>, Exc<CLandOwned>> islandWithoutOwnerFilter = default;

        readonly EcsPoolInject<SRUpdateProvinceRender> setMapRenderValuesSelfRequestsPool = default;

        public void Run(IEcsSystems systems)
        {
            //Для каждого острова без владельца
            foreach (int islandEntity in islandWithoutOwnerFilter.Value)
            {
                //Создаём запрос обновления визуализации провинции, отображающий отсутствие владельца
                MF.Map.MapModeData.UpdateProvinceRenderRequestFull(
                    setMapRenderValuesSelfRequestsPool.Value,
                    islandEntity,
                    new(),
                    0.01f,
                    0);
            }
        }
    }
}
