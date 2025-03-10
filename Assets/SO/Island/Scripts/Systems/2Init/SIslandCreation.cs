
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace SO.Island
{
    public class SIslandCreation : IEcsInitSystem
    {
        readonly EcsWorldInject world = default;


        readonly EcsPoolInject<CIsland> islandPool = default;

        readonly EcsFilterInject<Inc<SRIslandCreation>> islandCreationSRFilter = default;
        readonly EcsPoolInject<SRIslandCreation> islandCreationSRPool = default;

        public void Init(IEcsSystems systems)
        {
            //Создаём острова
            IslandsCreation();
        }

        void IslandsCreation()
        {
            //Для каждого запроса создания острова
            foreach (int islandRequestEntity in islandCreationSRFilter.Value)
            {
                //Берём запрос
                ref SRIslandCreation requestComp = ref islandCreationSRPool.Value.Get(islandRequestEntity);

                //Создаём остров
                IslandCreation(
                    ref requestComp,
                    islandRequestEntity);

                //Берём остров
                ref CIsland island = ref islandPool.Value.Get(islandRequestEntity);

                //Удаляем запрос
                islandCreationSRPool.Value.Del(islandRequestEntity);
            }
        }

        void IslandCreation(
            ref SRIslandCreation requestComp,
            int islandEntity)
        {
            //Назначаем переданной сущности компонент острова
            ref CIsland island = ref islandPool.Value.Add(islandEntity);

            //Заполняем основные данные острова
            island = new(
                world.Value.PackEntity(islandEntity), requestComp.islandName);
        }
    }
}
