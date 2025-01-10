
using UnityEngine;

namespace MF.Test
{
    [CreateAssetMenu]
    public class TestModule : MFModule
    {
        //Переменные для редактирования через редактор
        public string testText;

        public override void AddSystems(MFStartup startup)
        {
            //Добавляем системы инициализации

            //Добавляем покадровые системы
            startup.AddFrameSystem(new STest());

            //Добавляем системы рендеринга

            //Добавляем потиковые системы

        }

        public override void InjectData(MFStartup startup)
        {
            //Создаём компонент данных
            TestData testData = startup.AddDataObject().AddComponent<TestData>();

            testData.testText = testText;

            //Вводим данные
            startup.InjectData(testData);
        }
    }
}