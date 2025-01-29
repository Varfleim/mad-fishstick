
using UnityEngine;

using MF;

namespace SO.Initialization
{
    [CreateAssetMenu]
    internal class InitializationModule : MFModule
    {
        public string[] mapNames;
        public int hexasphereSubdivisions;
        public int averageProvincesPerRegion;

        public string[] agentNames;

        public override void AddSystems(MFStartup startup)
        {
            //Добавляем системы инициализации
            #region PreInit
            //Тестовая стартовая инициализация
            startup.AddPreInitSystem(new SInitializationTest());

            //Инициализация карты
            startup.AddPreInitSystem(new SMapInitialization());
            //Инициализация агентов
            startup.AddPreInitSystem(new SAgentInitialization());
            #endregion
            #region Init
            //Первый этап инициализации регионов
            startup.AddInitSystem(new SRegionInitializationFirst());
            //Второй этап инициализации регионов
            startup.AddInitSystem(new SRegionInitializationSecond());

            //Первый этап инициализации островов
            startup.AddInitSystem(new SIslandInitializationFirst());
            //Второй этап инициализации островов
            startup.AddInitSystem(new SIslandInitializationSecond());
            #endregion
        }

        public override void InjectData(MFStartup startup)
        {
            //Создаём компонент данных инициализации
            InitializerData initializerData = startup.AddDataObject().AddComponent<InitializerData>();

            //Переносим в него данные
            initializerData.mapNames = mapNames;
            initializerData.hexasphereSubdivisions = hexasphereSubdivisions;
            initializerData.averageProvincesPerRegion = averageProvincesPerRegion;

            initializerData.agentNames = agentNames;

            //Вводим данные
            startup.InjectData(initializerData);
        }
    }
}
