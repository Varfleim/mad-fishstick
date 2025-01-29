
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using MF.Map;
using SO.LandOwnership;

namespace SO.MapMode
{
    public class SPoliticalMapModeRender : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<CAgentLandOwner>> aLandOwnerFilter = default;

        readonly EcsPoolInject<SRSetMapRenderValues> setMapRenderValuesSelfRequestsPool = default;

        public void Run(IEcsSystems systems)
        {
            //Для каждого владельца земли
            foreach (int aLandOwnerEntity in aLandOwnerFilter.Value)
            {
                //Создаём запрос изменения визуализации
                MF.Map.MapModeData.SetMapRenderValuesRequestCreation(
                    setMapRenderValuesSelfRequestsPool.Value,
                    aLandOwnerEntity);
            }
        }
    }
}
