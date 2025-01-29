
using UnityEngine;

using MF;

namespace SO.Island
{
    [CreateAssetMenu]
    internal class IslandModule : MFModule
    {
        public override void AddSystems(MFStartup startup)
        {
            //Добавляем системы инициализации
            #region Init
            //Создание островов по запросу
            startup.AddInitSystem(new SIslandCreation());
            #endregion
        }

        public override void InjectData(MFStartup startup)
        {
            //Создаём компонент данных островов
            IslandData islandData = startup.AddDataObject().AddComponent<IslandData>();

            //Вводим данные
            startup.InjectData(islandData);
        }
    }
}
