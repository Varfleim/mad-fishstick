
using System.Collections.Generic;

using UnityEngine;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using MF.Map;

namespace SO.Region
{
    public class SRegionsGeneration : IEcsInitSystem
    {
        readonly EcsWorldInject world = default;


        readonly EcsPoolInject<CMap> mapPool = default;
        readonly EcsPoolInject<CMapRegions> mapRegionsPool = default;

        readonly EcsPoolInject<CRegionCore> rCPool = default;
        readonly EcsPoolInject<CTRegionGeneration> rGPool = default;

        readonly EcsPoolInject<CProvinceCore> pCPool = default;
        readonly EcsPoolInject<CTProvinceRegionNeighbours> pRNPool = default;
        readonly EcsPoolInject<CTProvinceRegionOwner> pROPool = default;

        readonly EcsPoolInject<CTWithoutFreeNeighbours> withoutFreeNeighboursPool = default;


        readonly EcsCustomInject<MapModeData> mapModeData = default;
             
        public void Init(IEcsSystems systems)
        {
            //Генерируем регионы
            RegionsGeneration();

            //Обновляем списки цветов регионов
            RegionsColorListsUpdate();
        }

        readonly EcsFilterInject<Inc<SRRegionsGeneration>> mapRegionsGenerationSRFilter = default;
        readonly EcsPoolInject<SRRegionsGeneration> regionsGenerationSRPool = default;
        void RegionsGeneration()
        {
            //Для каждой карты с запросом генерации регионов
            foreach (int mapEntity in mapRegionsGenerationSRFilter.Value)
            {
                //Берём карту и запрос
                ref CMap map = ref mapPool.Value.Get(mapEntity);
                ref SRRegionsGeneration requestComp = ref regionsGenerationSRPool.Value.Get(mapEntity);

                //Назначаем карте компонент для сохранения информации о регионах
                MapRegionsComponentCreation(mapEntity);

                //Берём этот компонент
                ref CMapRegions mapR = ref mapRegionsPool.Value.Get(mapEntity);

                //Создаём пустые регионы
                RegionsCreation(
                    ref map, ref mapR, ref requestComp);

                //Назначаем провинциям временные компоненты соседей
                ProvincesRegionNeighboursComponentCreation(ref map);

                //Назначаем регионам первые провинции
                RegionsSetFirstProvinces(
                    ref map, ref mapR);

                //Расширяем регионы от стартовых провинций
                RegionsExpansion(
                    ref map);

                //Определяем соседей регионов
                RegionsSetNeighbours(
                    ref mapR);

                //Переносим данные из временных компонентов в основные
                RegionsSaveTempData();

                //Удаляем запрос
                regionsGenerationSRPool.Value.Del(mapEntity);
            }
        }

        void MapRegionsComponentCreation(
            int mapEntity)
        {
            //Назначаем сущности карты компонент регионов карты
            ref CMapRegions mapR = ref mapRegionsPool.Value.Add(mapEntity);

            //Заполняем основные данные
            mapR = new(0);
        }

        void RegionsCreation(
            ref CMap map, ref CMapRegions mapR, ref SRRegionsGeneration requestComp)
        {
            //Определяем количество регионов
            int regionsCount = Mathf.RoundToInt(map.provincePEs.Length / requestComp.averageProvincesPerRegion);

            //Создаём массив для PE регионов
            mapR.regionPEs = new EcsPackedEntity[regionsCount];

            //Для каждого требуемого региона
            for(int a = 0; a < mapR.regionPEs.Length; a++)
            {
                //Создаём новую сущность и назначаем ей компоненты региона и генерации региона
                int regionEntity = world.Value.NewEntity();
                ref CRegionCore rC = ref rCPool.Value.Add(regionEntity);
                ref CTRegionGeneration rG = ref rGPool.Value.Add(regionEntity);

                //Заполняем основные данные региона
                rC = new(
                    world.Value.PackEntity(regionEntity),
                    map.selfPE);

                //Генерируем уникальный цвет региона
                RegionColorGeneration(ref rC);

                //Заполняем данные компонента генерации
                rG = new(0);

                //Заносим PE региона в массив карты
                mapR.regionPEs[a] = rC.selfPE;
            }
        }

        void RegionColorGeneration(
            ref CRegionCore rC)
        {
            //Создаём случайный цвет
            Color regionColor = new(Random.value, Random.value, Random.value);

            //Пока данный цвет существует в словаре цветов
            while(mapModeData.Value.regionMapModeUniqueColors.TryGetValue(regionColor, out EcsPackedEntity oldRegionPE) == true)
            {
                //Создаём случайный цвет
                regionColor = new(Random.value, Random.value, Random.value);
            }

            //Заносим его в словарь
            mapModeData.Value.regionMapModeUniqueColors.Add(regionColor, rC.selfPE);
        }

        void ProvincesRegionNeighboursComponentCreation(
            ref CMap map)
        {
            //Для каждой провинции карты
            for(int a = 0; a < map.provincePEs.Length; a++)
            {
                //Берём сущность провинции и назначаем ей компонент соседей региона
                map.provincePEs[a].Unpack(world.Value, out int provinceEntity);
                ref CTProvinceRegionNeighbours pRN = ref pRNPool.Value.Add(provinceEntity);

                //Заполняем данные компонента
                pRN = new(
                    world.Value.PackEntity(provinceEntity));
            }
        }

        void RegionsSetFirstProvinces(
            ref CMap map, ref CMapRegions mapR)
        {
            //Для каждого региона карты
            for(int a = 0; a < mapR.regionPEs.Length; a++)
            {
                //Берём регион и его временный компонент
                mapR.regionPEs[a].Unpack(world.Value, out int regionEntity);
                ref CRegionCore rC = ref rCPool.Value.Get(regionEntity);
                ref CTRegionGeneration rG = ref rGPool.Value.Get(regionEntity);

                //Берём случайную свободную провинцию со свободными соседями
                int provinceEntity = MapGetRandomProvinceWithFreeNeighbours(ref map);
                ref CProvinceCore pC = ref pCPool.Value.Get(provinceEntity);
                ref CTProvinceRegionNeighbours pRN = ref pRNPool.Value.Get(provinceEntity);

                //Заносим полученную провинцию в регион
                RegionAddProvince(
                    ref rC, ref rG,
                    provinceEntity, ref pC, ref pRN);

                //Заносим соседей провинции в регион
                //Для каждой соседней провинции
                for(int b = 0; b < pC.neighbourProvincePEs.Length; b++)
                {
                    //Берём провинцию
                    pC.neighbourProvincePEs[b].Unpack(world.Value, out int neighbourProvinceEntity);
                    ref CProvinceCore neighbourPC = ref pCPool.Value.Get(neighbourProvinceEntity);
                    ref CTProvinceRegionNeighbours neighbourPRN = ref pRNPool.Value.Get(neighbourProvinceEntity);

                    //Заносим провинцию в регион
                    RegionAddProvince(
                        ref rC, ref rG,
                        neighbourProvinceEntity, ref neighbourPC, ref neighbourPRN);
                }
            }
        }

        int MapGetRandomProvinceWithFreeNeighbours(
            ref CMap map)
        {
            //Берём сущность случайной провинции
            map.GetProvinceRandom().Unpack(world.Value, out int provinceEntity);

            //Проверяем, свободна ли данная провинция и свободны ли её соседи
            bool isFreeAndFreeNeighbours = false;

            //Пока провинция не подходит
            while(isFreeAndFreeNeighbours == false)
            {
                //Берём провинцию
                ref CProvinceCore pC = ref pCPool.Value.Get(provinceEntity);
                ref CTProvinceRegionNeighbours pRN = ref pRNPool.Value.Get(provinceEntity);

                //Если провинция не имеет компонента владельца и количество занятых соседей равно нулю
                if(pROPool.Value.Has(provinceEntity) == false
                    && pRN.ownedNeighboursCount == 0)
                {
                    //Отмечаем, что провинция подходит
                    isFreeAndFreeNeighbours = true;
                }
                //Иначе
                else
                {
                    //Берём сущность случайной провинции
                    map.GetProvinceRandom().Unpack(world.Value, out provinceEntity);
                }
            }

            //Возвращаем сущность итоговой провинции
            return provinceEntity;
        }

        readonly EcsFilterInject<Inc<CRegionCore, CTRegionGeneration>, Exc<CTWithoutFreeNeighbours>> regionWithFreeNeighboursFilter = default;
        readonly EcsFilterInject<Inc<CProvinceCore, CTProvinceRegionNeighbours, CTProvinceRegionOwner>> provinceWithOwnerFilter = default;
        void RegionsExpansion(
            ref CMap map)
        {
            //Создаём счётчик ошибок
            int regionErrorCount = 0;

            //Пока не всем провинциям назначены владельцы
            while(provinceWithOwnerFilter.Value.GetEntitiesCount() < map.provincePEs.Length)
            {
                //Определяем, сколько провинций было добавлено в этом цикле
                int addedProvincesCount = 0;

                //Для каждого региона, участвующего в генерации и имеющего свободных соседей
                foreach (int regionEntity in regionWithFreeNeighboursFilter.Value)
                {
                    //Если у региона нет свободных соседей - такое возможно, поскольку данный компонент назначается внутри этого цикла
                    if(withoutFreeNeighboursPool.Value.Has(regionEntity) == false)
                    {
                        //Берём регион
                        ref CRegionCore rC = ref rCPool.Value.Get(regionEntity);
                        ref CTRegionGeneration rG = ref rGPool.Value.Get(regionEntity);

                        //Берём из региона случайную провинцию со свободными соседями
                        int currentProvinceEntity = RegionGetProvinceWithFreeNeighbours(
                            ref rC, ref rG);
                        ref CProvinceCore currentPC = ref pCPool.Value.Get(currentProvinceEntity);
                        ref CTProvinceRegionNeighbours currentPRN = ref pRNPool.Value.Get(currentProvinceEntity);
                        ref CTProvinceRegionOwner currentPRO = ref pROPool.Value.Get(currentProvinceEntity);

                        //Пытаемся получить соседа провинции, соответствующего условиям
                        int neighbourProvinceEntity = ProvinceGetNeighbourWithoutRegion(
                            ref currentPC, ref currentPRO);
                        //Если полученная сущность действительна
                        if(neighbourProvinceEntity > -1)
                        {
                            //Берём соседнюю провинции
                            ref CProvinceCore neighbourPC = ref pCPool.Value.Get(neighbourProvinceEntity);
                            ref CTProvinceRegionNeighbours neighbourPRN = ref pRNPool.Value.Get(neighbourProvinceEntity);

                            //Присоединяем полученную провинцию к региону
                            RegionAddProvince(
                                ref rC, ref rG,
                                neighbourProvinceEntity, ref neighbourPC, ref neighbourPRN);

                            //Увеличиваем количество добавленных провинций
                            addedProvincesCount++;
                        }
                    }
                }

                //Если за цикл не было добавлено ни одной провинции
                if (addedProvincesCount == 0)
                {
                    //Увеличиваем счётчик ошибок
                    regionErrorCount++;
                }

                //Если счётчик ошибок достиг количества провинций
                if (regionErrorCount > map.provincePEs.Length)
                {
                    //Выводим ошибку и прерываем цикл
                    Debug.LogWarning(regionErrorCount + " Невозможно дальнейшее расширение регионов карты!");

                    break;
                }
            }
        }

        int RegionGetProvinceWithFreeNeighbours(
            ref CRegionCore rC, ref CTRegionGeneration rG)
        {
            //Создаём переменую для сущности лучшей найденной провинции
            int bestProvinceEntity = -1;
            //И счётчик соседних провинций с тем же родительским регионом
            int globalSameRegionNeighboursCount = -1;

            //Проходим по всем внешним провинциям региона, имеющим свободных соседей
            for(int a = 0; a < rG.outerProvinceWithFreeNeighboursPEs.Count; a++)
            {
                //Берём провинцию
                rG.outerProvinceWithFreeNeighboursPEs[a].Unpack(world.Value, out int provinceEntity);
                ref CProvinceCore pC = ref pCPool.Value.Get(provinceEntity);
                ref CTProvinceRegionNeighbours pRN = ref pRNPool.Value.Get(provinceEntity);

                //Создаём локальный счётчик соседних провинций с тем же родительским регионом
                int localSameRegionNeighboursCount = 0;

                //Проходим по её соседям, подсчитывая, сколько из них принадлежат тому же региону
                for(int b = 0; b < pC.neighbourProvincePEs.Length; b++)
                {
                    //Берём сущность соседней провинции
                    pC.neighbourProvincePEs[b].Unpack(world.Value, out int neighbourProvinceEntity);

                    //Если соседняя провинция имеет владельца
                    if(pROPool.Value.Has(neighbourProvinceEntity) == true)
                    {
                        //Берём компонент владения
                        ref CTProvinceRegionOwner neighbourPRO = ref pROPool.Value.Get(neighbourProvinceEntity);

                        //Если соседняя провинция принадлежит тому же региону
                        if(neighbourPRO.parentRegionPE.EqualsTo(rC.selfPE) == true)
                        {
                            //Увеличиваем локальный счётчик
                            localSameRegionNeighboursCount++;
                        }
                    }
                }

                //Если значение локального счётчика выше глобального
                if(localSameRegionNeighboursCount > globalSameRegionNeighboursCount)
                {
                    //Обновляем сущность лучшей провинции
                    bestProvinceEntity = provinceEntity;

                    //И глобальный счётчик
                    globalSameRegionNeighboursCount = localSameRegionNeighboursCount;
                }
                //Иначе, если локальное значение равно глобальному
                else if (localSameRegionNeighboursCount == globalSameRegionNeighboursCount)
                {
                    //С некоторым шансом
                    if(Random.value >= 0.5f)
                    {
                        //Обновляем сущность лучшей провинции
                        bestProvinceEntity = provinceEntity;
                    }
                }
            }

            //Возвращаем сущность лучщей провинции
            return bestProvinceEntity;
        }

        int ProvinceGetNeighbourWithoutRegion(
            ref CProvinceCore pC, ref CTProvinceRegionOwner pRO)
        {
            //Создаём переменную для сущности лучшей найденной провинции
            int bestProvinceEntity = -1;
            //И счётчик соседних провинций с тем же родительским регионом
            int globalSameRegionNeighboursCount = 1;

            //Создаём переменную для отслеживания предыдущего соседа и заполняем её последним соседом в массиве
            pC.neighbourProvincePEs[pC.neighbourProvincePEs.Length - 1].Unpack(world.Value, out int previousNeighbourProvinceEntity);

            //Проходим по всем соседям
            for (int a = 0; a < pC.neighbourProvincePEs.Length; a++)
            {
                //Берём сущность текущего соседа
                pC.neighbourProvincePEs[a].Unpack(world.Value, out int currentNeighbourProvinceEntity);

                //Если он не имеет владельца
                if(pROPool.Value.Has(currentNeighbourProvinceEntity) == false)
                {
                    //Создаём локальный счётчик соседних провинций с тем же родительским регионом
                    int localSameRegionNeighboursCount = 0;

                    //Если предыдущий сосед имеет владельца
                    if (pROPool.Value.Has(previousNeighbourProvinceEntity) == true)
                    {
                        //Берём компонент владения предыдущего соседа
                        ref CTProvinceRegionOwner previousNeighbourPRO = ref pROPool.Value.Get(previousNeighbourProvinceEntity);

                        //Если предыдущий сосед принадлежит тому же региону, увеличиваем счётчик
                        if (previousNeighbourPRO.parentRegionPE.EqualsTo(pRO.parentRegionPE) == true)
                        {
                            localSameRegionNeighboursCount++;
                        }
                    }

                    //Берём сущность следующего соседа
                    int nextNeighbourProvinceEntity;

                    //Если текущий сосед - не последний в массиве
                    if (a < pC.neighbourProvincePEs.Length - 1)
                    {
                        //Следующим соседом является a + 1, берём его сущность
                        pC.neighbourProvincePEs[a + 1].Unpack(world.Value, out nextNeighbourProvinceEntity);
                    }
                    //Иначе 
                    else
                    {
                        //Следующим соседом является первый сосед, берём его сущность
                        pC.neighbourProvincePEs[0].Unpack(world.Value, out nextNeighbourProvinceEntity);
                    }

                    //Если следующий сосед имеет владельца
                    if(pROPool.Value.Has(nextNeighbourProvinceEntity) == true)
                    {
                        //Берём компонент владения следующего соседа
                        ref CTProvinceRegionOwner nextNeighbourPRO = ref pROPool.Value.Get(nextNeighbourProvinceEntity);

                        //Если следующий сосед принадлежит тому же региону, увеличиваем счётчик
                        if (nextNeighbourPRO.parentRegionPE.EqualsTo(pRO.parentRegionPE) == true)
                        {
                            localSameRegionNeighboursCount++;
                        }
                    }

                    //Если значение локального счётчика выше глобального
                    if (localSameRegionNeighboursCount > globalSameRegionNeighboursCount)
                    {
                        //Обновляем сущность лучшей провинции
                        bestProvinceEntity = currentNeighbourProvinceEntity;

                        //И глобальный счётчик
                        globalSameRegionNeighboursCount = localSameRegionNeighboursCount;
                    }
                    //Иначе, если локальное значение равно глобальному
                    else if (localSameRegionNeighboursCount == globalSameRegionNeighboursCount)
                    {
                        //С некоторым шансом
                        if (Random.value >= 0.5f)
                        {
                            //Обновляем сущность лучшей провинции
                            bestProvinceEntity = currentNeighbourProvinceEntity;
                        }
                    }
                }

                //Сохраняем сущность текущего соседа как предыдущего
                previousNeighbourProvinceEntity = currentNeighbourProvinceEntity;
            }

            //Возвращаем сущность итоговой провинции
            return bestProvinceEntity;
        }

        void RegionAddProvince(
            ref CRegionCore rC, ref CTRegionGeneration rG,
            int provinceEntity, ref CProvinceCore pC, ref CTProvinceRegionNeighbours pRN)
        {
            //Заносим провинцию в список внешних провинций региона
            rG.outerProvinceWithFreeNeighboursPEs.Add(pRN.selfPE);

            //Назначаем провинции компонент владельца
            ref CTProvinceRegionOwner pRO = ref pROPool.Value.Add(provinceEntity);
            //Заносим регион в данные провинции
            pRO.parentRegionPE = rC.selfPE;

            //Обновляем количество свободных соседей у соседних провинций
            //Для каждой соседней провинции
            for (int a = 0; a < pC.neighbourProvincePEs.Length; a++)
            {
                //Увеличиваем количество занятых соседей
                ProvinceAddOwnedNeighbours(pC.neighbourProvincePEs[a]);
            }

            //Если у провинции нет свободных соседей
            if (withoutFreeNeighboursPool.Value.Has(provinceEntity) == true)
            {
                //Увеличиваем количество провинций, не имеющих свободных соседей
                RegionAddProvinceWithoutFreeNeighbours(
                    rC.selfPE,
                    pRN.selfPE);
            }

            //Проверяем пограничные провинции региона
            RegionCheckOuterProvinces(
                ref rC, ref rG);
        }

        void ProvinceAddOwnedNeighbours(
            EcsPackedEntity provincePE)
        {
            //Берём провинцию
            provincePE.Unpack(world.Value, out int provinceEntity);
            ref CProvinceCore pC = ref pCPool.Value.Get(provinceEntity);
            ref CTProvinceRegionNeighbours pRN = ref pRNPool.Value.Get(provinceEntity);

            //Увеличиваем количество занятых соседей
            pRN.ownedNeighboursCount++;

            //Если количество занятых соседей равно количеству соседей
            if (pRN.ownedNeighboursCount == pC.neighbourProvincePEs.Length)
            {
                //Назначаем провинции компонент отсутствия свободных соседей
                ref CTWithoutFreeNeighbours withoutFreeNeighbours = ref withoutFreeNeighboursPool.Value.Add(provinceEntity);

                //Если провинция принадлежит региону
                if (pROPool.Value.Has(provinceEntity) == true)
                {
                    //Берём компонент владения
                    ref CTProvinceRegionOwner pRO = ref pROPool.Value.Get(provinceEntity);

                    //Увеличиваем количество провинций, не имеющих свободных соседей
                    RegionAddProvinceWithoutFreeNeighbours(
                        pRO.parentRegionPE,
                        pRN.selfPE);
                }
            }
        }

        void RegionAddProvinceWithoutFreeNeighbours(
            EcsPackedEntity regionPE,
            EcsPackedEntity provincePE)
        {
            //Берём регион
            regionPE.Unpack(world.Value, out int regionEntity);
            ref CRegionCore rC = ref rCPool.Value.Get(regionEntity);
            ref CTRegionGeneration rG = ref rGPool.Value.Get(regionEntity);

            //Увеличиваем количество провинций, не имеющих свободных соседей
            rG.provinceWithoutFreeNeighboursCount++;

            //Если провинция находится в списке внешних провинций региона со свободными соседями
            if (rG.outerProvinceWithFreeNeighboursPEs.Contains(provincePE))
            {
                //Удаляем её из этого списка
                rG.outerProvinceWithFreeNeighboursPEs.Remove(provincePE);

                //И заносим в список внешних провинций, не имеющих свободных соседей
                rG.outerProvinceWithoutFreeNeighboursPEs.Add(provincePE);
            }

            //Если соседи всех провинций уже заняты
            if (rG.provinceWithoutFreeNeighboursCount == rG.ProvinceTotalCount)
            {
                //Назначаем региону компонент отсутствия свободных соседей
                ref CTWithoutFreeNeighbours withoutFreeNeighbours = ref withoutFreeNeighboursPool.Value.Add(regionEntity);
            }
        }

        void RegionCheckOuterProvinces(
            ref CRegionCore rC, ref CTRegionGeneration rG)
        {
            //Для каждой внешней провинции региона в обратном порядке
            for (int a = rG.outerProvinceWithoutFreeNeighboursPEs.Count - 1; a >= 0; a--)
            {
                //Берём сущость провинции
                rG.outerProvinceWithoutFreeNeighboursPEs[a].Unpack(world.Value, out int provinceEntity);

                //Если провинция не имеет свободных соседей, то она может оказаться внутренней
                if (withoutFreeNeighboursPool.Value.Has(provinceEntity) == true)
                {
                    //Берём провинцию
                    ref CProvinceCore pC = ref pCPool.Value.Get(provinceEntity);

                    //Проверяем, принадлежат ли все соседи к тому же региону
                    bool isSameRegion = true;

                    //Для каждого соседа провинции
                    for (int b = 0; b < pC.neighbourProvincePEs.Length; b++)
                    {
                        //Берём компонент владельца провинции
                        pC.neighbourProvincePEs[b].Unpack(world.Value, out int neighbourProvinceEntity);
                        ref CTProvinceRegionOwner neighbourPRO = ref pROPool.Value.Get(neighbourProvinceEntity);

                        //Если владелец соседа - не тот же регион
                        if (neighbourPRO.parentRegionPE.EqualsTo(rC.selfPE) == false)
                        {
                            //Отмечаем, что сосед принадлежит другому региону, и выходим из цикла
                            isSameRegion = false;

                            break;
                        }
                    }

                    //Если все соседи принадлежат тому же региону
                    if (isSameRegion == true)
                    {
                        //Заносим провинцию в список внутренних
                        rG.innerProvincePEs.Add(pC.selfPE);

                        //Удаляем провинцию из списка внешних
                        rG.outerProvinceWithoutFreeNeighboursPEs.RemoveAt(a);
                    }
                }
            }
        }

        void RegionsSetNeighbours(
            ref CMapRegions mapR)
        {
            //Для каждого региона карты
            for(int a = 0; a < mapR.regionPEs.Length; a++)
            {
                //Берём регион
                mapR.regionPEs[a].Unpack(world.Value, out int regionEntity);
                ref CRegionCore rC = ref rCPool.Value.Get(regionEntity);
                ref CTRegionGeneration rG = ref rGPool.Value.Get(regionEntity);

                //Для каждой внешней провинции региона без свободных соседей
                for (int b = 0; b < rG.outerProvinceWithoutFreeNeighboursPEs.Count; b++)
                {
                    //Берём провинцию
                    rG.outerProvinceWithoutFreeNeighboursPEs[b].Unpack(world.Value, out int provinceEntity);
                    ref CProvinceCore pC = ref pCPool.Value.Get(provinceEntity);
                    ref CTProvinceRegionNeighbours pRN = ref pRNPool.Value.Get(provinceEntity);

                    //Для каждой соседней провинции
                    for(int c = 0; c < pC.neighbourProvincePEs.Length; c++)
                    {
                        //Берём соседнюю провинцию
                        pC.neighbourProvincePEs[c].Unpack(world.Value, out int neighbourProvinceEntity);
                        ref CTProvinceRegionOwner neighbourPRO = ref pROPool.Value.Get(neighbourProvinceEntity);

                        //Если она принадлежит другому региону
                        if(neighbourPRO.parentRegionPE.EqualsTo(rC.selfPE) == false)
                        {
                            //Заносим её родительский регион в список соседних регионов текущего региона
                            rG.tempNeighbourRegionPEs.Add(neighbourPRO.parentRegionPE);

                            //Заносим её PE в список соседних провинций текущего региона
                            rG.tempNeighbourProvincePEs.Add(pC.neighbourProvincePEs[c]);
                        }
                    }
                }
            }
        }

        readonly EcsFilterInject<Inc<CRegionCore, CTRegionGeneration>> rGFilter = default;
        readonly EcsFilterInject<Inc<CProvinceCore, CTProvinceRegionNeighbours, CTProvinceRegionOwner>> provinceRegionGenerationFilter = default;
        readonly EcsFilterInject<Inc<CTWithoutFreeNeighbours>> withoutFreeNeighboursFilter = default;
        void RegionsSaveTempData()
        {
            //Для каждой сущности с компонентом отсутствия соседей
            foreach(int entity in withoutFreeNeighboursFilter.Value)
            {
                //Удаляем компонент с сущности
                withoutFreeNeighboursPool.Value.Del(entity);
            }

            //Для каждой провинции с компонентами региона
            foreach (int provinceEntity in provinceRegionGenerationFilter.Value)
            {
                //Берём провинцию
                ref CProvinceCore pC = ref pCPool.Value.Get(provinceEntity);
                ref CTProvinceRegionNeighbours pRN = ref pRNPool.Value.Get(provinceEntity);
                ref CTProvinceRegionOwner pRO = ref pROPool.Value.Get(provinceEntity);

                //Переносим временные данные в основные

                //Удаляем компоненты с сущности
                pRNPool.Value.Del(provinceEntity);
                pROPool.Value.Del(provinceEntity);
            }

            //Для каждого региона с компонентом генерации
            foreach (int regionEntity in rGFilter.Value)
            {
                //Берём регион
                ref CRegionCore rC = ref rCPool.Value.Get(regionEntity);
                ref CTRegionGeneration rG = ref rGPool.Value.Get(regionEntity);

                //Переносим временные данные в основные
                rC.provincePEs = rG.GetAllProvinces().ToArray();
                rC.firstOuterProvinceIndex = rG.innerProvincePEs.Count;

                rC.neighbourRegionPEs = new EcsPackedEntity[rG.tempNeighbourRegionPEs.Count];
                rG.tempNeighbourRegionPEs.CopyTo(rC.neighbourRegionPEs);
                rC.neighbourProvincePEs = new EcsPackedEntity[rG.tempNeighbourProvincePEs.Count];
                rG.tempNeighbourProvincePEs.CopyTo(rC.neighbourProvincePEs);

                //Удаляем компонент с сущности
                rGPool.Value.Del(regionEntity);
            }
        }

        readonly EcsPoolInject<RMapModeUpdateColorsListSecond> mapModeUpdateColorsListSecondRPool = default;
        void RegionsColorListsUpdate()
        {
            //Если количество цветов в словаре больше количества цветов в списке
            if(mapModeData.Value.regionMapModeUniqueColors.Count > mapModeData.Value.regionMapModeColors.Count)
            {
                //Очищаем списки
                mapModeData.Value.regionMapModeColors.Clear();
                mapModeData.Value.regionMapModePEs.Clear();

                //Для каждой записи в словаре
                foreach(KeyValuePair<Color, EcsPackedEntity> kVP in mapModeData.Value.regionMapModeUniqueColors)
                {
                    //Заносим в списки цвет и PE региона
                    mapModeData.Value.regionMapModeColors.Add(kVP.Key);
                    mapModeData.Value.regionMapModePEs.Add(kVP.Value);

                    //Берём регион
                    kVP.Value.Unpack(world.Value, out int regionEntity);
                    ref CRegionCore rC = ref rCPool.Value.Get(regionEntity);

                    //Назначаем индекс цвета региона
                    rC.SetColorIndex(mapModeData.Value.regionMapModeColors.Count - 1);
                }

                //Запрашиваем вторичное обновление списка цветов карты
                MF.Map.MapModeData.MapModeUpdateColorsListSecondRequest(
                    world.Value,
                    mapModeUpdateColorsListSecondRPool.Value,
                    mapModeData.Value.regionMapModePE,
                    mapModeData.Value.regionMapModeColors, mapModeData.Value.regionMapModeDefaultColor);
            }
        }
    }
}
