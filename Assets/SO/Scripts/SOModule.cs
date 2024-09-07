
using UnityEngine;

namespace SO
{
    public abstract class SOModule : ScriptableObject
    {
        public abstract void AddSystems(SOStartup startup);

        public abstract void InjectData(SOStartup startup);
    }
}