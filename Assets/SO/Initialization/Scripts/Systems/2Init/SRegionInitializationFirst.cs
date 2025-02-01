
using System.Collections.Generic;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using HS.Pathfinding;
using SO.Region;

namespace SO.Initialization
{
    public class SRegionInitializationFirst : IEcsInitSystem
    {
        readonly EcsWorldInject world = default;


        readonly EcsPoolInject<CMapRegions> mapRPool = default;
        readonly EcsPoolInject<CMapPathfinding> mapPFPool = default;

        readonly EcsPoolInject<CRegionCore> rCPool = default;
        readonly EcsFilterInject<Inc<RRegionInitializationFirst>> regionInitializationFirstRFilter = default;
        readonly EcsPoolInject<RRegionInitializationFirst> regionInitializationFirstRPool = default;
        readonly EcsFilterInject<Inc<SRRegionInitializationSecond>> regionInitializationSecondSRFilter = default;
        readonly EcsPoolInject<SRRegionInitializationSecond> regionInitializationSecondSRPool = default;

        readonly EcsPoolInject<CCellPathfinding> cPFPool = default;


        readonly EcsCustomInject<RegionData> regionData = default;

        public void Init(IEcsSystems systems)
        {
            //Проводим первичную инициализацию регионов
            RegionsInitializationLocation();
        }

        void RegionsInitializationLocation()
        {
            //Для каждого первичного инициализатора региона
            foreach (int requestEntity in regionInitializationFirstRFilter.Value)
            {
                //Берём запрос
                ref RRegionInitializationFirst requestComp = ref regionInitializationFirstRPool.Value.Get(requestEntity);

                //Подбираем регион для инициализации
                RegionInitializationLocation(ref requestComp);

                //Удаляем запрос
                regionInitializationFirstRPool.Value.Del(requestEntity);
            }
        }

        void RegionInitializationLocation(
            ref RRegionInitializationFirst requestComp)
        {
            //Берём компонент регионов родительской карты
            requestComp.parentMapPE.Unpack(world.Value, out int mapEntity);
            ref CMapRegions mapR = ref mapRPool.Value.Get(mapEntity);
            ref CMapPathfinding mapPF = ref mapPFPool.Value.Get(mapEntity);

            //Получаем сущность региона, соответствующего условиям, и берём регион
            int regionEntity = MapGetRandomRegionEntity(
                ref requestComp,
                ref mapR, ref mapPF);

            //Запрашиваем вторичную инициализацию найденного региона
            InitializerData.RegionInitializationSecondRequest(
                regionInitializationSecondSRPool.Value,
                regionEntity,
                ref requestComp,
                requestComp.ownerAgentPE);

            //Получаем сущность региона, соответствующего условиям, находящегося на заданном расстоянии
            int firstNeighbourRegionEntity = MapGetRegionEntityWithinSteps(
                ref requestComp,
                ref mapR, ref mapPF,
                regionEntity,
                2, 2);
            InitializerData.RegionInitializationSecondRequest(
                regionInitializationSecondSRPool.Value,
                firstNeighbourRegionEntity,
                ref requestComp);

            int secondNeighbourRegionEntity = MapGetRegionEntityWithinSteps(
                ref requestComp,
                ref mapR, ref mapPF,
                regionEntity,
                2, 2);
            InitializerData.RegionInitializationSecondRequest(
                regionInitializationSecondSRPool.Value,
                secondNeighbourRegionEntity,
                ref requestComp);
        }

        int MapGetRandomRegionEntity(
            ref RRegionInitializationFirst requestComp,
            ref CMapRegions mapR, ref CMapPathfinding mapPF)
        {
            //Подбираем регион, удовлетворяющий условиям
            bool isCorrect = false;
            int regionEntity = -1;

            while (isCorrect == false)
            {
                //Берём сущность случайного региона
                mapR.GetRegionRandom().Unpack(world.Value, out regionEntity);

                //Если регион не имеет запроса вторичной инициализации
                if(regionInitializationSecondSRPool.Value.Has(regionEntity) == false)
                {
                    //Берём регион и компонент поиска пути
                    ref CRegionCore rC = ref rCPool.Value.Get(regionEntity);
                    ref CCellPathfinding rCPF = ref cPFPool.Value.Get(regionEntity);

                    //Временно устанавливаем переменную на true
                    isCorrect = true;

                    //Для каждого региона с запросом вторичной инициализации
                    foreach (int initializedRegionEntity in regionInitializationSecondSRFilter.Value)
                    {
                        //Берём регион и компонент поиска пути
                        ref CRegionCore initializedRC = ref rCPool.Value.Get(initializedRegionEntity);
                        ref CCellPathfinding initializedRCPF = ref cPFPool.Value.Get(initializedRegionEntity);

                        //Рассчитываем путь
                        List<int> path = PathfindingData.PathFind(
                            world.Value,
                            ref mapPF, regionData.Value.modulePathfindingIndex,
                            cPFPool.Value, ref mapR.regionPEs,
                            ref rCPF, ref initializedRCPF,
                            4);

                        //Если длина пути меньше 3
                        if (path != null
                            && path.Count < 3)
                        {
                            //Отмечаем, что данный регион не подходит
                            isCorrect = false;

                            //Выходим из цикла
                            break;
                        }
                    }
                }
            }

            //Возвращаем сущность итогового региона
            return regionEntity;
        }

        int MapGetRegionEntityWithinSteps(
            ref RRegionInitializationFirst requestComp,
            ref CMapRegions mapR, ref CMapPathfinding mapPF,
            int startRegionEntity,
            int minSteps, int maxSteps)
        {
            //Берём стартовый регион
            ref CRegionCore startRC = ref rCPool.Value.Get(startRegionEntity);
            ref CCellPathfinding startRCPF = ref cPFPool.Value.Get(startRegionEntity);

            //Получаем список регионов в указанном диапазоне
            List<int> neighboursWithinSteps = PathfindingData.GetCellIndicesWithinSteps(
                world.Value,
                ref mapPF, regionData.Value.modulePathfindingIndex,
                cPFPool.Value, ref mapR.regionPEs,
                ref startRCPF,
                minSteps, maxSteps);

            //Подбираем регион, удовлетворяющий условиям
            bool isCorrect = false;
            int regionEntity = -1;
            while(isCorrect == false)
            {
                //Берём индекс случайного региона в списке
                mapR.regionPEs[GetRandomRegionIndexInList(neighboursWithinSteps)].Unpack(world.Value, out int neighbourRegionEntity);
                
                //Если регион не имеет запроса вторичной инициализации
                if(regionInitializationSecondSRPool.Value.Has(neighbourRegionEntity) == false)
                {
                    //Отмечаем, что он подходит
                    isCorrect = true;

                    //Сохраняем его сущность
                    regionEntity = neighbourRegionEntity;
                }
            }

            //Возвращаем сущность итогового региона
            return regionEntity;
        }

        int GetRandomRegionIndexInList(
            List<int> regionIndexes)
        {
            return regionIndexes[UnityEngine.Random.Range(0, regionIndexes.Count)];
        }
    }
}
