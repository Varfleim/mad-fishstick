
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

        public static void MapRenderUpdateRequest(
            EcsWorld world,
            EcsPool<RMapRenderUpdate> requestPool,
            bool isMaterialUpdated, bool isHeightUpdated, bool isColorUpdated)
        {
            //Создаём новую сущность и назначаем ей запрос
            int requestEntity = world.NewEntity();
            ref RMapRenderUpdate requestComp = ref requestPool.Add(requestEntity);

            //Заполняем данные запроса
            requestComp = new(
                isMaterialUpdated, isHeightUpdated, isColorUpdated);
        }
    }
}
