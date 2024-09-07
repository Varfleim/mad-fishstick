
using System.Collections.Generic;

using UnityEngine;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace SO
{
    public class SOStartup : MonoBehaviour
    {
        EcsWorld world;
        EcsSystems perFrameSystems;
        EcsSystems perTickSystems;

        public GameObject coreObject;

        public List<SOModule> modules = new List<SOModule>();

        void Start()
        {
            //�������������� ��� � ������ ������
            world = new EcsWorld();
            perFrameSystems = new EcsSystems(world);
            perTickSystems = new EcsSystems(world);

            //�������������� ������
            RuntimeData runtimeData = coreObject.AddComponent(typeof(RuntimeData)) as RuntimeData;

            //�������������� ������
            Random.InitState(0);

            //��� ������� ������ ��������� �������
            for(int a = 0; a < modules.Count; a++)
            {
                //��������� �������
                modules[a].AddSystems(this);
            }

            //��� ������� ������ ������ ������
            for (int a = 0; a < modules.Count; a++)
            {
                //������ ������
                modules[a].InjectData(this);
            }

            //������ ������
            perFrameSystems.Inject(runtimeData);
            perTickSystems.Inject(runtimeData);

            //��������� ������������� ������
            perFrameSystems.Init();
            perTickSystems.Init();

            TimeTickSystem.Create();

            TimeTickSystem.OnTick += delegate (object sender, TimeTickSystem.OnTickEventArgs e)
            {
                if (runtimeData.isGameActive == true)
                {
                    Debug.Log("Stage 1 " + System.DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss:fff"));
                    perTickSystems?.Run();
                    Debug.Log("Stage 2 " + System.DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss:fff"));
                }
            };
        }

        void Update()
        {
            perFrameSystems?.Run();
        }

        void OnDestroy()
        {
            if (perFrameSystems != null)
            {
                perFrameSystems.Destroy();
                perFrameSystems = null;
            }
            if (perTickSystems != null)
            {
                perTickSystems.Destroy();
                perTickSystems = null;
            }

            if (world != null)
            {
                world.Destroy();
                world = null;
            }
        }

        public void AddPerFrameSystem(IEcsSystem system)
        {
            perFrameSystems.Add(system);
        }

        public void AddPerTickSystem(IEcsSystem system)
        {
            perTickSystems.Add(system);
        }

        public void InjectData(params object[] injects)
        {
            perFrameSystems.Inject(injects);
        }
    }
}