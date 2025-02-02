
using Leopotam.EcsLite;
using Leopotam.EcsLite.Threads;

namespace SO.Colonization
{
    public struct TColonizationProgressCalc : IEcsThread<
        CLandColony>
    {
        int[] landEntities;

        CLandColony[] landColonyPool;
        int[] landColonyIndices;

        public void Init(
            int[] entities,
            CLandColony[] pool1, int[] indices1)
        {
            landEntities = entities;

            landColonyPool = pool1;
            landColonyIndices = indices1;
        }

        public void Execute(int threadId, int fromIndex, int beforeIndex)
        {
            //Для каждой колонии
            for(int a = fromIndex; a < beforeIndex; a++)
            {
                //Берём колонию
                int landEntity = landEntities[a];
                ref CLandColony landColony = ref landColonyPool[landColonyIndices[landEntity]];

                //Увеличиваем прогресс колонизации
                landColony.AddProgress(500);
            }
        }
    }
}
