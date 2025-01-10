
using System.Collections.Generic;

using UnityEngine;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;

namespace MF
{
    public class MFStartup : MonoBehaviour
    {
        EcsWorld world;

        EcsSystems preInitSystems;
        EcsSystems initSystems;
        EcsSystems postInitSystems;

        EcsSystems preFrameSystems;
        EcsSystems frameSystems;
        EcsSystems postFrameSystems;

        EcsSystems preRenderSystems;
        EcsSystems renderSystems;
        EcsSystems postRenderSystems;

        EcsSystems preTickSystems;
        EcsSystems tickSystems;
        EcsSystems postTickSystems;

        #region Map
        public GameObject coreObject;
        public GameObject mapObject;
        public Collider mapCollider;
        #endregion

        #region Camera
        public Transform mapCamera;
        public Transform swiwel;
        public Transform stick;
        public new Camera camera;
        #endregion

        public List<MFModule> modules = new List<MFModule>();

        void Start()
        {
            //Инициализируем пулы списков
            InitListPools();

            //Инициализируем мир и группы систем
            world = new EcsWorld();

            preInitSystems = new EcsSystems(world);
            initSystems = new EcsSystems(world);
            postInitSystems = new EcsSystems(world);

            preFrameSystems = new EcsSystems(world);
            frameSystems = new EcsSystems(world);
            postFrameSystems = new EcsSystems(world);

            preRenderSystems = new EcsSystems(world);
            renderSystems = new EcsSystems(world);
            postRenderSystems = new EcsSystems(world);

            preTickSystems = new EcsSystems(world);
            tickSystems = new EcsSystems(world);
            postTickSystems = new EcsSystems(world);

            //Инициализируем данные
            RuntimeData runtimeData = coreObject.AddComponent(typeof(RuntimeData)) as RuntimeData;

            //Инициализируем семена
            Random.InitState(0);

            //Для каждого модуля добавляем системы
            for (int a = 0; a < modules.Count; a++)
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
            InjectData(runtimeData);

            //Выполняем инициализацию систем
            preInitSystems.Init();
            initSystems.Init();
            postInitSystems.Init();

            preFrameSystems.Init();
            frameSystems.Init();
            postFrameSystems.Init();

            preRenderSystems.Init();
            renderSystems.Init();
            postRenderSystems.Init();

            preTickSystems.Init();
            tickSystems.Init();
            postTickSystems.Init();

            TimeTickSystem.Create();

            TimeTickSystem.OnTick += delegate (object sender, TimeTickSystem.OnTickEventArgs e)
            {
                if (runtimeData.isGameActive == true)
                {
                    Debug.Log("Stage 1 " + System.DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss:fff"));
                    preTickSystems?.Run();
                    tickSystems?.Run();
                    postTickSystems?.Run();
                    Debug.Log("Stage 2 " + System.DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss:fff"));
                }
            };
        }

        void Update()
        {
            preFrameSystems?.Run();
            frameSystems?.Run();
            postFrameSystems?.Run();

            preRenderSystems?.Run();
            renderSystems?.Run();
            postRenderSystems?.Run();
        }

        void OnDestroy()
        {
            //Удаление систем инициализации
            if (preInitSystems != null)
            {
                preInitSystems.Destroy();
                preInitSystems = null;
            }
            if (initSystems != null)
            {
                initSystems.Destroy();
                initSystems = null;
            }
            if (postInitSystems != null)
            {
                postInitSystems.Destroy();
                postInitSystems = null;
            }

            //Удаление покадровых систем
            if (preFrameSystems != null)
            {
                preFrameSystems.Destroy();
                preFrameSystems = null;
            }
            if (frameSystems != null)
            {
                frameSystems.Destroy();
                frameSystems = null;
            }
            if (postFrameSystems != null)
            {
                postFrameSystems.Destroy();
                postFrameSystems = null;
            }

            //Удаление систем рендеринга
            if (preRenderSystems != null)
            {
                preRenderSystems.Destroy();
                preRenderSystems = null;
            }
            if (renderSystems != null)
            {
                renderSystems.Destroy();
                renderSystems = null;
            }
            if (postRenderSystems != null)
            {
                postRenderSystems.Destroy();
                postRenderSystems = null;
            }

            //Удаление потиковых систем
            if (preTickSystems != null)
            {
                preTickSystems.Destroy();
                preTickSystems = null;
            }
            if (tickSystems != null)
            {
                tickSystems.Destroy();
                tickSystems = null;
            }
            if (postTickSystems != null)
            {
                postTickSystems.Destroy();
                postTickSystems = null;
            }

            if (world != null)
            {
                world.Destroy();
                world = null;
            }
        }

        public void InitListPools()
        {

        }

        public void AddPreInitSystem(IEcsSystem system)
        {
            preInitSystems.Add(system);
        }
        public void AddInitSystem(IEcsSystem system)
        {
            initSystems.Add(system);
        }
        public void AddPostInitSystem(IEcsSystem system)
        {
            postInitSystems.Add(system);
        }

        public void AddPreFrameSystem(IEcsSystem system)
        {
            preFrameSystems.Add(system);
        }
        public void AddFrameSystem(IEcsSystem system)
        {
            frameSystems.Add(system);
        }
        public void AddPostFrameSystem(IEcsSystem system)
        {
            postFrameSystems.Add(system);
        }

        public void AddPreRenderSystem(IEcsSystem system)
        {
            preRenderSystems.Add(system);
        }
        public void AddPreRenderSystemGroup(
            string groupName, 
            bool defaultState,
            params IEcsSystem[] systems)
        {
            preRenderSystems.AddGroup(
                groupName,
                defaultState,
                null,
                systems);
        }
        public void AddRenderSystem(IEcsSystem system)
        {
            renderSystems.Add(system);
        }
        public void AddPostRenderSystem(IEcsSystem system)
        {
            postRenderSystems.Add(system);
        }

        public void AddPreTickSystem(IEcsSystem system)
        {
            preTickSystems.Add(system);
        }
        public void AddTickSystem(IEcsSystem system)
        {
            tickSystems.Add(system);
        }
        public void AddPostTickSystem(IEcsSystem system)
        {
            postTickSystems.Add(system);
        }

        public GameObject AddDataObject()
        {
            //Создаём новый объект для компонента данных
            GameObject newDataObject = new GameObject();

            //Прикрепляем его к корневому объекту
            newDataObject.transform.SetParent(coreObject.transform);

            //Возвращаем объект
            return newDataObject;
        }

        public void InjectData(params object[] injects)
        {
            preInitSystems.Inject(injects);
            initSystems.Inject(injects);
            postInitSystems.Inject(injects);

            preFrameSystems.Inject(injects);
            frameSystems.Inject(injects);
            postFrameSystems.Inject(injects);

            preRenderSystems.Inject(injects);
            renderSystems.Inject(injects);
            postRenderSystems.Inject(injects);

            preTickSystems.Inject(injects);
            tickSystems.Inject(injects);
            postTickSystems.Inject(injects);
        }
    }
}