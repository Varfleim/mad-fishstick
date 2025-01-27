
using UnityEngine;

using MF;

namespace GS.Agent
{
    [CreateAssetMenu]
    public class AgentModule : MFModule
    {
        public string[] agentNames;

        public override void AddSystems(MFStartup startup)
        {
            //Добавляем системы инициализации
            #region PreInit
            //Создание агентов по запросу
            startup.AddPreInitSystem(new SAgentCreation());
            #endregion
        }

        public override void InjectData(MFStartup startup)
        {
            //Создаём компонент данных агентов
            AgentData agentData = startup.AddDataObject().AddComponent<AgentData>();

            //Переносим в него данные
            agentData.agentNames = agentNames;

            //Вводим данные
            startup.InjectData(agentData);
        }
    }
}
