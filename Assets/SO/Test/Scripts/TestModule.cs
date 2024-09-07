
using UnityEngine;

namespace SO.Test
{
    [CreateAssetMenu]
    public class TestModule : SOModule
    {
        //���������� ��� �������������� ����� ��������
        public string testText;

        public override void AddSystems(SOStartup startup)
        {
            startup.AddPerFrameSystem(new STest());
        }

        public override void InjectData(SOStartup startup)
        {
            //������ ��������� ������
            TestData testData = startup.coreObject.AddComponent(typeof(TestData)) as TestData;

            testData.testText = testText;

            //������ ������
            startup.InjectData(testData);
        }
    }
}