
using UnityEngine;

namespace SO.Test
{
    [CreateAssetMenu]
    public class TestModule : SOModule
    {
        //Переменные для редактирования через редактор
        public string testText;

        public override void AddSystems(SOStartup startup)
        {
            startup.AddPerFrameSystem(new STest());
        }

        public override void InjectData(SOStartup startup)
        {
            //Создаём компонент данных
            TestData testData = startup.coreObject.AddComponent(typeof(TestData)) as TestData;

            testData.testText = testText;

            //Вводим данные
            startup.InjectData(testData);
        }
    }
}