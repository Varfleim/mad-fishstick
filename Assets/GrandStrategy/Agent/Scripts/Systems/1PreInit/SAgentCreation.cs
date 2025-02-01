
using System.Collections.Generic;

using UnityEngine;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace GS.Agent
{
    public class SAgentCreation : IEcsInitSystem
    {
        readonly EcsWorldInject world = default;


        readonly EcsPoolInject<CAgent> agentPool = default;

        readonly EcsFilterInject<Inc<SRAgentCreation>> agentCreationSelfRequestFilter = default;
        readonly EcsPoolInject<SRAgentCreation> agentCreationSelfRequestPool = default;


        readonly EcsCustomInject<AgentData> agentData = default;

        public void Init(IEcsSystems systems)
        {
            //Создаём агентов
            AgentsCreation();
        }

        void AgentsCreation()
        {
            //Проверяем, обновлены ли цвета
            bool isColorsUpdated = false;

            //Для каждого запроса создания агента
            foreach (int agentRequestEntity in agentCreationSelfRequestFilter.Value)
            {
                //Берём запрос
                ref SRAgentCreation requestComp = ref agentCreationSelfRequestPool.Value.Get(agentRequestEntity);

                //Создаём агента
                AgentCreation(
                    ref requestComp,
                    agentRequestEntity);

                //Отмечаем, что цвета обновлены
                isColorsUpdated = true;

                //Берём агента
                ref CAgent agent = ref agentPool.Value.Get(agentRequestEntity);

                //Удаляем запрос
                agentCreationSelfRequestPool.Value.Del(agentRequestEntity);
            }

            //Если цвета обновлены
            if (isColorsUpdated == true)
            {
                //Обновляем списки цветов агентов
                AgentColorListsUpdate();
            }
        }

        void AgentCreation(
            ref SRAgentCreation requestComp,
            int agentEntity)
        {
            //Назначаем переданной сущности компонент агента
            ref CAgent agent = ref agentPool.Value.Add(agentEntity);

            //Заполняем основные данные агента
            agent = new(
                world.Value.PackEntity(agentEntity), requestComp.agentName);

            //Генерируем уникальный цвет агента
            AgentColorGeneration(ref agent);
        }

        void AgentColorGeneration(
            ref CAgent agent)
        {
            //Создаём случайный цвет
            Color agentColor = new Color(Random.value, Random.value, Random.value);

            //Пока данный цвет существует в словаре цветов
            while (agentData.Value.agentUniqueColors.TryGetValue(agentColor, out EcsPackedEntity oldAgentPE) == true)
            {
                //Создаём случайный цвет
                agentColor = new Color(Random.value, Random.value, Random.value);
            }

            //Заносим его в словарь
            agentData.Value.agentUniqueColors.Add(agentColor, agent.selfPE);
        }

        readonly EcsPoolInject<MF.Map.RMapModeUpdateColorsListFirst> mapModeUpdateColorsListFirstRequestPool = default;
        void AgentColorListsUpdate()
        {
            //Очищаем списки
            agentData.Value.agentColors.Clear();
            agentData.Value.agentColorPEs.Clear();

            //Заносим в списки цвет отсутствия агента
            agentData.Value.agentColors.Add(Color.white);
            agentData.Value.agentColorPEs.Add(new());

            //Для каждой записи в словаре
            foreach(KeyValuePair<Color, EcsPackedEntity> kVP in agentData.Value.agentUniqueColors)
            {
                //Заносим в списки цвет и PE агента
                agentData.Value.agentColors.Add(kVP.Key);
                agentData.Value.agentColorPEs.Add(kVP.Value);

                //Берём агента
                kVP.Value.Unpack(world.Value, out int agentEntity);
                ref CAgent agent = ref agentPool.Value.Get(agentEntity);

                //Назначаем индекс цвета агента
                agent.SetColorIndex(agentData.Value.agentColors.Count - 1);
            }

            //Запрашиваем первичное обновление списка цветов режима карты
            MF.Map.MapModeData.MapModeUpdateColorsListFirstRequest(
                world.Value,
                mapModeUpdateColorsListFirstRequestPool.Value,
                AgentData.agentObjectType,
                agentData.Value.agentColors);
        }
    }
}
