
using System.Collections.Generic;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using MF.Map;
using HS.Pathfinding;
using SO.Region;

namespace SO.Initialization
{
    public class SPathfindingInitialization : IEcsInitSystem
    {
        readonly EcsWorldInject world = default;


        readonly EcsPoolInject<CCellPathfinding> cPFPool = default;


        readonly EcsCustomInject<RegionData> regionData = default;

        public void Init(IEcsSystems systems)
        {
            //Инициализириуем данные поиска пути карт
            MapsPathfindingInitialization();
        }

        readonly EcsFilterInject<Inc<CMap>> mapFilter = default;
        readonly EcsPoolInject<CMapPathfinding> mapPFPool = default;
        void MapsPathfindingInitialization()
        {
            //Для каждой карты
            foreach(int mapEntity in mapFilter.Value)
            {
                //Назначаем карте компонент поиска пути
                ref CMapPathfinding mapPF = ref mapPFPool.Value.Add(mapEntity);

                //Создаём временный список данных поиска пути модулей
                List<DModulePathfinding> modulePathfindingList = new();

                //Инициализируем поиск пути по регионам
                RegionsPathfindingInitialization(
                    mapEntity,
                    modulePathfindingList);

                //Сохраняем полученный список данных модулей в данных карты
                mapPF.modulesPathfindingData = modulePathfindingList.ToArray();
            }
        }

        readonly EcsPoolInject<CMapRegions> mapRPool = default;
        readonly EcsPoolInject<CRegionCore> rCPool = default;
        void RegionsPathfindingInitialization(
            int mapEntity,
            List<DModulePathfinding> modulePathfindingList)
        {
            //Берём компонент регионов карты
            ref CMapRegions mapR = ref mapRPool.Value.Get(mapEntity);

            //Создаём данные поиска пути для модуля
            DModulePathfinding regionModulePathfinding = new();

            //Заносим его в список и сохраняем индекс
            modulePathfindingList.Add(regionModulePathfinding);
            regionData.Value.modulePathfindingIndex = modulePathfindingList.Count - 1;

            //Для каждого региона карты
            for(int a = 0; a < mapR.regionPEs.Length; a++)
            {
                //Берём регион и назначаем ему компонент ячейки
                mapR.regionPEs[a].Unpack(world.Value, out int regionEntity);
                ref CRegionCore rC = ref rCPool.Value.Get(regionEntity);
                ref CCellPathfinding regionCPF = ref cPFPool.Value.Add(regionEntity);

                //Заполняем данные ячейки
                regionCPF = new(
                    a,
                    new(),
                    rC.neighbourRegionPEs);
            }
        }
    }
}
