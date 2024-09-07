
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace SO.Test
{
    public class STest : IEcsInitSystem, IEcsRunSystem
    {
        readonly EcsCustomInject<TestData> testData = default;

        public void Init(IEcsSystems systems)
        {

        }

        public void Run(IEcsSystems systems)
        {
            UnityEngine.Debug.LogWarning(testData.Value.testText);
        }
    }
}