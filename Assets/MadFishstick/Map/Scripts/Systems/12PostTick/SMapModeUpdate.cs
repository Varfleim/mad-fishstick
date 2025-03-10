
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace MF.Map
{
    public class SMapModeUpdate : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            //«апрашиваем обновление активного режима карты
            MapModeActiveUpdate();
        }

        readonly EcsFilterInject<Inc<CMapModeCore, CActiveMapMode>> activeMapModeFilter = default;
        readonly EcsPoolInject<SRMapModeUpdate> mapModeUpdateSRPool = default;
        void MapModeActiveUpdate()
        {
            //ƒл€ каждого активного режима карты
            foreach(int activeMapModeEntity in activeMapModeFilter.Value)
            {
                //«апрашиваем обновление режима карты
                MapModeData.MapModeUpdateRequest(
                    mapModeUpdateSRPool.Value,
                    activeMapModeEntity);
            }
        }
    }
}
