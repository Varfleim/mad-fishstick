
using System.Collections.Generic;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace MF.Map
{
    public class SProvinceCoreCreation : IEcsInitSystem
    {
        readonly EcsWorldInject world = default;


        readonly EcsFilterInject<Inc<CMap>> mapFilter = default;
        readonly EcsPoolInject<CMap> mapPool = default;

        readonly EcsPoolInject<CProvinceCore> pCPool = default;

        public void Init(IEcsSystems systems)
        {
            //Создаём PC по запросам
            ProvincesCoreCreation();
        }

        readonly EcsFilterInject<Inc<SRProvinceCoreCreation>> pCCreationSRFilter = default;
        readonly EcsPoolInject<SRProvinceCoreCreation> pCCreationSRPool = default;
        void ProvincesCoreCreation()
        {
            //Создаём временный список провинций
            List<EcsPackedEntity> tempProvinces = new();

            //Для каждой карты
            foreach(int mapEntity in mapFilter.Value)
            {
                //Берём карту
                ref CMap map = ref mapPool.Value.Get(mapEntity);

                //Очищаем временный список провинций
                tempProvinces.Clear();

                //Для каждой провинции с запросом создания PC
                foreach (int provinceEntity in pCCreationSRFilter.Value)
                {
                    //Берём запрос
                    ref SRProvinceCoreCreation requestComp = ref pCCreationSRPool.Value.Get(provinceEntity);

                    //Если провинция принадлежит текущей карте
                    if(requestComp.parentMapPE.EqualsTo(map.selfPE))
                    {
                        //Создаём PC по запросу
                        ProvinceCoreCreation(
                            provinceEntity,
                            ref requestComp);

                        //Берём PC
                        ref CProvinceCore pC = ref pCPool.Value.Get(provinceEntity);

                        //Заносим провинцию в список
                        tempProvinces.Add(pC.selfPE);

                        //Удаляем запрос
                        pCCreationSRPool.Value.Del(provinceEntity);
                    }
                }

                //Сохраняем список как массив провинций карты
                map.provincePEs = tempProvinces.ToArray();
            }
        }

        void ProvinceCoreCreation(
            int provinceEntity,
            ref SRProvinceCoreCreation requestComp)
        {
            //Назначаем сущности провинции компонент PC
            ref CProvinceCore pC = ref pCPool.Value.Add(provinceEntity);

            //Заполняем основные данные PC
            pC = new(
                world.Value.PackEntity(provinceEntity),
                requestComp.neighbourProvincePEs);
        }
    }
}
