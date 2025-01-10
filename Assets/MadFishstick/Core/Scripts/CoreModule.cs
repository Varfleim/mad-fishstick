
using UnityEngine;

namespace MF.Core
{
    [CreateAssetMenu]
    public class CoreModule : MFModule
    {
        //Переменные для редактирования через редактор
        public int seed;

        public override void AddSystems(MFStartup startup)
        {
            //Добавляем системы инициализации
            startup.AddPreInitSystem(new SRandom());

            //Добавляем покадровые системы

            //Добавляем системы рендеринга

            //Добавляем потиковые системы

        }

        public override void InjectData(MFStartup startup)
        {
            //Создаём компонент данных
            CoreData coreData = startup.AddDataObject().AddComponent<CoreData>();

            //Переносим в него данные
            coreData.seed = seed;

            //Вводим данные
            startup.InjectData(coreData);
        }
    }
}
