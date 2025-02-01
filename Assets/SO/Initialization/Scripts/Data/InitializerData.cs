
using UnityEngine;

using Leopotam.EcsLite;

namespace SO.Initialization
{
    internal class InitializerData : MonoBehaviour
    {
        public string[] mapNames;
        public int hexasphereSubdivisions;
        public int averageProvincesPerRegion;

        public string[] agentNames;

        public static void RegionInitializationFirstRequest(
            EcsWorld world,
            EcsPool<RRegionInitializationFirst> requestPool,
            EcsPackedEntity parentMapPE,
            EcsPackedEntity ownerAgentPE = new())
        {
            //Создаём новую сущность и назначаем ей запрос
            int requestEntity = world.NewEntity();
            ref RRegionInitializationFirst requestComp = ref requestPool.Add(requestEntity);

            //Заполняем данные запроса
            requestComp = new(
                parentMapPE,
                ownerAgentPE);
        }
        
        public static void RegionInitializationSecondRequest(
            EcsPool<SRRegionInitializationSecond> requestPool,
            int regionEntity,
            ref RRegionInitializationFirst oldRequestComp,
            EcsPackedEntity ownerAgentPE = new())
        {
            //Назначаем сущности региона запрос
            ref SRRegionInitializationSecond requestComp = ref requestPool.Add(regionEntity);

            //Заполняем данные запроса
            requestComp = new(
                ownerAgentPE);
        }

        public static void IslandInitializationFirstRequest(
            EcsWorld world,
            EcsPool<RIslandInitializationFirst> requestPool,
            EcsPackedEntity parentRegionPE,
            EcsPackedEntity ownerAgentPE = new())
        {
            //Создаём новую сущность и назначаем ей запрос
            int requestEntity = world.NewEntity();
            ref RIslandInitializationFirst requestComp = ref requestPool.Add(requestEntity);

            //Заполняем данные запроса
            requestComp = new(
                parentRegionPE,
                ownerAgentPE);
        }

        public static void IslandInitializationSecondRequest(
            EcsPool<SRIslandInitializationSecond> requestPool,
            int regionEntity,
            ref RIslandInitializationFirst oldRequestComp)
        {
            //Назначаем сущности региона запрос
            ref SRIslandInitializationSecond requestComp = ref requestPool.Add(regionEntity);

            //Заполняем данные запроса
            requestComp = new(
                oldRequestComp.ownerAgentPE);
        }
    }
}
