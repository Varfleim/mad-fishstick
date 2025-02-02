
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using SO.LandOwnership;

namespace SO.Colonization
{
    public class SColonizationEnd : IEcsRunSystem
    {
        readonly EcsWorldInject world = default;


        readonly EcsPoolInject<CAgentColoniesOwner> aColoniesOwnerPool = default;

        readonly EcsFilterInject<Inc<CLandColony>> landColonyFilter = default;
        readonly EcsPoolInject<CLandColony> landColonyPool = default;

        readonly EcsPoolInject<RLandChangeOwner> landChangeOwnerRPool = default;


        readonly EcsCustomInject<ColonizationData> colonizationData = default;

        public void Run(IEcsSystems systems)
        {
            //Проверяем, не требуется ли окончание колонизации в колониях
            ColoniesCheck();
        }

        void ColoniesCheck()
        {
            //Для каждой колонии
            foreach(int landEntity in landColonyFilter.Value)
            {
                //Берём колонию
                ref CLandColony landColony = ref landColonyPool.Value.Get(landEntity);

                //Проверяем, не требуется ли окончание колонизации
                ColonyCheck(ref landColony);
            }
        }

        void ColonyCheck(
            ref CLandColony landColony)
        {
            //Если прогресс колонизации больше максимального или равен
            if(landColony.ColonizationProgress >= colonizationData.Value.colonizationProgressMax)
            {
                //Обрабатываем окончание колонизации
                ColonizationFinish(ref landColony);

                //Удаляем колонию
                ColonyRemove(ref landColony);
            }
            //Иначе, если прогресс колонизации меньше нуля
            else if(landColony.ColonizationProgress < 0)
            {
                //Обрабатываем отмену колонизации
                ColonizationCancel(ref landColony);

                //Удаляем колонию
                ColonyRemove(ref landColony);
            }
        }

        void ColonizationFinish(
            ref CLandColony landColony)
        {
            //Запрашиваем смену владельца земли
            LandOwnershipData.LandChangeOwnerRequest(
                world.Value,
                landChangeOwnerRPool.Value,
                landColony.ownerPE,
                landColony.selfPE);

            //Удаляем колонию из списка владельца
            ColonizationData.ColoniesOwnerRemoveColony(
                world.Value,
                aColoniesOwnerPool.Value,
                ref landColony);
        }

        void ColonizationCancel(
            ref CLandColony landColony)
        {
            //Удаляем колонию из списка владельца
            ColonizationData.ColoniesOwnerRemoveColony(
                world.Value,
                aColoniesOwnerPool.Value,
                ref landColony);
        }

        void ColonyRemove(
            ref CLandColony landColony)
        {
            //Берём сущность колонии
            landColony.selfPE.Unpack(world.Value, out int landEntity);

            //Удаляем компонент колонии
            landColonyPool.Value.Del(landEntity);

            UnityEngine.Debug.LogWarning("Colony Removed!");
        }
    }
}
