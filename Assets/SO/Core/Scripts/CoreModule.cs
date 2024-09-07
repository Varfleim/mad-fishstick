
using UnityEngine;

namespace SO.Core
{
    [CreateAssetMenu]
    public class CoreModule : SOModule
    {
        //���������� ��� �������������� ����� ��������
        public int seed;

        public override void AddSystems(SOStartup startup)
        {
            //�������, ������������ ������� �����������
            startup.AddPerFrameSystem(new SRandom());
        }

        public override void InjectData(SOStartup startup)
        {
            //������ ��������� ������
            CoreData coreData = startup.coreObject.AddComponent(typeof(CoreData)) as CoreData;

            //��������� � ���� ����
            coreData.seed = seed;

            //������ ������
            startup.InjectData(coreData);
        }
    }
}
