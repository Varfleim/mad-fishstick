
using System.Collections.Generic;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using MF;

namespace HS
{
    public class SHexasphereGeneration : IEcsInitSystem
    {
        readonly EcsWorldInject world = default;


        readonly EcsFilterInject<Inc<CProvinceHexasphere>, Exc<MF.Map.SRProvinceCoreCreation>> provinceWithoutNeighboursFilter = default;
        readonly EcsPoolInject<CProvinceHexasphere> pHSPool = default;


        readonly EcsCustomInject<HexasphereData> hexasphereData = default;

        public void Init(IEcsSystems systems)
        {
            //Генерируем гексасферы
            HexasphereGeneration();
        }

        readonly EcsFilterInject<Inc<SRHexasphereGeneration>> mapHexasphereGenerationSelfRequestFilter = default;
        readonly EcsPoolInject<SRHexasphereGeneration> hexasphereGenerationSelfRequestPool = default;
        void HexasphereGeneration()
        {
            //Для каждой карты с запросом генерации гексасферы
            foreach (int mapEntity in mapHexasphereGenerationSelfRequestFilter.Value)
            {
                //Берём запрос
                ref SRHexasphereGeneration requestComp = ref hexasphereGenerationSelfRequestPool.Value.Get(mapEntity);

                //Инициализируем данные карты
                HexasphereInitialize();

                //Генерируем простую гексасферу
                HexasphereGenerate(ref requestComp);

                //Запрашиваем создание PC по PHS
                ProvinceHexasphereCoreCreationSelfRequests(ref requestComp);

                //Удаляем запрос
                hexasphereGenerationSelfRequestPool.Value.Del(mapEntity);
            }
        }

        void HexasphereInitialize()
        {

        }

        void HexasphereGenerate(
            ref SRHexasphereGeneration requestComp)
        {
            DHexaspherePoint[] corners = new DHexaspherePoint[]
            {
                new DHexaspherePoint(1, HexasphereData.PHI, 0),
                new DHexaspherePoint(-1, HexasphereData.PHI, 0),
                new DHexaspherePoint(1, -HexasphereData.PHI, 0),
                new DHexaspherePoint(-1, -HexasphereData.PHI, 0),
                new DHexaspherePoint(0, 1, HexasphereData.PHI),
                new DHexaspherePoint(0, -1, HexasphereData.PHI),
                new DHexaspherePoint(0, 1, -HexasphereData.PHI),
                new DHexaspherePoint(0, -1, -HexasphereData.PHI),
                new DHexaspherePoint(HexasphereData.PHI, 0, 1),
                new DHexaspherePoint(-HexasphereData.PHI, 0, 1),
                new DHexaspherePoint(HexasphereData.PHI, 0, -1),
                new DHexaspherePoint(-HexasphereData.PHI, 0, -1)
            };

            //Определяем треугольники изначального икосаэдра
            DHexasphereTriangle[] triangles = new DHexasphereTriangle[]
            {
                new DHexasphereTriangle(corners [0], corners [1], corners [4], false),
                new DHexasphereTriangle(corners [1], corners [9], corners [4], false),
                new DHexasphereTriangle(corners [4], corners [9], corners [5], false),
                new DHexasphereTriangle(corners [5], corners [9], corners [3], false),
                new DHexasphereTriangle(corners [2], corners [3], corners [7], false),
                new DHexasphereTriangle(corners [3], corners [2], corners [5], false),
                new DHexasphereTriangle(corners [7], corners [10], corners [2], false),
                new DHexasphereTriangle(corners [0], corners [8], corners [10], false),
                new DHexasphereTriangle(corners [0], corners [4], corners [8], false),
                new DHexasphereTriangle(corners [8], corners [2], corners [10], false),
                new DHexasphereTriangle(corners [8], corners [4], corners [5], false),
                new DHexasphereTriangle(corners [8], corners [5], corners [2], false),
                new DHexasphereTriangle(corners [1], corners [0], corners [6], false),
                new DHexasphereTriangle(corners [11], corners [1], corners [6], false),
                new DHexasphereTriangle(corners [3], corners [9], corners [11], false),
                new DHexasphereTriangle(corners [6], corners [10], corners [7], false),
                new DHexasphereTriangle(corners [3], corners [11], corners [7], false),
                new DHexasphereTriangle(corners [11], corners [6], corners [7], false),
                new DHexasphereTriangle(corners [6], corners [0], corners [10], false),
                new DHexasphereTriangle(corners [9], corners [1], corners [11], false)
            };

            //Очищаем словарь точек
            hexasphereData.Value.points.Clear();

            //Заносим вершины икосаэдра в словарь
            for (int a = 0; a < corners.Length; a++)
            {
                hexasphereData.Value.points[corners[a]] = corners[a];
            }

            //Создаём список точек нижнего ребра треугольника
            List<DHexaspherePoint> bottom = ListPool<DHexaspherePoint>.Get();
            //Определяем количество треугольников
            int triangleCount = triangles.Length;
            //Для каждого треугольника
            for (int a = 0; a < triangleCount; a++)
            {
                //Создаём пустой список точек
                List<DHexaspherePoint> previous;

                //Берём первую вершину треугольника
                DHexaspherePoint point0 = triangles[a].points[0];

                //Очищаем временный список точек
                bottom.Clear();

                //Заносим в список первую вершину треугольника
                bottom.Add(point0);

                //Создаём список точек левого ребра треугольника
                List<DHexaspherePoint> left = PointSubdivide(
                    point0, triangles[a].points[1],
                    requestComp.subdivisions);
                //Создаём список точек правого ребра треугольника
                List<DHexaspherePoint> right = PointSubdivide(
                    point0, triangles[a].points[2],
                    requestComp.subdivisions);

                //Для каждого подразделения
                for (int b = 1; b <= requestComp.subdivisions; b++)
                {
                    //Переносим список точек нижнего ребра
                    previous = bottom;

                    //Подразделяем перемычку с левого ребра до правого
                    bottom = PointSubdivide(
                        left[b], right[b],
                        b);

                    //Создаём новый треугольник
                    new DHexasphereTriangle(previous[0], bottom[0], bottom[1]);

                    //Для каждого ...
                    for (int c = 1; c < b; c++)
                    {
                        //Создаём два новых треугольника
                        new DHexasphereTriangle(previous[c], bottom[c], bottom[c + 1]);
                        new DHexasphereTriangle(previous[c - 1], previous[c], bottom[c]);
                    }
                }

                //Возвращаем списки в пул
                ListPool<DHexaspherePoint>.Add(left);
                ListPool<DHexaspherePoint>.Add(right);
            }

            //Возвращаем список в пул
            ListPool<DHexaspherePoint>.Add(bottom);

            //Создаём провинции
            //Обнуляем флаг точек
            DHexaspherePoint.flag = 0;

            //Для каждой вершины в словаре
            foreach (DHexaspherePoint point in hexasphereData.Value.points.Values)
            {
                //Создаём для провинций компоненты гексасферы
                ProvinceCreate(point);
            }
        }

        List<DHexaspherePoint> PointSubdivide(
            DHexaspherePoint startPoint, DHexaspherePoint endPoint,
            int count)
        {
            //Создаём список точек, определяющих сегменты грани, и заносим в него текущую вершину
            List<DHexaspherePoint> segments = ListPool<DHexaspherePoint>.Get();
            segments.Add(startPoint);

            //Рассчитываем координаты точек
            double dx = endPoint.x - startPoint.x;
            double dy = endPoint.y - startPoint.y;
            double dz = endPoint.z - startPoint.z;
            double doublex = startPoint.x;
            double doubley = startPoint.y;
            double doublez = startPoint.z;
            double doubleCount = count;

            //Для каждого подразделения
            for (int a = 1; a < count; a++)
            {
                //Создаём новую вершину
                DHexaspherePoint newPoint = new(
                    (float)(doublex + dx * a / doubleCount),
                    (float)(doubley + dy * a / doubleCount),
                    (float)(doublez + dz * a / doubleCount));

                //Проверяем вершину
                newPoint = PointGetCached(newPoint);

                //Заносим вершину в список
                segments.Add(newPoint);
            }

            //Заносим в список конечную вершину
            segments.Add(endPoint);

            //Возвращаем список точеку
            return segments;
        }

        DHexaspherePoint PointGetCached(
            DHexaspherePoint point)
        {
            //Если запрошенная вершина существует в словаре
            if (hexasphereData.Value.points.TryGetValue(point, out DHexaspherePoint thePoint))
            {
                //Возвращаем вершину
                return thePoint;
            }
            //Иначе
            else
            {
                //Обновляем вершину в словаре
                hexasphereData.Value.points[point] = point;

                //Возвращаем вершину
                return point;
            }
        }

        void ProvinceCreate(
            DHexaspherePoint centerPoint)
        {
            //Создаём новую сущность и назначаем ей компонент PHS
            int provinceEntity = world.Value.NewEntity();
            ref CProvinceHexasphere currentPHS = ref pHSPool.Value.Add(provinceEntity);

            //Заполняем основные данные PHS
            currentPHS = new(
                world.Value.PackEntity(provinceEntity),
                centerPoint);
        }

        void ProvinceHexasphereCoreCreationSelfRequests(
            ref SRHexasphereGeneration hexasphereGenerationRequestComp)
        {
            //Создаём временный список для соседей
            List<EcsPackedEntity> tempNeighbours = ListPool<EcsPackedEntity>.Get();

            //Для каждой провинции без соседей
            foreach (int provinceEntity in provinceWithoutNeighboursFilter.Value)
            {
                //Берём PHS
                ref CProvinceHexasphere pHS = ref pHSPool.Value.Get(provinceEntity);

                //Рассчитываем соседей

                //Очищаем временный список
                tempNeighbours.Clear();

                //Берём первый треугольник в данных провинции
                DHexasphereTriangle firstTriangle = pHS.centerPoint.triangles[0];

                //Для каждой вершины треугольника
                for (int a = 0; a < firstTriangle.points.Length; a++)
                {
                    //Если это не текущая провинция
                    if (firstTriangle.points[a].provincePE.EqualsTo(pHS.selfPE) == false)
                    {
                        //Заносим провинцию в список
                        tempNeighbours.Add(firstTriangle.points[a].provincePE);
                    }
                }

                //Пока количество соседей во временном списке меньше количества треугольников
                while (tempNeighbours.Count < pHS.centerPoint.triangleCount)
                {
                    //Создаём переменную для новой провинции
                    EcsPackedEntity newProvincePE = new();

                    //Для каждого треугольника провинции
                    for (int a = 0; a < pHS.centerPoint.triangleCount; a++)
                    {
                        //Берём треугольник
                        DHexasphereTriangle triangle = pHS.centerPoint.triangles[a];

                        //Проверяем, является ли этот треугольник следующим по порядку
                        bool isContainPreviousProvince = false;
                        bool isContainNewProvince = false;

                        //Для каждой вершины треугольника
                        for (int b = 0; b < triangle.points.Length; b++)
                        {
                            //Если это не текущая провинция
                            if (triangle.points[b].provincePE.EqualsTo(pHS.selfPE) == false)
                            {
                                //Берём PE провинции
                                EcsPackedEntity trianglePointProvincePE = triangle.points[b].provincePE;

                                //Если эта провинция - последняя во временном списке
                                if (tempNeighbours[tempNeighbours.Count - 1].EqualsTo(trianglePointProvincePE) == true)
                                {
                                    //Отмечаем, что треугольник содержит предыдущую провинцию
                                    isContainPreviousProvince = true;
                                }
                                //Иначе, если этой провинции нет во временном списке
                                else if (tempNeighbours.Contains(trianglePointProvincePE) == false)
                                {
                                    //Отмечаем, что треугольник содержит новую провинцию
                                    isContainNewProvince = true;

                                    //Сохраняем PE новой провинции
                                    newProvincePE = trianglePointProvincePE;
                                }
                            }
                        }

                        //Если этот треугольник удовлетворяет обоим условиям, то он является следующим
                        if (isContainPreviousProvince == true
                            && isContainNewProvince == true)
                        {
                            //Выходим из цикла
                            break;
                        }
                    }

                    //Заносим новую провинцию во временный список
                    tempNeighbours.Add(newProvincePE);
                }

                //Запрашиваем создание PC по PHS
                ProvinceCoreCreationRequest(
                    hexasphereGenerationRequestComp.mapIndex,
                    provinceEntity,
                    tempNeighbours);
            }

            //Возвращаем список в пул
            ListPool<EcsPackedEntity>.Add(tempNeighbours);
        }

        readonly EcsPoolInject<MF.Map.SRProvinceCoreCreation> pCCreationSelfRequestPool = default;
        void ProvinceCoreCreationRequest(
            int parentMapIndex,
            int provinceEntity,
            List<EcsPackedEntity> neighbours)
        {
            //Назначаем сущности провинции запрос создания PC
            ref MF.Map.SRProvinceCoreCreation requestComp = ref pCCreationSelfRequestPool.Value.Add(provinceEntity);

            //Заполняем данные запроса
            requestComp = new(
                parentMapIndex,
                neighbours.ToArray());
        }
    }
}
