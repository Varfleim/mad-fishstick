
using UnityEngine;

using MF;

namespace HS.Pathfinding
{
    [CreateAssetMenu]
    public class HexaspherePathfindingModule : MFModule
    {
        public override void AddSystems(MFStartup startup)
        {
            
        }

        public override void InjectData(MFStartup startup)
        {
            //—оздаЄм компонент данных поиска пути
            PathfindingData pathfindingData = startup.AddDataObject().AddComponent<PathfindingData>();

            //¬водим данные
            startup.InjectData(pathfindingData);
        }
    }
}
