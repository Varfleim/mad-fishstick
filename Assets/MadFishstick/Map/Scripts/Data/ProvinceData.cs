
using System.Collections.Generic;

using UnityEngine;

using Leopotam.EcsLite;

namespace MF.Map
{
    public class ProvinceData : MonoBehaviour
    {
        public static void ProvinceCoreCreationRequest(
            EcsPool<SRProvinceCoreCreation> requestPool,
            int provinceEntity,
            EcsPackedEntity parentMapPE,
            List<EcsPackedEntity> neighbours)
        {
            //Назначаем сущности запрос
            ref SRProvinceCoreCreation requestComp = ref requestPool.Add(provinceEntity);

            //Заполняем данные запроса
            requestComp = new(
                parentMapPE,
                neighbours.ToArray());
        }

        public static void ProvinceCoreCreation(
            EcsWorld world,
            ref SRProvinceCoreCreation requestComp,
            int provinceEntity,
            EcsPool<CProvinceCore> pCPool,
            List<EcsPackedEntity> mapProvincesList)
        {
            //Назначаем сущности компонент PC
            ref CProvinceCore pC = ref pCPool.Add(provinceEntity);

            //Заполняем основные данные PC
            pC = new(
                world.PackEntity(provinceEntity),
                requestComp.neighbourProvincePEs);

            //Заносим провинцию в список
            mapProvincesList.Add(pC.selfPE);
        }
    }
}
