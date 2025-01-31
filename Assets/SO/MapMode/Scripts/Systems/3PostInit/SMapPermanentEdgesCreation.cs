
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using SO.Region;
using SO.Island;

namespace SO.MapMode
{
    public class SMapPermanentEdgesCreation : IEcsInitSystem
    {
        readonly EcsFilterInject<Inc<CRegionCore>> rCFilter = default;

        public void Init(IEcsSystems systems)
        {
            //Запрашиваем создание тонких граней
            ThinEdgesCreationRequest();

            //Запрашиваем создание толстых граней
            ThickEdgesCreationRequest();
        }

        readonly EcsPoolInject<MF.Map.SRUpdateThinEdges> updateThinEdgesSRPool = default;
        void ThinEdgesCreationRequest()
        {
            //Создаём счётчик регионов
            int regionsCount = 0;

            //Для каждого региона
            foreach(int regionEntity in rCFilter.Value)
            {
                //Создаём запрос обновления тонких граней для него
                MF.Map.MapModeData.UpdateThinEdgesRequest(
                    updateThinEdgesSRPool.Value,
                    regionEntity,
                    regionsCount);

                //Увеличиваем счётчик регионов
                regionsCount++;
            }
        }

        readonly EcsFilterInject<Inc<CIsland>> islandFilter = default;
        readonly EcsPoolInject<MF.Map.SRUpdateThickEdges> updateThickEdgesSRPool = default;
        void ThickEdgesCreationRequest()
        {
            //Создаём счётчик островов
            int islandCount = 0;

            //Для каждого острова
            foreach(int islandEntity in islandFilter.Value)
            {
                //Создаём запрос обновления толстых граней для него
                MF.Map.MapModeData.UpdateThickEdgesRequest(
                    updateThickEdgesSRPool.Value,
                    islandEntity,
                    islandCount);

                //Увеличиваем счётчик островов
                islandCount++;
            }
        }
    }
}
