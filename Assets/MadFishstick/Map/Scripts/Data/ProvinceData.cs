
using System.Collections.Generic;

using UnityEngine;

using Leopotam.EcsLite;

namespace MF.Map
{
    public class ProvinceData : MonoBehaviour
    {
        public float mapPanelAltitude;

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

        public static void ProvinceMapPanelSetParentRequest(
            EcsWorld world,
            EcsPool<RProvinceMapPanelSetParent> requestPool,
            EcsPackedEntity parentProvincePE,
            GameObject mapPanelGO)
        {
            //Создаём новую сущность и назначаем ей запрос
            int requestEntity = world.NewEntity();
            ref RProvinceMapPanelSetParent requestComp = ref requestPool.Add(requestEntity);

            //Заполняем данные запроса
            requestComp = new(
                parentProvincePE,
                mapPanelGO);
        }
    }
}
