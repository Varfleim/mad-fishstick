
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
            //Инициализируем мир и группы систем
            world = new EcsWorld();
            perFrameSystems = new EcsSystems(world);
            perTickSystems = new EcsSystems(world);

            //Инициализируем данные
            RuntimeData runtimeData = coreObject.AddComponent(typeof(RuntimeData)) as RuntimeData;

            //Инициализируем семена
            Random.InitState(0);

            //Для каждого модуля добавляем системы
            for(int a = 0; a < modules.Count; a++)
            {
                //Добавляем системы
                modules[a].AddSystems(this);
            }

            //Для каждого модуля вводим данные
            for (int a = 0; a < modules.Count; a++)
            {
                //Вводим данные
                modules[a].InjectData(this);
            }

            //Вводим данные
            perFrameSystems.Inject(runtimeData);
            perTickSystems.Inject(runtimeData);

            //Выполняем инициализацию систем
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