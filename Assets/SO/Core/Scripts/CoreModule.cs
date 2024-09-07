
using UnityEngine;

namespace SO.Core
{
    [CreateAssetMenu]
    public class CoreModule : SOModule
    {
        //Переменные для редактирования через редактор
        public int seed;

        public override void AddSystems(SOStartup startup)
        {
            //Система, определяющие функции случайности
            startup.AddPerFrameSystem(new SRandom());
        }

        public override void InjectData(SOStartup startup)
        {
            //Создаём компонент данных
            CoreData coreData = startup.coreObject.AddComponent(typeof(CoreData)) as CoreData;

            //Переносим в него семя
            coreData.seed = seed;

            //Вводим данные
            startup.InjectData(coreData);
        }
    }
}
