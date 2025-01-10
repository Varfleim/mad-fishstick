
using UnityEngine;

namespace MF
{
    public abstract class MFModule : ScriptableObject
    {
        public abstract void AddSystems(MFStartup startup);

        //public abstract void AddPreInitSystems(MFStartup startup);
        //public abstract void AddInitSystems(MFStartup startup);
        /// <summary>
        /// В этой группе систем нельзя направлять запросы к другим модулям, чтобы не нарушить порядок удаления запросов
        /// </summary>
        /// <param name="startup"></param>
        //public abstract void AddPostInitSystems(MFStartup startup);

        //public abstract void AddPreFrameSystems(MFStartup startup);
        //public abstract void AddFrameSystems(MFStartup startup);
        //public abstract void AddPostFrameSystems(MFStartup startup);

        //public abstract void AddPreRenderSystems(MFStartup startup);
        //public abstract void AddRenderSystems(MFStartup startup);
        //public abstract void AddPostRenderSystems(MFStartup startup);

        //public abstract void AddPreTickSystems(MFStartup startup);
        //public abstract void AddTickSystems(MFStartup startup);
        //public abstract void AddPostTickSystems(MFStartup startup);

        public abstract void InjectData(MFStartup startup);
    }
}