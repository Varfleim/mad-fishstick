
using UnityEngine;

using Leopotam.EcsLite;

namespace MF.Map
{
    internal class MapData : MonoBehaviour
    {
        public static void MapActivationRequest(
            EcsWorld world,
            EcsPool<RMapActivation> requestPool,
            EcsPackedEntity mapPE)
        {
            //Создаём новую сущность и назначаем ей запрос
            int requestEntity = world.NewEntity();
            ref RMapActivation requestComp = ref requestPool.Add(requestEntity);

            //Заполняем данные запроса
            requestComp = new(
                mapPE);
        }

        public static void MapRenderInitializationRequest(
            EcsWorld world,
            EcsPool<RMapRenderInitialization> requestPool)
        {
            //Создаём новую сущность и назначаем ей запрос
            int requestEntity = world.NewEntity();
            ref RMapRenderInitialization requestComp = ref requestPool.Add(requestEntity);

            //Заполняем данные запроса
            requestComp = new(0);
        }

        public static void MapEdgesUpdateRequest(
            EcsWorld world,
            EcsPool<RMapEdgesUpdate> requestPool,
            bool isThinUpdated, bool isThickUpdated)
        {
            //Создаём новую сущность и назначаем ей запрос
            int requestEntity = world.NewEntity();
            ref RMapEdgesUpdate requestComp = ref requestPool.Add(requestEntity);

            //Заполняем данные запроса
            requestComp = new(
                isThinUpdated, isThickUpdated, false);
        }

        public static void MapProvincesUpdateRequest(
            EcsWorld world,
            EcsPool<RMapProvincesUpdate> requestPool,
            bool isMaterialUpdated, bool isHeightUpdated, bool isColorUpdated)
        {
            //Создаём новую сущность и назначаем ей запрос
            int requestEntity = world.NewEntity();
            ref RMapProvincesUpdate requestComp = ref requestPool.Add(requestEntity);

            //Заполняем данные запроса
            requestComp = new(
                isMaterialUpdated, isHeightUpdated, isColorUpdated);
        }
    }
}
